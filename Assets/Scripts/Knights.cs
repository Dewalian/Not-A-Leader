using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knights : MonoBehaviour
{
    [SerializeField] private float aggroRange = 1.5f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private GameObject[] knights;
    void Update()
    {
        Collider2D unitInRange = Physics2D.OverlapCircle(transform.position, aggroRange, enemyLayer);
        if(unitInRange){
            foreach(GameObject k in knights){
                k.GetComponent<Unit>().EnemyToAggro(unitInRange);
            }
        }
    }
}
