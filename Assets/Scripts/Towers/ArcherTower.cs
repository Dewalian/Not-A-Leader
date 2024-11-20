using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ArcherTower : ShootingTower
{
    [Serializable]
    private struct ArcherTowerStats{
        public float maxMultiplier;
        public float multiplierChance;
    }
    [SerializeField] private ArcherTowerStats[] archerTowerStats = new ArcherTowerStats[3];
    [SerializeField] private float maxMultiplier;
    [SerializeField] private float multiplierChance;
    [SerializeField] private float multiplierDelay;
    private bool canMultiply = true;
    private int multiplyCount = 0;

    protected override void SpawnBullet()
    {
        base.SpawnBullet();
        multiplyCount = 0;
        StartCoroutine(Multiplier());
    }

    private IEnumerator Multiplier()
    {
        if(canMultiply && multiplyCount < maxMultiplier && Random.value <= multiplierChance / 100){
            canMultiply = false;
            yield return new WaitForSeconds(multiplierDelay);
            base.SpawnBullet();
            Debug.Log("Test");
            canMultiply = true;
            multiplyCount++;
            StartCoroutine(Multiplier());
        }
    }

    public override void UpgradeTower()
    {
        base.UpgradeTower();
        maxMultiplier = archerTowerStats[level].maxMultiplier;
        multiplierChance = archerTowerStats[level].multiplierChance;
    }
}
