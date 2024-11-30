using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamblerRouletteCollider : MonoBehaviour
{
    private GamblerRoulette gamblerRoulette;
    private void Start()
    {
        gamblerRoulette = GetComponentInParent<GamblerRoulette>();    
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Tower")){
            Tower tower = other.GetComponent<Tower>();
            gamblerRoulette.OnAreaEnter(tower.gameObject, tower.towerName, tower.towerSymbol);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Tower")){
            Tower tower = other.GetComponent<Tower>();
            gamblerRoulette.OnAreaExit(tower.gameObject, tower.towerName, tower.towerSymbol);
        }
    }    
}
