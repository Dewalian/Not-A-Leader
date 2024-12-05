using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet
{
    protected override void SetEndPos()
    {
        endPos = (Vector2)target.position;
    }
}
