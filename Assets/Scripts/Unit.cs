using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float health = 100f;
    [SerializeField] private float attackDamage = 15f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackCD = 1.5f;
    [SerializeField] private float respawnTime = 3f;
    [SerializeField] private GameObject barrackPos;
    private Vector2 startingPos;
    private bool canAttack = true;
    private Collider2D enemyToAggro;
    private Enemy enemyScript;
    private Collider2D unitInRange;
    private bool addedToFightList = false;
    public enum State{
        Standing,
        Aggroing,
        Fighting
    };
    public State unitState;

    void Start()
    {
        startingPos = transform.position;
    }

    void Update()
    {
        if(unitState == State.Standing){
            enemyToAggro = unitInRange;
            if(enemyToAggro){
                enemyScript = enemyToAggro.GetComponent<Enemy>();
            }
        }if(unitState == State.Fighting){
            StartCoroutine(AttackEnemy());
        }

        if(enemyToAggro && unitState != State.Fighting){
            unitState = State.Aggroing;
            StartCoroutine(ToAggro());
        }else if(!enemyToAggro){
            unitState = State.Standing;
        }

        if(enemyToAggro == null){
            MoveToStartPosition();
        }

        if(health <= 0){
            Death();
            StartCoroutine(Respawn());
        }
    }

    IEnumerator ToAggro(){
        enemyScript.ChangeStateToFighting();
        AddToFightList();
        if(Vector2.Distance(transform.position, enemyToAggro.transform.position) >= attackRange){
            transform.position = Vector2.MoveTowards(transform.position, enemyToAggro.transform.position, moveSpeed * Time.deltaTime);
        }
        yield return new WaitUntil(() => Vector2.Distance(transform.position, enemyToAggro.transform.position) <= attackRange);
        unitState = State.Fighting;
    }

    IEnumerator AttackEnemy(){
        if(canAttack){
            canAttack = false;
            enemyScript.TakeDamage(attackDamage);
            yield return new WaitForSeconds(attackCD);
            canAttack = true;
        }
    }

    IEnumerator Respawn(){
        yield return new WaitForSeconds(respawnTime);
        health = 100f;
        unitState = State.Standing;
    }

    void Death(){
        if(enemyToAggro){
            RemoveFromFightList();
        }
        transform.position = barrackPos.transform.position;
    }

    void AddToFightList(){
        if(!addedToFightList){
            enemyScript.AddUnitToFightArr(gameObject);
            addedToFightList = true;
        }
    }

    void RemoveFromFightList(){
        enemyScript.RemoveUnitToFightArr(gameObject);
        addedToFightList = false;
    }

    public void MoveToStartPosition(){
        transform.position = Vector2.MoveTowards(transform.position, startingPos, moveSpeed * Time.deltaTime);
    }

    public void TakeDamage(float damage){
        health -= damage;
    }

    public void EnemyToAggro(Collider2D unitInRange){
        this.unitInRange = unitInRange;
    }
}
