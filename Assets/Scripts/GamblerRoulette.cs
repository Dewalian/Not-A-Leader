using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamblerRoulette : MonoBehaviour
{
    private Joker joker;

    private void Awake()
    {
        joker = GetComponentInParent<Joker>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Tower")){
            Tower tower = other.GetComponent<Tower>();
            joker.OnGamblerRouletteEnter(other.gameObject, tower.towerName, tower.towerSymbol);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Tower")){
            Tower tower = other.GetComponent<Tower>();
            joker.OnGamblerRouletteExit(other.gameObject, tower.towerName, tower.towerSymbol);
        }
    }
}
