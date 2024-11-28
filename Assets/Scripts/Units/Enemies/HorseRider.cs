using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseRider : SwiftEnemy
{
    [SerializeField] private GameObject lancer;

    public override void DeathAnimator()
    {
        GameObject lancerObj = Instantiate(lancer, transform.position, Quaternion.identity);
        lancerObj.GetComponent<Enemy>().InitializeSummoned(wayPointIndex, enemySpawner);
        lancerObj.transform.SetParent(enemySpawner.transform);
        base.DeathAnimator();
    }
}
