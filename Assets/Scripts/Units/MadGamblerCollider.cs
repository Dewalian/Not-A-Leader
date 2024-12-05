using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MadGamblerCollider : MonoBehaviour
{
    private MadGambler madGambler;

    private void Start()
    {
        madGambler = GetComponentInParent<MadGambler>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy") && madGambler.canEffect){
            other.GetComponent<Unit>().OnDeath.AddListener(() => StartCoroutine(madGambler.JokerEffect(other.transform.position)));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Enemy")){
            other.GetComponent<Unit>().OnDeath.RemoveListener(() => madGambler.JokerEffect(other.transform.position));
        }
    }
}
