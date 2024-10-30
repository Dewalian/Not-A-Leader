using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : Unit
{
    [SerializeField] private Transform originalPos;
    private GameObject enemy;
    private float healthCopy;
    public State allyState;
    public bool isAlive = true;
    public bool isDuel = false;

    protected override void Start()
    {
        base.Start();
        MoveToOriginalPos();
        healthCopy = health;
    }

    private void Update()
    {
        if(allyState == State.Aggro){
            agent.isStopped = false;
            AggroEnemy();
        }else if(allyState == State.Fighting){
            StartCoroutine(AttackUnit(enemy));
            agent.isStopped = true;
        }else if(allyState == State.Neutral){
            agent.isStopped = false;
            MoveToOriginalPos();
        }

        if(enemy == null){
            allyState = State.Neutral;
        }
    }

    private void AggroEnemy()
    {
        if(enemy != null){
            if(Vector2.Distance(transform.position, enemy.transform.position) >= attackRange){
                agent.SetDestination(enemy.transform.position);
            }else{
                allyState = State.Fighting;
            }
        }else{
            allyState = State.Neutral;
        }
    }

    private void MoveToOriginalPos()
    {
        agent.SetDestination(originalPos.position);
    }

    protected override void Death()
    {
        isAlive = false;
        RemoveFromFight();
        health = healthCopy;
        gameObject.SetActive(false);
    }

    public void SetTarget(GameObject enemy)
    {
        RemoveFromFight();
        this.enemy = enemy;
        enemy.GetComponent<Enemy>().AddUnitToFightArr(gameObject);
        allyState = State.Aggro;
    }

    public void RemoveFromFight()
    {
        if(enemy != null){
            enemy.GetComponent<Enemy>().RemoveUnitFromFightArr(gameObject);
            allyState = State.Neutral;
        }
    }
}
