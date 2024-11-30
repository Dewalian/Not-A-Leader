using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Unit : MonoBehaviour
{
    public string unitName;
    public  float health;
    public float moveSpeed;
    public float attackRange;
    public float attackDamagePhysic;
    public float attackDamageMagic;
    public float attackCD;
    public float physicRes;
    public float magicRes;
    public float healthRegen;
    protected Animator animator;
    protected float healthCopy;
    protected float moveSpeedCopy;
    protected float attackDamagePhysicCopy;
    protected float attackDamageMagicCopy;
    protected float attackCDCopy;
    protected float physicResCopy;
    protected float magicResCopy;
    protected float healthRegenCopy;
    [HideInInspector] protected bool canAttack = true;
    public enum State{
        Neutral,
        Aggro,
        Fighting,
        Shooting,
        Skill,
        Death
    };
    protected Unit unitTarget;
    protected bool isAttackAnimation;
    public State unitState;
    [HideInInspector] public UnityEvent OnSwitch;
    [HideInInspector] public UnityEvent OnHealthChanged;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        UpdateStatsCopy();
    }

    protected virtual void Update()
    {
        StateChange();
    }

    protected virtual void FatalEffect()
    {
        return;
    }

    // protected virtual void AttackEffect()
    // {
    //     return;
    // }

    protected virtual void StateChange()
    {
        return;
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
        return health <= Mathf.Max(1, attackDamagePhysic - physicRes) + Mathf.Max(1, attackDamageMagic - magicRes);
    }

    public virtual void TakeDamage(float attackDamagePhysic, float attackDamageMagic)
    {
        if(unitState != State.Death){
            health -= Mathf.Max(1, attackDamagePhysic - physicRes) + Mathf.Max(1, attackDamageMagic - magicRes);
            OnHealthChanged?.Invoke();

            if(health <= 0){
                unitState = State.Death;
                Death();
            }
        }
    }

    protected virtual IEnumerator AttackUnit(GameObject unit)
    {
        unitTarget = unit.GetComponent<Unit>();

        if(canAttack && unitTarget != null && attackCD > 0){
            isAttackAnimation = true;

            canAttack = false;
            moveSpeed = 0;

            animator.SetTrigger("TriggerAttack");

            yield return new WaitForSeconds(attackCD);
            canAttack = true;
        }
    }

    public void FinishAttackAnimator()
    {
        isAttackAnimation = false;
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

    public IEnumerator ChangeStatsTimed(float changePercentage, float time)
    {
        ChangeStats(changePercentage);
        yield return new WaitForSeconds(time);
        ChangeStats(-changePercentage);
    }

    public IEnumerator ChangeColor(float duration, Color color){
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = color;
        yield return new WaitForSeconds(duration);
        spriteRenderer.color = Color.white;
    }
}
