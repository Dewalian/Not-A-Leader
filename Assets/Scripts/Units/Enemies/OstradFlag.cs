using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OstradFlag : Bullet
{
    protected override void SetEndPos()
    {
        endPos = target.position;
    }

    protected override IEnumerator Lob()
    {
        float timePassed = 0;

        while(timePassed < duration)
        {
            timePassed += Time.deltaTime;
            
            float durationNorm = timePassed / duration;
            float height = Mathf.Lerp(0, maxHeight, trajectoryCurve.Evaluate(durationNorm));

            Vector2 bulletPos = Vector2.Lerp(startPosBullet.position, endPos, durationNorm) + new Vector2(0, height);
            Vector2 bulletShadowPos = Vector2.Lerp(startPosBulletShadow.position, endPos, durationNorm);

            bullet.right = (bulletPos - (Vector2)bullet.position).normalized;
            bulletShadow.right = (bulletShadowPos - (Vector2)bulletShadow.position).normalized;

            bullet.position = bulletPos;
            bulletShadow.position = bulletShadowPos;

            yield return null;
        }
    }
}
