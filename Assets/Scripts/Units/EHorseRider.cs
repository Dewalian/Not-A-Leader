using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EHorseRider : Enemy
{
    [SerializeField] private GameObject lancer;

    protected override void Start()
    {
        base.Start();
        enemyState = State.Fighting;
    }

    protected override void StateChange()
    {
        
    }

    protected override void Death()
    {
        GameObject lancerObj = Instantiate(lancer, transform.position, Quaternion.identity);
        lancerObj.GetComponent<Enemy>().InitializeSummoned(wayPointIndex, enemySpawner);
        lancerObj.transform.SetParent(enemySpawner.transform);
        base.Death();
    }
}
