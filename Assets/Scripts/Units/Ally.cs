using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ally : Unit
{
    [SerializeField] private NavMeshAgent agent;
    private AllyArea allyArea;
    private GameObject enemy;
    private float healthCopy;
    public Transform originalPos;
    public State allyState;
    public bool isDuel = false;

    protected virtual void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = moveSpeed;

        agent.radius = 0.1f;
        agent.acceleration = 50f;
        
        MoveToOriginalPos();
        healthCopy = health;

        allyArea = GetComponentInParent<AllyArea>();
        allyArea.OnMoveArea.AddListener(() => RemoveFromFight());
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
        RemoveFromFight();
        health = healthCopy;
        allyArea.StartRespawn(gameObject);
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

    public void TeleportToOriginalPos()
    {
        transform.position = originalPos.transform.position;
    }
}
