using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
        allyArea.OnMoveArea.AddListener(() => RemoveFromFight());
        OnHealthChanged?.Invoke();
    }
    
    protected virtual void OnDisable()
    {
        allyArea.OnMoveArea.RemoveListener(() => RemoveFromFight());
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

        if(enemy == null && unitState != State.Death){
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

    public override void DeathAnimator()
    {
        RemoveFromFight();
        animator.SetBool("BoolDeath", false);
        health = healthCopy;
        canAttack = true;
        moveSpeed = moveSpeedCopy;
        allyArea.StartRespawn(gameObject);
        gameObject.SetActive(false);
    }

    public override void ChangeStats(float changePercentage)
    {
        base.ChangeStats(changePercentage);
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
        physicRes, magicRes, healthRegen);
        gameObject.tag = "Enemy";
        gameObject.layer = 6;
        Destroy(GetComponent<NavMeshAgent>());
        Destroy(this);
    }
}
