using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherTower : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Transform head;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private GameObject bullet;

    [SerializeField ]private float attackRange = 5f;
    [SerializeField] private float shootCD = 0.5f;
    private bool canShoot = true;
    private Transform target;

    void Update()
    {
        if(target == null){
            DetectEnemies();
        }else{
            HeadFollowEnemy();
            if(!CheckIfEnemyIsInRange()){
                target = null;
            }
            if(canShoot){
                StartCoroutine(Shoot());
            }
        }
    }

    IEnumerator Shoot(){
        canShoot = false;
        yield return new WaitForSeconds(shootCD);
        GameObject bulletObj = Instantiate(bullet, firingPoint.position, Quaternion.identity);
        bulletObj.GetComponent<Bullet>().SetTarget(target);
        canShoot = true;
    }

    void DetectEnemies(){
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);
        if(enemiesInRange.Length > 0){
            target = enemiesInRange[0].transform;
        }
    }

    void HeadFollowEnemy(){
        Vector2 direction = (target.position - transform.position).normalized;
        head.transform.right = direction * -1;
    }

    bool CheckIfEnemyIsInRange(){
        return Vector2.Distance(transform.position, target.position) <= attackRange;
    }
}
