using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float health = 100f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackDamage = 50f;
    [SerializeField] private float attackCD = 1.5f;
    [SerializeField] private LayerMask playerLayer;
    
    private bool canAttack = true;
    private Transform target;
    private List<GameObject> unitsToFight = new List<GameObject>();
    private int movePointIndex = 0;
    // private int index = 0;
    public enum State{
        Walking,
        Fighting
    };
    public State enemyState;

    void Update()
    {
        if(enemyState == State.Walking){
            moveSpeed = 5f;
        }else if(enemyState == State.Fighting){
            moveSpeed = 0f;
            if(unitsToFight.Count > 0 && Vector2.Distance(transform.position, unitsToFight[0].transform.position) <= attackRange){
                StartCoroutine(AttackUnit(unitsToFight[0]));
            }
        }

        if(unitsToFight.Count == 0){
            enemyState = State.Walking;
        }

        target = LevelManager.instance.movePoint[movePointIndex];
        MoveToMovePoint();
        ChangeTargetPos();

        if(health <= 0){
            Destroy(gameObject);
        }
    }

    void ChangeTargetPos(){
        if(Vector2.Distance(target.position, transform.position) <= 0.1f){
            movePointIndex++;
            if(movePointIndex == LevelManager.instance.movePoint.Length){
                Destroy(gameObject);
            }else{
                target = LevelManager.instance.movePoint[movePointIndex];
            }
        }
    }

    void MoveToMovePoint(){
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
    }

    IEnumerator AttackUnit(GameObject unitToFight){
        if(canAttack){
            canAttack = false;
            unitToFight.GetComponent<Unit>().TakeDamage(attackDamage);
            yield return new WaitForSeconds(attackCD);
            canAttack = true;
        }
    }

    public void TakeDamage(float damage){
        health -= damage;
    }

    public void ChangeStateToFighting(){
        enemyState = State.Fighting;
    }

    public void AddUnitToFightArr(GameObject unitToFight){
        this.unitsToFight.Add(unitToFight);
    }

    public void RemoveUnitToFightArr(GameObject unitToFight){
        this.unitsToFight.Remove(unitToFight);
    }
}
