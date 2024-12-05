using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butcher : Enemy
{
    [SerializeField] private float healthPercentage;
    protected override void FatalEffect()
    {
        health = healthCopy * healthPercentage / 100;
    }

    protected override IEnumerator AttackUnit(GameObject unit)
    {
        unitTarget = unit.GetComponent<Unit>();

        if(canAttack && unitTarget != null){
            isAttackAnimation = true;
            canAttack = false;

            if(unitTarget.AboutToDie(attackDamagePhysic, attackDamageMagic)){
                animator.SetTrigger("TriggerEat");
                unit.GetComponent<Ally>().RemoveFromFight();
                unit.GetComponent<Ally>().DeathAnimator();
            }else{
                animator.SetTrigger("TriggerAttack");
            }
            yield return new WaitForSeconds(attackCD);
            canAttack = true;
        }
    }
}
