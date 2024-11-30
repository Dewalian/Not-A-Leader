using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HalfSuccubus : Enemy
{
    private List<Enemy> charmedUnits;
    [SerializeField] private GameObject charmedUnit;
    [SerializeField] private float charmChance;
    [SerializeField] private float maxCharmed;
    [SerializeField] private Color charmedColor;
    [SerializeField] private float heroStunDuration;

    public override void TakeDamage(float attackDamagePhysic, float attackDamageMagic)
    {
        base.TakeDamage(attackDamagePhysic, attackDamageMagic);

        if(unitState == State.Death || unitsToFight.Count <= 0 || charmedUnits.Count >= maxCharmed){
            return;
        }

        if(Random.value <= charmChance / 100){
            animator.SetTrigger("TriggerKiss");
        }
    }

    public void CharmAnimator()
    {
        if(unitTarget.CompareTag("Player")){
            StartCoroutine(unitTarget.ChangeStatsTimed(-100, heroStunDuration));
            StartCoroutine(unitTarget.ChangeColor(heroStunDuration, charmedColor));
            return;
        }

        GameObject charmedObj = Instantiate(charmedUnit, unitTarget.transform.position, Quaternion.identity);

        unitTarget.GetComponent<Ally>().DeathAnimator();

        charmedObj.GetComponent<SpriteRenderer>().sprite = unitTarget.GetComponent<SpriteRenderer>().sprite;

        charmedObj.GetComponent<Enemy>().InitializeSummoned(wayPointIndex, enemySpawner);

        charmedObj.GetComponent<Enemy>().Upgrade(unitTarget.moveSpeed, unitTarget.health, unitTarget.attackRange,
        unitTarget.attackDamagePhysic, unitTarget.attackDamageMagic, unitTarget.attackCD, unitTarget.physicRes,
        unitTarget.magicRes, unitTarget.healthRegen);

        charmedObj.transform.SetParent(enemySpawner.transform);
    }
}
