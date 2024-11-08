using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ERanged : Enemy
{
    [SerializeField] private float rangedRange;
    [SerializeField] private float rangedDamagePhysic;
    [SerializeField] private float rangedDamageMagic;
    [SerializeField] private float rangedCD;
    [SerializeField] private GameObject bullet;
    [SerializeField] private LayerMask allyLayer;
    private bool canShoot = true;
    protected override void StateChange()
    {
        base.StateChange();
        if(enemyState == State.Neutral){
            DetectTargetRanged();
        }else if(enemyState == State.Shooting){
            moveSpeed = 0;
        }
    }

    private void DetectTargetRanged()
    {
        Collider2D target = Physics2D.OverlapCircle(transform.position, rangedRange, allyLayer);
        if(target != null){
            enemyState = State.Shooting;
            if(canShoot){
                StartCoroutine(RangeAttackTarget(target.gameObject));
            }
        }
    }

    private IEnumerator RangeAttackTarget(GameObject target)
    {
        canShoot = false;
        if(target != null){
            GameObject bulletObj = Instantiate(bullet, transform.position, Quaternion.identity);
            bulletObj.transform.SetParent(gameObject.transform);
            bulletObj.GetComponent<Bullet>().InitVariables(target.transform, transform, transform, rangedDamagePhysic, rangedDamageMagic, 1);
            yield return new WaitForSeconds(rangedCD);
            canShoot = true;
        }
    }
}
