using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butcher : Enemy
{
    private float healthCopy;
    protected override void Start()
    {
        base.Start();
        healthCopy = health;
    }
    protected override void CriticalEffect()
    {
        health = healthCopy;
    }
}
