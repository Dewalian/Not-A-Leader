using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Ally : Unit
{
    [SerializeField] protected float healthRegenDelay;
    protected bool isStartRegen = true;
    private NavMeshAgent agent;
    private AllyArea allyArea;
    private GameObject enemy;
    private Coroutine healthRegenCoroutine;
    public Transform originalPos;
    [HideInInspector] public bool isDuel = false;

    protected override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        allyArea = GetComponentInParent<AllyArea>();
    }

    protected virtual void OnEnable()
    {
        allyArea.OnMoveArea.AddListener(() => {
            RemoveFromFight();
            if(unitState != State.Skill){
                unitState = State.Neutral;
            }
        });

        OnHealthChanged?.Invoke();
    }
    
    protected override void OnDisable()
    {
        allyArea.OnMoveArea.RemoveAllListeners();
        health = healthCopy;
        canAttack = true;
        moveSpeed = moveSpeedCopy;
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
    }

    protected override void Update()
    {
        base.Update();
        agent.speed = moveSpeed;
    }

    protected override void StateChange()
    {
        if(unitState == State.Death){
            return;
        }

        if(unitState == State.Aggro){
            agent.isStopped = false;

            if(animator) animator.SetBool("BoolWalk", true);
            AggroEnemy();

            isStartRegen = false;
            if(healthRegenCoroutine != null) { StopCoroutine(healthRegenCoroutine); }
            
        }else if(unitState == State.Fighting){
            agent.isStopped = true;

            if(animator) animator.SetBool("BoolWalk", false);
            if(enemy!= null) StartCoroutine(AttackUnit(enemy));
            
        }else if(unitState == State.Neutral){
            agent.isStopped = false;
            moveSpeed = moveSpeedCopy;

            StartCoroutine(WalkToTarget(originalPos.position));

            if(!isStartRegen && health < healthCopy)
            {
                healthRegenCoroutine = StartCoroutine(HealthRegen());
            }
        }

        if(enemy == null && unitState != State.Death && !isAttackAnimation){
            unitState = State.Neutral;
        }
    }

    private void AggroEnemy()
    {
        if(enemy != null){
            if(Vector2.Distance(transform.position, enemy.transform.position) >= attackRange){
                agent.SetDestination(enemy.transform.position);
                if(animator) animator.SetBool("BoolWalk", true);
            }else{
                unitState = State.Fighting;
            }
        }
    }

    protected virtual IEnumerator HealthRegen()
    {
        isStartRegen = true;
        yield return new WaitForSeconds(healthRegenDelay);

        while(health < healthCopy){
            health += Mathf.Min(healthRegen, healthCopy - health);
            OnHealthChanged?.Invoke();
            yield return new WaitForSeconds(1f);
        }  

        isStartRegen = false;
    }

    public override void TakeDamage(float attackDamagePhysic, float attackDamageMagic)
    {
        base.TakeDamage(attackDamagePhysic, attackDamageMagic);

        if(healthRegenCoroutine != null) { StopCoroutine(healthRegenCoroutine); }
    }

    public override void Death()
    {
        RemoveFromFight();
        base.Death();
    }

    public override void DeathAnimator()
    {
        //RemoveFromFight();
        animator.SetBool("BoolDeath", false);
        
        allyArea.StartRespawn(gameObject);
        gameObject.SetActive(false);
    }

    public IEnumerator WalkToTarget(Vector2 targetPos)
    {
        if(Vector2.Distance(transform.position, targetPos) > 0.1f){
            agent.SetDestination(targetPos);
            FlipDirection(targetPos);
            if(animator) animator.SetBool("BoolWalk", true);
            yield return new WaitUntil(() => Vector2.Distance(transform.position, targetPos) < 0.1f);
            if(animator) animator.SetBool("BoolWalk", false);
        }
    }

    public void SetTarget(GameObject enemy)
    {
        if(unitState != State.Skill){
            RemoveFromFight();
            FlipDirection(enemy.transform.position);
            this.enemy = enemy;
            enemy.GetComponent<Enemy>().AddUnitToFightArr(gameObject);
            unitState = State.Aggro;
        }
    }

    public void RemoveFromFight()
    {
        if(enemy != null && health <= 0){
            enemy.GetComponent<Enemy>().RemoveUnitFromFightArr(gameObject);
        }
    }

    public void TeleportToOriginalPos()
    {
        transform.position = originalPos.transform.position;
    }
}
