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
            joker.OnGamblerRouletteEnter(other.gameObject, other.GetComponent<Tower>().towerName);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Tower")){
            joker.OnGamblerRouletteExit(other.gameObject, other.GetComponent<Tower>().towerName);
        }
    }
}
