using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingTower : Tower
{
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] protected Transform firingPoint;
    [SerializeField] protected Transform shadowPoint;
    [SerializeField] protected GameObject bullet;

    [Serializable]
    private struct Stats{
        public float attackRange;
        public float shootCD;
        public float bulletDamagePhysic;
        public float bulletDamageMagic;
    }
    [SerializeField] private Stats[] stats = new Stats[3];
    [SerializeField] protected float attackRange;
    [SerializeField] protected float shootCD;
    [SerializeField] protected float bulletDamagePhysic;
    [SerializeField] protected float bulletDamageMagic;
    [SerializeField] protected float bulletDuration;
    protected Transform target;
    private bool canShoot = true;

    private void Update()
    {
        if(target == null || !IsEnemyInRange() || target.GetComponent<Unit>().health <= 0){
            DetectEnemies();
        }
        
        if(IsEnemyInRange() && canShoot){
            StartCoroutine(Shoot());
        }
    }

    private IEnumerator Shoot()
    {
        canShoot = false;
        if(target != null){
            SpawnBullet();
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

        foreach(Collider2D e in enemiesInRange){
            if(e.GetComponent<Enemy>().health > 0){
                target = e.transform;
            }
        }
    }

    protected virtual void SpawnBullet()
    {
        GameObject bulletObj = Instantiate(bullet, transform.position, Quaternion.identity);
        bulletObj.transform.SetParent(gameObject.transform);
        bulletObj.GetComponent<Bullet>().InitVariables(target, firingPoint, shadowPoint, 
        bulletDamagePhysic, bulletDamageMagic, bulletDuration);
    }

    public override void UpgradeTower()
    {
        base.UpgradeTower();
        attackRange = stats[level].attackRange;
        shootCD = stats[level].shootCD;
        bulletDamagePhysic = stats[level].bulletDamagePhysic;
        bulletDamageMagic = stats[level].bulletDamageMagic;

        OnUpgrade?.Invoke();
    }

    public override void ChangeStats(float changePercentage)
    {
        changePercentage /= 100;

        attackRange += stats[level].attackRange * changePercentage;
        shootCD -= stats[level].shootCD * changePercentage;
        bulletDamagePhysic += stats[level].bulletDamagePhysic * changePercentage;
        bulletDamageMagic += stats[level].bulletDamageMagic * changePercentage;
    }

    public override float GetRange()
    {
        return attackRange;
    }
}
