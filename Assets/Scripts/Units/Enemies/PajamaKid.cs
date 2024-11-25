using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PajamaKid : Enemy
{
    [SerializeField] private GameObject debuffArea;
    [SerializeField] private float debuffAreaRange;
    [SerializeField] private float debuffPercentage;
    [SerializeField] private ParticleSystem slothParticle;

    protected override void Start()
    {
        base.Start();
        debuffArea.transform.localScale = new Vector2(debuffAreaRange, debuffAreaRange);
        var particleShape = slothParticle.shape;
        particleShape.radius = debuffAreaRange;
    }

    public void OnSlowAreaEnter(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Ally")){
            other.GetComponent<Unit>().ChangeStats(debuffPercentage);
        }else if(other.gameObject.layer == LayerMask.NameToLayer("Tower")){
            other.GetComponent<Tower>()?.ChangeStats(debuffPercentage);
        }
    }

    public void OnSlowAreaExit(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Ally")){
            other.GetComponent<Unit>().ChangeStats(-debuffPercentage);
        }else if(other.gameObject.layer == LayerMask.NameToLayer("Tower")){
            other.GetComponent<Tower>().ChangeStats(-debuffPercentage);
        }
        
    }
}
