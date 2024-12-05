using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HalfSuccubus : Enemy
{
    private List<Unit> charmedUnits;
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

    public override void Death()
    {
        foreach(Unit c in charmedUnits){
            c.Death();
        }
        base.Death();
    }

    public void CharmAnimator()
    {
        if(unitTarget.CompareTag("Player")){
            StartCoroutine(unitTarget.ChangeStatsTimed(-100, heroStunDuration));
            StartCoroutine(unitTarget.ChangeColor(heroStunDuration, charmedColor));
            return;
        }

        GameObject charmedObj = Instantiate(charmedUnit, unitTarget.transform.position, Quaternion.identity);

        unitTarget.GetComponent<Ally>().RemoveFromFight();
        unitTarget.GetComponent<Ally>().DeathAnimator();

        Enemy charmedEnemy = charmedObj.GetComponent<Enemy>();

        charmedObj.GetComponent<SpriteRenderer>().sprite = unitTarget.GetComponent<SpriteRenderer>().sprite;

        charmedEnemy.InitializeSummoned(wayPointIndex, enemySpawner);

        charmedEnemy.Upgrade(unitTarget.moveSpeed, unitTarget.health, unitTarget.attackRange,
        unitTarget.attackDamagePhysic, unitTarget.attackDamageMagic, unitTarget.attackCD, unitTarget.physicRes,
        unitTarget.magicRes, unitTarget.healthRegen);

        charmedUnits.Add(charmedEnemy);
        charmedEnemy.OnDeath.AddListener((Unit enemy) => RemoveCharmedEnemy(enemy));

        charmedObj.transform.SetParent(enemySpawner.transform);
    }

    private void RemoveCharmedEnemy(Unit enemy)
    {
        charmedUnits.Remove(enemy);
    }
}
