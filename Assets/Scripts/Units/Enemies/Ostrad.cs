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
    private Collider2D[] unitInWarScream;
    private Collider2D[] towerInEarthShaker;
    private bool canWarScream;
    private bool canEarthShaker;

    protected override void Start()
    {
        base.Start();
        canEarthShaker = true;
    }

    protected override void Update()
    {
        base.Update();
        WarScreamDetect();
        EarthShakerDetect();
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
            if(unitInWarScream.Length >= unitCountTrigger){
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
            if(towerInEarthShaker.Length >= towerCountTrigger){
                unitState = State.Skill;
                animator.SetTrigger("TriggerEarthShaker");
                canEarthShaker = false;
            }
        }
    }

    private void EarthShakerAnimator()
    {
        foreach(Collider2D t in towerInEarthShaker){
            t.GetComponent<Tower>().DestroyTower();
        }
        StartCoroutine(EarthShakerCD());
    }

    private IEnumerator EarthShakerCD()
    {
        yield return new WaitForSeconds(earthShakerCD);
        canEarthShaker = true;
    }

    private void NeutralAnimator()
    {
        unitState = State.Neutral;
    }
}
