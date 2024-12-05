using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : Enemy
{
    [SerializeField] private float rangedRange;
    [SerializeField] private float rangedDamagePhysic;
    [SerializeField] private float rangedDamageMagic;
    [SerializeField] private float rangedCD;
    [SerializeField] private GameObject bullet;
    [SerializeField] private LayerMask allyLayer;
    private bool canShoot = true;
    private GameObject target;
    protected override void StateChange()
    {
        base.StateChange();
        if(unitState == State.Neutral){
            DetectTargetRanged();
        }else if(unitState == State.Shooting){
            moveSpeed = 0;
        }
    }

    private void DetectTargetRanged()
    {
        Collider2D target = Physics2D.OverlapCircle(transform.position, rangedRange, allyLayer);
        if(target != null && target.gameObject.activeSelf){
            unitState = State.Shooting;
            FlipDirection(target.transform.position);
            if(canShoot){
                this.target = target.gameObject;
                RangeAttackTargetAnimator();
            }
        }
        else
        {
            unitState = State.Neutral;
        }
    }

    private void RangeAttackTargetAnimator()
    {
        animator.SetTrigger("TriggerAttackRanged");
    }

    private IEnumerator RangeAttackCD()
    {
        yield return new WaitForSeconds(rangedCD);
        canShoot = true;
    }

    private void RangeAttackTarget()
    {
        canShoot = false;
        if(target != null){
            GameObject bulletObj = Instantiate(bullet, transform.position, Quaternion.identity);
            bulletObj.transform.SetParent(gameObject.transform);
            bulletObj.GetComponent<Bullet>().InitVariables(target.transform, transform, transform, 
            rangedDamagePhysic, rangedDamageMagic, 1);
            StartCoroutine(RangeAttackCD());
        }
    }
}
