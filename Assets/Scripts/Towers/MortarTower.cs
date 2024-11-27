using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarTower : ShootingTower
{
    [Serializable]
    private struct MortarTowerStats{
        public float innerRange;
        public float outerRange;
        public float bulletSize;
    }
    [SerializeField] private MortarTowerStats[] mortarTowerStats = new MortarTowerStats[3];
    private float innerRange;
    private float outerRange;
    private float bulletSize;

    protected override void SpawnBullet()
    {
        GameObject bulletObj = Instantiate(bullet, transform.position, Quaternion.identity);
        bulletObj.transform.SetParent(gameObject.transform);
        bulletObj.GetComponent<MortarBullet>().InitVariablesMortar(target, firingPoint, shadowPoint, 
        bulletDamagePhysic, bulletDamageMagic, bulletDuration, innerRange, outerRange);
        bulletObj.transform.localScale = new Vector2(bulletSize, bulletSize);
    }

    public override void UpgradeTower()
    {
        if(level >= 2 || (level < 2 && LevelManager.Instance.gold < costs[level+1])){
            return;
        }

        base.UpgradeTower();
        innerRange = mortarTowerStats[level].innerRange;
        outerRange = mortarTowerStats[level].outerRange;
        bulletSize = mortarTowerStats[level].bulletSize;
    }
}
