using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ally : Unit
{
    private NavMeshAgent agent;
    private AllyArea allyArea;
    private GameObject enemy;
    private float healthCopy;
    public Transform originalPos;
    public bool isDuel = false;

    protected override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
    }

    protected override void Start()
    {
        base.Start();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = moveSpeed;

        agent.radius = 0.1f;
        agent.acceleration = 50f;
        
        StartCoroutine(WalkToTarget(originalPos.position));
        healthCopy = health;

        allyArea = GetComponentInParent<AllyArea>();
        allyArea.OnMoveArea.AddListener(() => RemoveFromFight());
    }

    protected virtual void Update()
    {
        StateChange();

        agent.speed = moveSpeed;
    }

    protected override void StateChange()
    {
        base.StateChange();

        if(unitState == State.Aggro){
            agent.isStopped = false;
            animator.SetBool("BoolWalk", true);
            AggroEnemy();
        }else if(unitState == State.Fighting){
            StartCoroutine(AttackUnit(enemy));
            agent.isStopped = true;
            animator.SetBool("BoolWalk", false);
        }else if(unitState == State.Neutral){
            agent.isStopped = false;
            StartCoroutine(WalkToTarget(originalPos.position));
        }

        if(enemy == null && unitState != State.Death){
            unitState = State.Neutral;
        }
    }

    private void AggroEnemy()
    {
        if(enemy != null){
            if(Vector2.Distance(transform.position, enemy.transform.position) >= attackRange){
                agent.SetDestination(enemy.transform.position);
                animator.SetBool("BoolWalk", true);
            }else{
                unitState = State.Fighting;
            }
        }else{
            unitState = State.Neutral;
        }
    }

    public override void Death()
    {
        RemoveFromFight();
        animator.SetBool("BoolDeath", false);
        health = healthCopy;
        canAttack = true;
        allyArea.StartRespawn(gameObject);
        gameObject.SetActive(false);
    }

    public override void ChangeStats(float changePercentage)
    {
        base.ChangeStats(changePercentage);
    }

    public IEnumerator WalkToTarget(Vector2 targetPos)
    {
        agent.SetDestination(targetPos);
        FlipDirection(targetPos);
        animator.SetBool("BoolWalk", true);
        yield return new WaitUntil(() => Vector2.Distance(transform.position, targetPos) < attackRange);
        animator.SetBool("BoolWalk", false);
    }

    public void SetTarget(GameObject enemy)
    {
        RemoveFromFight();
        FlipDirection(enemy.transform.position);
        this.enemy = enemy;
        enemy.GetComponent<Enemy>().AddUnitToFightArr(gameObject);
        unitState = State.Aggro;
    }

    public void RemoveFromFight()
    {
        if(enemy != null){
            enemy.GetComponent<Enemy>().RemoveUnitFromFightArr(gameObject);
            unitState = State.Neutral;
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
}
