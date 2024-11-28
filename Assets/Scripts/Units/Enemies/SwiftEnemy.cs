using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwiftEnemy : Enemy
{
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
}
