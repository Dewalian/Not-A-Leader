using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joker : Ally
{
    [Serializable]
    private struct HealthRegenAccel{
        public float regenAccelCount;
        public float regenAccelValue;
    }
    [SerializeField] private List<HealthRegenAccel> healthRegenAccels;

    protected override void Start()
    {
        base.Start();
        LevelManager.Instance.UpdateHeroHealth(health);
    }

    protected override void StateChange()
    {
        if(unitState == State.Skill){
            animator.SetBool("BoolWalk", false);
            moveSpeed = 0;
            return;
        }
        base.StateChange();
    }

    protected override IEnumerator HealthRegen()
    {
        isStartRegen = true;
        yield return new WaitForSeconds(healthRegenDelay);

        float seconds = 0;
        int index = -1;
        while(health < healthCopy){

            if(index == -1){
                health += Mathf.Min(healthRegen, healthCopy - health);
            }else{
                health += Mathf.Min(healthRegenAccels[index].regenAccelValue, healthCopy - health);
            }
            OnHealthChanged?.Invoke();
            LevelManager.Instance.UpdateHeroHealth(health);

            yield return new WaitForSeconds(1f);
            seconds++;

            if(index+1 < healthRegenAccels.Count && seconds == healthRegenAccels[index+1].regenAccelCount){
                index++;
            }
        }
    }

    public override void TakeDamage(float attackDamagePhysic, float attackDamageMagic)
    {
        base.TakeDamage(attackDamagePhysic, attackDamageMagic);
        LevelManager.Instance.UpdateHeroHealth(health);
    }
}
