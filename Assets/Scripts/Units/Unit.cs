using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    protected float moveSpeedCopy;
    protected bool canAttack = true;
    public enum State{
        Neutral,
        Aggro,
        Fighting,
        Shooting
    };

    protected virtual void Start()
    {
        moveSpeedCopy = moveSpeed;
    }

    protected virtual void CriticalEffect()
    {
        return;
    }

    protected virtual void AttackEffect()
    {
        return;
    }

    protected virtual void Death()
    {
        Destroy(gameObject);
    }

    public virtual bool AboutToDie(float attackDamagePhysic, float attackDamageMagic)
    {
        return health < Mathf.Max(0, attackDamagePhysic - physicRes) + Mathf.Max(0, attackDamageMagic - magicRes);
    }

    public virtual void TakeDamage(float attackDamagePhysic, float attackDamageMagic)
    {
        health -= Mathf.Max(0, attackDamagePhysic - physicRes) + Mathf.Max(0, attackDamageMagic - magicRes);
        if(health <= 0){
            Death();
        }
    }

    public IEnumerator AttackUnit(GameObject unitToFight)
    {
        if(canAttack && unitToFight != null){
            canAttack = false;
            if(unitToFight.GetComponent<Unit>().AboutToDie(attackDamagePhysic, attackDamageMagic)){
                CriticalEffect();
            }else{
                AttackEffect();
            }
            unitToFight.GetComponent<Unit>().TakeDamage(attackDamagePhysic, attackDamageMagic);
            yield return new WaitForSeconds(attackCD);
            canAttack = true;
        }
    }

    public void FlipDirection(Vector2 targetPos)
    {
        Vector2 dir = (targetPos - (Vector2)transform.position).normalized;
        
        GetComponent<SpriteRenderer>().flipX = dir.x < 0;
    }

    public void Upgrade(float moveSpeed, float health, float attackRange, float attackDamagePhysic, 
    float attackDamageMagic, float attackCD, float physicRes, float magicRes)
    {
        this.moveSpeed = moveSpeed;
        this.health = health;
        this.attackRange = attackRange;
        this.attackDamagePhysic = attackDamagePhysic;
        this.attackDamageMagic = attackDamageMagic;
        this.attackCD = attackCD;
        this.physicRes = physicRes;
        this.magicRes = magicRes;
    }

    public virtual void ChangeMoveSpeed(float changeValue)
    {
        moveSpeed += changeValue;
    }
}
