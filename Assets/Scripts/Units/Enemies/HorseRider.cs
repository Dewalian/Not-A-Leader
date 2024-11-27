using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseRider : Enemy
{
    [SerializeField] private GameObject lancer;

    protected override void Start()
    {
        base.Start();
        unitState = State.Fighting;
    }

    protected override void StateChange()
    {
        if(health <= 0){
            unitState = State.Death;
        }

        if(unitState == State.Death){
            Death();
            return;
        }
        
        animator.SetBool("BoolWalk", true);
    }

    public override void DeathAnimator()
    {
        GameObject lancerObj = Instantiate(lancer, transform.position, Quaternion.identity);
        lancerObj.GetComponent<Enemy>().InitializeSummoned(wayPointIndex, enemySpawner);
        lancerObj.transform.SetParent(enemySpawner.transform);
        base.DeathAnimator();
    }
}
