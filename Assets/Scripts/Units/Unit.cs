using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float health;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float attackDamagePhysic;
    [SerializeField] protected float attackDamageMagic;
    [SerializeField] protected float attackCD;
    [SerializeField] protected float physicRes;
    [SerializeField] protected float magicRes;
    protected bool canAttack = true;
    public enum State{
        Neutral,
        Aggro,
        Fighting,
        Shooting
    };

    protected virtual void Death()
    {
        Destroy(gameObject);
    }

    public virtual void TakeDamage(float attackDamagePhysic, float attackDamageMagic){
        health -= Mathf.Max(0, attackDamagePhysic - physicRes) + Mathf.Max(0, attackDamageMagic - magicRes);
        if(health <= 0){
            Death();
        }
    }

    public IEnumerator AttackUnit(GameObject unitToFight)
    {
        if(canAttack && unitToFight != null){
            canAttack = false;
            unitToFight.GetComponent<Unit>().TakeDamage(attackDamagePhysic, attackDamageMagic);
            yield return new WaitForSeconds(attackCD);
            canAttack = true;
        }
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
        Debug.Log("Test");
    }

    //test
}
