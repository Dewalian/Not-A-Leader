using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] protected AnimationCurve trajectoryCurve;
    [SerializeField] protected float maxHeight;
    [SerializeField] protected Transform bullet;
    [SerializeField] protected Transform bulletShadow;
    protected Transform startPosBullet;
    protected Transform startPosBulletShadow;
    protected float duration;
    protected Collider2D bulletCol;
    protected Transform target;
    protected Vector2 endPos;
    protected float damagePhysic;
    protected float damageMagic;

    protected virtual void Start()
    {
        SetEndPos();
        StartCoroutine(Lob());
    }

    protected virtual void SetEndPos()
    {
        if(target != null){
            endPos = (Vector2)target.position + target.GetComponent<Enemy>().GetBulletPos(duration);
        }
    }

    protected virtual IEnumerator Lob(){
        float timePassed = 0;

        while(timePassed < duration)
        {
            timePassed += Time.deltaTime;
            
            float durationNorm = timePassed / duration;
            float height = Mathf.Lerp(0, maxHeight, trajectoryCurve.Evaluate(durationNorm));

            Vector2 bulletPos = Vector2.Lerp(startPosBullet.position, endPos, durationNorm) + new Vector2(0, height);
            Vector2 bulletShadowPos = Vector2.Lerp(startPosBulletShadow.position, endPos, durationNorm);

            bullet.up = (bulletPos - (Vector2)bullet.position).normalized;
            bulletShadow.up = (bulletShadowPos - (Vector2)bulletShadow.position).normalized;

            bullet.position = bulletPos;
            bulletShadow.position = bulletShadowPos;

            yield return null;
        }

        DamageEnemy();
        yield return null;
        Destroy(gameObject);
    }

    protected virtual void DamageEnemy()
    {
        if(target != null && Vector2.Distance(target.position, bullet.position) <= 0.5f){
            target.GetComponent<Unit>().TakeDamage(damagePhysic, damageMagic);
        }
    }

    public virtual void InitVariables(Transform target, Transform startPosBullet, Transform startPosBulletShadow, 
    float damagePhysic, float damageMagic , float duration)
    {
        this.target = target;
        this.startPosBullet = startPosBullet;
        this.startPosBulletShadow = startPosBulletShadow;
        this.damagePhysic = damagePhysic;
        this.damageMagic = damageMagic;
        this.duration = duration;
    }
}
