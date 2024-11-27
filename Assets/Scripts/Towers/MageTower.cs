using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MageTower : ShootingTower
{
    [Serializable]
    private struct MageTowerStats{
        public int maxCharge;
    }
    [SerializeField] private MageTowerStats[] mageTowerStats = new MageTowerStats[3];
    [SerializeField] private float chargeBuffPercentage;
    private int maxCharge;
    private int chargeCount;
    private float totalChargeBuff;
    private Transform lastTarget;
    public UnityEvent OnSameTarget;
    public UnityEvent OnDifferentTarget;

    protected override void SpawnBullet()
    {
        if(lastTarget != null && target == lastTarget && chargeCount < maxCharge){
            ChangeStats(chargeBuffPercentage);
            chargeCount++;
            totalChargeBuff += chargeBuffPercentage;
            OnSameTarget?.Invoke();
        }else if(lastTarget == null || target != lastTarget){
            ChangeStats(-totalChargeBuff);
            chargeCount = 0;
            totalChargeBuff = 0;
            lastTarget = target;
            OnDifferentTarget?.Invoke();
        }

        base.SpawnBullet();
    }

    public override void UpgradeTower()
    {
        base.UpgradeTower();
        maxCharge = mageTowerStats[level].maxCharge;
    }

}
