using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ostrad : Enemy
{
    [SerializeField] private float warScreamRange;
    [SerializeField] private float warScreamCD;
    [SerializeField] private int unitCountTrigger;
    [SerializeField] private float earthShakerRange;
    [SerializeField] private float earthShakerCD;
    [SerializeField] private int towerCountTrigger;
    [SerializeField] private int throwFlagLimit;
    [SerializeField] private float[] throwFlagTriggerHealthPercentages;
    [SerializeField] private float throwFlagDuration;
    [SerializeField] private float jumpDuration;
    [SerializeField] private float maxJumpHeight;
    [SerializeField] private Transform shadowStartPos;
    [SerializeField] private AnimationCurve trajectoryCurve;
    [SerializeField] private GameObject shadow;
    [SerializeField] private GameObject flag;
    [SerializeField] private Transform flagStart;
    [SerializeField] private Transform flagStartShadow;
    private int throwFlagTriggerIndex;
    private GameObject flagObj;
    private bool isJumping;
    private Collider2D[] unitInWarScream;
    private Collider2D[] towerInEarthShaker;
    private bool canWarScream;
    private bool canEarthShaker;

    protected override void Start()
    {
        base.Start();
        canEarthShaker = true;
        isJumping = false;
        throwFlagTriggerIndex = 0;
    }

    protected override void Update()
    {
        base.Update();
        WarScreamDetect();
        EarthShakerDetect();
        JumpDetect();
    }

    protected override void StateChange()
    {
        if(unitState == State.Skill){
            animator.SetBool("BoolWalk", false);
            moveSpeed = 0;
            return;
        }
        base.StateChange();
    }

    private void WarScreamDetect()
    {
        if(canWarScream){
            unitInWarScream = Physics2D.OverlapCircleAll(transform.position, warScreamRange, LayerMask.GetMask("Enemy", "Ally"));
            if(unitInWarScream.Length >= unitCountTrigger && !canEarthShaker && !isJumping){
                unitState = State.Skill;
                animator.SetTrigger("TriggerWarScream");
                canWarScream = false;
            }
        }
    }

    private void WarScreamAnimator()
    {
        foreach(Collider2D u in unitInWarScream){
            u.GetComponent<Unit>().Death();
        }
        unitState = State.Neutral;
        StartCoroutine(WarScreamCD());
    }

    private IEnumerator WarScreamCD()
    {
        yield return new WaitForSeconds(warScreamCD);
        canWarScream = true;
    }

    private void EarthShakerDetect()
    {
        if(canEarthShaker){
            towerInEarthShaker = Physics2D.OverlapCircleAll(transform.position, earthShakerRange, LayerMask.GetMask("Tower"));
            if(towerInEarthShaker.Length >= towerCountTrigger && !isJumping){
                unitState = State.Skill;
                animator.SetTrigger("TriggerEarthShaker");
                canEarthShaker = false;
            }
        }
    }

    private void EarthShakerAnimator()
    {
        foreach(Collider2D t in towerInEarthShaker){
            Debug.Log("Test");
            t.GetComponent<Tower>().DestroyTower();
        }
        StartCoroutine(EarthShakerCD());
    }

    private IEnumerator EarthShakerCD()
    {
        yield return new WaitForSeconds(earthShakerCD);
        canEarthShaker = true;
    }

    private void JumpDetect()
    {
        if(throwFlagTriggerIndex < throwFlagLimit && 
        health <= healthCopy * throwFlagTriggerHealthPercentages[throwFlagTriggerIndex] / 100 && !isJumping){
            isJumping = true;
            unitState = State.Skill;
            animator.SetTrigger("TriggerThrow");
        }
    }

    private IEnumerator Jump()
    {
        float timePassed = 0;
        Vector2 startPos = transform.position;
        Vector2 shadowStartPosCopy = shadowStartPos.position;
        shadow.SetActive(true);

        while(timePassed < jumpDuration)
        {
            timePassed += Time.deltaTime;
            
            float durationNorm = timePassed / jumpDuration;
            float height = Mathf.Lerp(0, maxJumpHeight, trajectoryCurve.Evaluate(durationNorm));

            transform.position = Vector2.Lerp(startPos, LevelManager.Instance.ostradFlagPos[throwFlagTriggerIndex].position, durationNorm) + new Vector2(0, height);
            shadow.transform.position = Vector2.Lerp(shadowStartPosCopy, LevelManager.Instance.ostradFlagPos[throwFlagTriggerIndex].position, durationNorm);

            yield return null;
        }

        shadow.SetActive(false);
        animator.SetTrigger("TriggerLand");
    }

    private void ThrowFlagAnimator()
    {
        ThrowFlag(LevelManager.Instance.ostradFlagPos[throwFlagTriggerIndex]);
    }

    private IEnumerator WaitJump()
    {
        yield return new WaitForSeconds(throwFlagDuration);
        animator.SetTrigger("TriggerJump");
    }

    private void JumpAnimator()
    {
        StartCoroutine(Jump());
    }

    private void LandAnimator()
    {
        Destroy(flagObj);

        transform.SetParent(LevelManager.Instance.ostradSpawners[throwFlagTriggerIndex].transform);
        enemySpawner = GetComponentInParent<EnemySpawner>();
        wayPointIndex = 1;

        throwFlagTriggerIndex++;
        isJumping = false;

        NeutralAnimator();
    }

    private void ThrowFlag(Transform flagPos)
    {
        FlipDirection(flagPos.position);
        flagObj = Instantiate(flag, flagStart.position, Quaternion.identity);
        flagObj.GetComponent<OstradFlag>().InitVariables(LevelManager.Instance.ostradFlagPos[throwFlagTriggerIndex], flagStart, flagStartShadow,
        0, 0, throwFlagDuration);

        StartCoroutine(WaitJump());
    }

    private void NeutralAnimator()
    {
        unitState = State.Neutral;
    }

    public override void DeathAnimator()
    {
        base.DeathAnimator();
        LevelManager.Instance.Win();
    }
}
