using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet
{
    protected override void Start()
    {
        endPos = (Vector2)target.position;
    }
}