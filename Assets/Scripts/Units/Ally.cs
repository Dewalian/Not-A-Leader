using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ally : Unit
{
    [SerializeField] private NavMeshAgent agent;
    private AllyArea allyArea;
    private GameObject enemy;
    private float healthCopy;
    public Animator animator;
    public Transform originalPos;
    public State allyState;
    public bool isDuel = false;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected override void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = moveSpeed;

        agent.radius = 0.1f;
        agent.acceleration = 50f;
        
        StartCoroutine(WalkToTarget(originalPos.position));
        healthCopy = health;

        allyArea = GetComponentInParent<AllyArea>();
        allyArea.OnMoveArea.AddListener(() => RemoveFromFight());
        // allyArea.OnMoveArea.AddListener(() => FlipDirection(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
    }

    private void Update()
    {
        if(allyState == State.Aggro){
            agent.isStopped = false;
            animator.SetBool("BoolWalk", true);
            AggroEnemy();
        }else if(allyState == State.Fighting){
            StartCoroutine(AttackUnit(enemy));
            agent.isStopped = true;
            animator.SetBool("BoolWalk", false);
        }else if(allyState == State.Neutral){
            agent.isStopped = false;
            StartCoroutine(WalkToTarget(originalPos.position));
        }

        if(enemy == null){
            allyState = State.Neutral;
        }
    }

    //TEST soalnya enemy belum ada animasi
    protected override void AttackEffect()
    {
        animator.SetTrigger("TriggerAttack");
    }
    //TEST

    private void AggroEnemy()
    {
        if(enemy != null){
            if(Vector2.Distance(transform.position, enemy.transform.position) >= attackRange){
                agent.SetDestination(enemy.transform.position);
                animator.SetBool("BoolWalk", true);
            }else{
                allyState = State.Fighting;
            }
        }else{
            allyState = State.Neutral;
        }
    }

    protected override void Death()
    {
        animator.SetTrigger("TriggerDeath");
    }

    public void DeathAnimator()
    {
        RemoveFromFight();
        health = healthCopy;
        canAttack = true;
        allyArea.StartRespawn(gameObject);
        gameObject.SetActive(false);
    }

    public IEnumerator WalkToTarget(Vector2 targetPos)
    {
        agent.SetDestination(targetPos);
        FlipDirection(targetPos);
        animator.SetBool("BoolWalk", true);
        yield return new WaitUntil(() => Vector2.Distance(transform.position, targetPos) < 0.1f);
        animator.SetBool("BoolWalk", false);
    }

    public void SetTarget(GameObject enemy)
    {
        RemoveFromFight();
        FlipDirection(enemy.transform.position);
        this.enemy = enemy;
        enemy.GetComponent<Enemy>().AddUnitToFightArr(gameObject);
        allyState = State.Aggro;
    }

    public void RemoveFromFight()
    {
        if(enemy != null){
            enemy.GetComponent<Enemy>().RemoveUnitFromFightArr(gameObject);
            allyState = State.Neutral;
        }
    }

    public void TeleportToOriginalPos()
    {
        transform.position = originalPos.transform.position;
    }

    public virtual void SwitchSide()
    {
        gameObject.AddComponent<Enemy>();
        GetComponent<Enemy>().Upgrade(moveSpeed, health, attackRange, attackDamagePhysic, attackDamageMagic, attackCD,
        physicRes, magicRes);
        gameObject.tag = "Enemy";
        gameObject.layer = 6;
        Destroy(GetComponent<NavMeshAgent>());
        Destroy(this);
    }

    public override void ChangeMoveSpeed(float changeValue)
    {
        base.ChangeMoveSpeed(changeValue);
        agent.speed = moveSpeed;
    }
}
