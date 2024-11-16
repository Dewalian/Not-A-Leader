using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowArea : MonoBehaviour
{
    private PajamaKid pajamaKid;

    private void Awake()
    {
        pajamaKid = GetComponentInParent<PajamaKid>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        pajamaKid.OnSlowAreaEnter(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        pajamaKid.OnSlowAreaExit(other);
    }
}
