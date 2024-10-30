using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    [SerializeField] private LayerMask playerLayer;
    private float moveSpeedCopy;
    private Transform wayPoint;
    private int wayPointIndex = 0;
    private List<GameObject> unitsToFight = new List<GameObject>();
    public State enemyState;

    protected override void Start()
    {
        moveSpeedCopy = moveSpeed;
    }

    private void Update()
    {
        if(enemyState == State.Neutral){
            moveSpeed = moveSpeedCopy;
        }else if(enemyState == State.Fighting){
            moveSpeed = 0f;
            if(unitsToFight.Count > 0 && Vector2.Distance(transform.position, unitsToFight[0].transform.position) <= attackRange){
                StartCoroutine(AttackUnit(unitsToFight[0]));
            }
        }

        if(unitsToFight.Count == 0){
            enemyState = State.Neutral;
        }else{
            enemyState = State.Fighting;
        }

        wayPoint = LevelManager.instance.wayPoint[wayPointIndex];
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
            if(wayPointIndex == LevelManager.instance.wayPoint.Length){
                Destroy(gameObject);
            }else{
                wayPoint = LevelManager.instance.wayPoint[wayPointIndex];
            }
        }
    }

    private void MoveToWayPoint()
    {
        transform.position = Vector2.MoveTowards(transform.position, wayPoint.position, moveSpeed * Time.deltaTime);
    }

    public void AddUnitToFightArr(GameObject unitToFight)
    {
        unitsToFight.Add(unitToFight);
    }

    public void RemoveUnitFromFightArr(GameObject unitToFight)
    {
        unitsToFight.Remove(unitToFight);
    }

    public Vector2 GetBulletPos(float bulletDuration)
    {
        float bulletCalc = bulletDuration * moveSpeed;

        Vector2 enemyDir = (wayPoint.position - transform.position).normalized;
        Vector2 nextWaypointDir = (LevelManager.instance.wayPoint[wayPointIndex + 1].position - LevelManager.instance.wayPoint[wayPointIndex].position).normalized;

        if(Vector2.Distance(transform.position, wayPoint.position) <= bulletCalc){
            float remainingDistance = bulletCalc - Vector2.Distance(transform.position, wayPoint.position);
            return (nextWaypointDir * remainingDistance) + (enemyDir * (bulletCalc - remainingDistance));
        }
        return enemyDir * bulletCalc;
    }
}
