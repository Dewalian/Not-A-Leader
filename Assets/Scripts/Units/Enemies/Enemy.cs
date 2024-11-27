using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : Unit
{
    [SerializeField] private float dodgeChance;
    [SerializeField] private int bounty;
    [SerializeField] private int lifeDamage;
    protected Transform wayPoint;
    protected List<GameObject> unitsToFight = new List<GameObject>();
    protected EnemySpawner enemySpawner;
    protected int wayPointIndex = 0;

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
    }

    private void ChangeTargetPos()
    {
        if(Vector2.Distance(wayPoint.position, transform.position) <= 0.1f){
            wayPointIndex++;
            if(wayPointIndex == enemySpawner.wayPoint.Length){
                LevelManager.Instance.life -= lifeDamage;
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

    protected override void StateChange()
    {
        base.StateChange();

        if(unitState == State.Neutral){
            moveSpeed = moveSpeedCopy;

            animator.SetBool("BoolWalk", true);

        }else if(unitState == State.Fighting){
            moveSpeed = 0;

            animator.SetBool("BoolWalk", false);
            
            if(unitsToFight != null && 
            unitsToFight.Count > 0 && Vector2.Distance(transform.position, unitsToFight[0].transform.position) <= attackRange){
                StartCoroutine(AttackUnit(unitsToFight[0]));
            }
        }

        if(unitsToFight.Count == 0 && unitState != State.Shooting){
            unitState = State.Neutral;
        }else{
            unitState = State.Fighting;
        }
    }

    public override void Death()
    {
        LevelManager.Instance.AddGold(bounty);
        base.Death();
    }

    public override void TakeDamage(float attackDamagePhysic, float attackDamageMagic)
    {
        if(Random.value >= dodgeChance / 100){
            base.TakeDamage(attackDamagePhysic, attackDamageMagic);
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
        FlipDirection(wayPoint.position);
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
