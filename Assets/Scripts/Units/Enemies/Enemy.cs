using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    protected Transform wayPoint;
    protected List<GameObject> unitsToFight = new List<GameObject>();
    protected EnemySpawner enemySpawner;
    protected int wayPointIndex = 0;
    public State enemyState;

    protected override void Start()
    {
        enemySpawner = GetComponentInParent<EnemySpawner>();
        moveSpeedCopy = moveSpeed;
    }

    protected virtual void Update()
    {
        StateChange();
        MoveToWayPoint();
        ChangeTargetPos();

        if(health <= 0){
            Destroy(gameObject);
        }
    }

    private void ChangeTargetPos()
    {
        if(Vector2.Distance(wayPoint.position, transform.position) <= 0.1f){
            wayPointIndex++;
            if(wayPointIndex == enemySpawner.wayPoint.Length){
                Destroy(gameObject);
            }else{
                wayPoint = enemySpawner.wayPoint[wayPointIndex];
                FlipDirection(wayPoint.position);
            }
        }
    }

    private void MoveToWayPoint()
    {
        wayPoint = enemySpawner.wayPoint[wayPointIndex];
        transform.position = Vector2.MoveTowards(transform.position, wayPoint.position, moveSpeed * Time.deltaTime);
    }

    protected virtual void StateChange()
    {
        if(enemyState == State.Neutral){
            moveSpeed = moveSpeedCopy;
        }else if(enemyState == State.Fighting){
            moveSpeed = 0;
            if(unitsToFight != null && 
            unitsToFight.Count > 0 && Vector2.Distance(transform.position, unitsToFight[0].transform.position) <= attackRange){
                StartCoroutine(AttackUnit(unitsToFight[0]));
            }
        }

        if(unitsToFight.Count == 0 && enemyState != State.Shooting){
            enemyState = State.Neutral;
        }else{
            enemyState = State.Fighting;
        }
    }

    public void AddUnitToFightArr(GameObject unitToFight)
    {
        unitsToFight.Add(unitToFight);
        FlipDirection(unitToFight.transform.position);
    }

    public void RemoveUnitFromFightArr(GameObject unitToFight)
    {
        unitsToFight.Remove(unitToFight);
    }

    public Vector2 GetBulletPos(float bulletDuration)
    {
        float bulletCalc = bulletDuration * moveSpeed;

        Vector2 enemyDir = (wayPoint.position - transform.position).normalized;

        if(Vector2.Distance(transform.position, wayPoint.position) <= bulletCalc &&
        wayPointIndex != enemySpawner.wayPoint.Length - 1){
            Vector2 nextWaypointDir = (enemySpawner.wayPoint[wayPointIndex + 1].position - enemySpawner.wayPoint[wayPointIndex].position).normalized;
            float remainingDistance = bulletCalc - Vector2.Distance(transform.position, wayPoint.position);
            return (nextWaypointDir * remainingDistance) + (enemyDir * (bulletCalc - remainingDistance));
        }

        return enemyDir * bulletCalc;
    }

    public void InitializeSummoned(int wayPointIndex, EnemySpawner enemySpawner)
    {
        this.wayPointIndex = wayPointIndex;
        this.enemySpawner = enemySpawner;
    }

    
}
