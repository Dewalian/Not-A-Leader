using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarBullet : Bullet
{
    [SerializeField] private LayerMask enemylayer;
    [SerializeField] private GameObject blast;
    private float innerRange;
    private float outerRange;

    protected override void DamageEnemy()
    {
        Collider2D[] outerCircle = Physics2D.OverlapCircleAll(transform.position, outerRange, enemylayer);
        Collider2D[] innerCircle = Physics2D.OverlapCircleAll(transform.position, innerRange, enemylayer);

        GameObject blastObj = Instantiate(blast, endPos, Quaternion.identity);
        ParticleSystem blastParticle = blastObj.GetComponent<ParticleSystem>();

        var blastMain = blastParticle.main;

        blastMain.startLifetime = outerRange / 10;
        blastMain.startSpeed = outerRange * 12;
        blastParticle.Emit((int) outerRange * 12);

        Destroy(blastObj, blastMain.duration);

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
