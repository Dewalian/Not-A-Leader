using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Unit : MonoBehaviour
{
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float health;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float attackDamage;
    [SerializeField] protected float attackCD;
    [SerializeField] protected NavMeshAgent agent;
    protected bool canAttack = true;
    public enum State{
        Neutral,
        Aggro,
        Fighting
    };

    protected virtual void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = moveSpeed;
    }

    protected virtual void Death()
    {
        Destroy(gameObject);
    }

    public virtual void TakeDamage(float attackDamage){
        health -= attackDamage;
        if(health <= 0){
            Death();
        }
    }

    public IEnumerator AttackUnit(GameObject unitToFight)
    {
        if(canAttack && unitToFight != null){
            canAttack = false;
            unitToFight.GetComponent<Unit>().TakeDamage(attackDamage);
            yield return new WaitForSeconds(attackCD);
            canAttack = true;
        }
    }
}
