using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Unit : MonoBehaviour
{
    public float health;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float attackDamagePhysic;
    [SerializeField] protected float attackDamageMagic;
    [SerializeField] protected float attackCD;
    [SerializeField] protected float physicRes;
    [SerializeField] protected float magicRes;
    [SerializeField] protected float healthRegen;
    protected Animator animator;
    protected float healthCopy;
    protected float moveSpeedCopy;
    protected float attackDamagePhysicCopy;
    protected float attackDamageMagicCopy;
    protected float attackCDCopy;
    protected float physicResCopy;
    protected float magicResCopy;
    protected float healthRegenCopy;
    protected bool canAttack = true;
    public enum State{
        Neutral,
        Aggro,
        Fighting,
        Shooting,
        Skill,
        Death
    };
    public State unitState;
    public UnityEvent OnHealthChanged;
    private Unit unitTarget;


    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        UpdateStatsCopy();
    }

    protected virtual void CriticalEffect()
    {
        return;
    }

    protected virtual void AttackEffect()
    {
        return;
    }

    protected virtual void StateChange()
    {
        if(health <= 0){
            unitState = State.Death;
        }

        if(unitState == State.Death){
            Death();
            return;
        }

        if(unitState == State.Skill){
            moveSpeed = 0;
        }
    }

    public virtual void Death()
    {
        animator.SetBool("BoolWalk", false);
        animator.SetBool("BoolDeath", true);
        moveSpeed = 0;
        GetComponent<Collider2D>().enabled = false;
    }

    public virtual void DeathAnimator()
    {
        Destroy(gameObject);
    }

    public virtual bool AboutToDie(float attackDamagePhysic, float attackDamageMagic)
    {
        return health < Mathf.Max(1, attackDamagePhysic - physicRes) + Mathf.Max(1, attackDamageMagic - magicRes);
    }

    public virtual void TakeDamage(float attackDamagePhysic, float attackDamageMagic)
    {
        health -= Mathf.Max(1, attackDamagePhysic - physicRes) + Mathf.Max(1, attackDamageMagic - magicRes);
        OnHealthChanged?.Invoke();
    }

    protected IEnumerator AttackUnit(GameObject unit)
    {
        this.unitTarget = unit.GetComponent<Unit>();;

        if(canAttack && unitTarget != null){
            canAttack = false;
            if(animator){
                animator.SetTrigger("TriggerAttack");
            }else{
                DamageUnitAnimator();
            }

            if(unitTarget.AboutToDie(attackDamagePhysic, attackDamageMagic)){
                CriticalEffect();
            }else{
                AttackEffect();
            }
            yield return new WaitForSeconds(attackCD);
            canAttack = true;
        }
    }

    public void DamageUnitAnimator()
    {
        unitTarget.TakeDamage(attackDamagePhysic, attackDamageMagic);
    }

    public void FlipDirection(Vector2 targetPos)
    {
        Vector2 dir = (targetPos - (Vector2)transform.position).normalized;
        
        GetComponent<SpriteRenderer>().flipX = dir.x < 0;
    }

    public void Upgrade(float moveSpeed, float health, float attackRange, float attackDamagePhysic, 
    float attackDamageMagic, float attackCD, float physicRes, float magicRes, float healthRegen)
    {
        this.moveSpeed = moveSpeed;
        this.health = health;
        this.attackRange = attackRange;
        this.attackDamagePhysic = attackDamagePhysic;
        this.attackDamageMagic = attackDamageMagic;
        this.attackCD = attackCD;
        this.physicRes = physicRes;
        this.magicRes = magicRes;
        this.healthRegen = healthRegen;

        UpdateStatsCopy();
    }

    public void UpdateStatsCopy()
    {
        healthCopy = health;
        moveSpeedCopy = moveSpeed;
        attackDamagePhysicCopy = attackDamagePhysic;
        attackDamageMagicCopy = attackDamageMagic;
        attackCDCopy = attackCD;
        physicResCopy = physicRes;
        magicResCopy = magicRes;
    }

    public virtual void ChangeStats(float changePercentage)
    {
        changePercentage /= 100;

        moveSpeed += moveSpeedCopy * changePercentage;
        attackDamagePhysic += attackDamagePhysicCopy * changePercentage;
        attackDamageMagic += attackDamageMagicCopy * changePercentage;
        attackCD -= attackCDCopy * changePercentage;
        physicRes += physicResCopy * changePercentage;
        magicRes += magicResCopy * changePercentage;
    }
}
