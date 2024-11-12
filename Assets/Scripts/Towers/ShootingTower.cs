using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingTower : Tower
{
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private Transform shadowPoint;
    [SerializeField] private GameObject bullet;

    [Serializable]
    private struct Stats{
        public float attackRange;
        public float shootCD;
        public float bulletDamagePhysic;
        public float bulletDamageMagic;
    }
    [SerializeField] private Stats[] stats;
    [SerializeField] private float attackRange;
    [SerializeField] private float shootCD;
    [SerializeField] private float bulletDamagePhysic;
    [SerializeField] private float bulletDamageMagic;
    [SerializeField] private float bulletDuration;
    private bool canShoot = true;
    private Transform target;

    private void Start()
    {
        UpgradeTower();
    }

    private void Update()
    {
        if(target == null){
            DetectEnemies();
        }else if(IsEnemyInRange() && canShoot){
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {
        canShoot = false;
        if(target != null){
            GameObject bulletObj = Instantiate(bullet, transform.position, Quaternion.identity);
            bulletObj.transform.SetParent(gameObject.transform);
            bulletObj.GetComponent<Bullet>().InitVariables(target, firingPoint, shadowPoint, 
            bulletDamagePhysic, bulletDamageMagic, bulletDuration);
            yield return new WaitForSeconds(shootCD);
            canShoot = true;
        }
    }

    private bool IsEnemyInRange()
    {
        if(target != null){
            return Vector2.Distance(transform.position, target.position) < attackRange;
        }
        return false;
    }

    private void DetectEnemies()
    {
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);
        if(enemiesInRange.Length > 0){
            target = enemiesInRange[0].transform;
        }
    }

    public override void UpgradeTower()
    {
        base.UpgradeTower();
        attackRange = stats[level].attackRange;
        shootCD = stats[level].shootCD;
        bulletDamagePhysic = stats[level].bulletDamagePhysic;
        bulletDamageMagic = stats[level].bulletDamageMagic;
    }

    public void ChangeShootCD(float changeValue)
    {
        shootCD += changeValue;
    }
}
