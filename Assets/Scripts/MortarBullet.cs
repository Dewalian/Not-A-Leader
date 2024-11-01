using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarBullet : Bullet
{
    [SerializeField] private LayerMask enemylayer;
    [SerializeField] private float innerRange;
    [SerializeField] private float outerRange;

    protected override void DamageEnemy()
    {
        Collider2D[] innerCircle = Physics2D.OverlapCircleAll(transform.position, innerRange, enemylayer);
        Collider2D[] outerCircle = Physics2D.OverlapCircleAll(transform.position, outerRange, enemylayer);

        foreach(Collider2D e in outerCircle){
            foreach(Collider2D e2 in innerCircle){
                e2.GetComponent<Enemy>().TakeDamage(damage);
                return;
            }
            e.GetComponent<Enemy>().TakeDamage(damage/2);
        }
    }
}
