using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfSuccubus : Enemy
{
    private GameObject charmedUnit;
    [SerializeField] private float charmChance;

    protected override void AttackEffect()
    {   
        charmedUnit = unitsToFight[0];
        if(charmedUnit.CompareTag("Player")){
            return;
        }

        if(Random.value <= charmChance / 100){
            GameObject charmedObj = Instantiate(charmedUnit, charmedUnit.transform.position, Quaternion.identity);
            unitsToFight[0].GetComponent<Ally>().Death();

            charmedObj.GetComponent<Ally>().SwitchSide();
            charmedObj.GetComponent<Enemy>().InitializeSummoned(wayPointIndex, enemySpawner);
            charmedObj.GetComponent<Enemy>().FlipDirection(wayPoint.position);
            charmedObj.transform.SetParent(enemySpawner.transform);
        }
    }
}
