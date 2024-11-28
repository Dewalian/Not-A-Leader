using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MortarBullet : Bullet
{
    [SerializeField] private LayerMask enemylayer;
    private float innerRange;
    private float outerRange;

    protected override void DamageEnemy()
    {
        Collider2D[] outerCircle = Physics2D.OverlapCircleAll(transform.position, outerRange, enemylayer);
        Collider2D[] innerCircle = Physics2D.OverlapCircleAll(transform.position, innerRange, enemylayer);

        foreach(Collider2D e in outerCircle){
            foreach(Collider2D e2 in innerCircle){
                if(e == e2){
                    e2.GetComponent<Unit>().TakeDamage(damagePhysic, damageMagic);
                    break;
                }
            }
            e.GetComponent<Unit>().TakeDamage(damagePhysic/4, damageMagic/4);
        }
    }

    public void InitVariablesMortar(Transform target, Transform startPosBullet, Transform startPosBulletShadow, 
    float damagePhysic, float damageMagic , float duration, float innerRange, float outerRange)
    {
        InitVariables(target, startPosBullet, startPosBulletShadow, damagePhysic, damageMagic, duration);
        this.innerRange = innerRange;
        this.outerRange = outerRange;
    }
}
