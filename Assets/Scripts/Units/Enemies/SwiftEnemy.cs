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
        if(unitState == State.Death || unitState == State.Skill){
            return;
        }
        
        animator.SetBool("BoolWalk", true);
    }
}
