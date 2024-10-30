using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    [SerializeField] private AnimationCurve trajectoryCurve;
    [SerializeField] private float maxHeight;
    [SerializeField] private Transform bullet;
    [SerializeField] private Transform bulletShadow;
    private Transform startPosBullet;
    private Transform startPosBulletShadow;
    private Transform target;
    private Vector2 endPos;
    private float duration;
    protected float damage;

    private void Start()
    {
        endPos = (Vector2)target.position + target.GetComponent<Enemy>().GetBulletPos(duration);
    }

    private void Update()
    {
        StartCoroutine(Lob());
    }

    private IEnumerator Lob(){
        float timePassed = 0;

        while(timePassed < duration)
        {
            timePassed += Time.deltaTime;
            
            float durationNorm = timePassed / duration;
            float height = Mathf.Lerp(0, maxHeight, trajectoryCurve.Evaluate(durationNorm));

            bullet.position = Vector2.Lerp(startPosBullet.position, endPos, durationNorm) + new Vector2(0, height);
            bulletShadow.position = Vector2.Lerp(startPosBulletShadow.position, endPos, durationNorm);

            yield return null;
        }

        DamageEnemy();
        Destroy(gameObject);
    }

    protected virtual void DamageEnemy()
    {
        target.GetComponent<Enemy>().TakeDamage(damage);
    }

    public void InitVariables(Transform target, Transform startPosBullet, Transform startPosBulletShadow, float damage, float duration)
    {
        this.target = target;
        this.startPosBullet = startPosBullet;
        this.startPosBulletShadow = startPosBulletShadow;
        this.damage = damage;
        this.duration = duration;
    }
}
