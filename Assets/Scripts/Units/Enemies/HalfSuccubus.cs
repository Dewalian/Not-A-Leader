using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfSuccubus : Enemy
{
    private GameObject charmedUnit;
    [SerializeField] private float charmChance;

    protected override void AttackEffect()
    {
        if(Random.value <= charmChance / 100){
            charmedUnit = unitsToFight[0];
            GameObject charmedObj = Instantiate(charmedUnit, transform.position, Quaternion.identity);
            unitsToFight[0].GetComponent<Unit>().health = 0;

            charmedObj.GetComponent<Ally>().SwitchSide();
            charmedObj.GetComponent<Enemy>().InitializeSummoned(wayPointIndex, enemySpawner);
            charmedObj.GetComponent<Enemy>().FlipDirection(wayPoint.position);
            charmedObj.transform.SetParent(enemySpawner.transform);
            

        }
    }
}
