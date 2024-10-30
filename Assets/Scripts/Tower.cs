using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private Transform shadowPoint;
    [SerializeField] private GameObject bullet;
    [SerializeField]private float attackRange;
    [SerializeField] private float shootCD;
    [SerializeField] private float bulletDuration;
    [SerializeField] private float bulletDamage;


    private bool canShoot = true;
    private Transform target;

    void Update()
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
            bulletObj.GetComponent<Bullet>().InitVariables(target, firingPoint, shadowPoint, bulletDamage, bulletDuration);
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

    void DetectEnemies()
    {
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);
        if(enemiesInRange.Length > 0){
            target = enemiesInRange[0].transform;
        }
    }
}
