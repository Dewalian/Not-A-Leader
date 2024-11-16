using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PajamaKid : Enemy
{
    [SerializeField] private CircleCollider2D debuffArea;
    [SerializeField] private float debuffAreaRange;
    [SerializeField] private float debuffPercentage;

    protected override void Start()
    {
        base.Start();
        debuffArea.radius = debuffAreaRange;
    }

    public void OnSlowAreaEnter(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Ally")){
            Debug.Log(other);
            other.GetComponent<Unit>().ChangeStats(-debuffPercentage / 100);
        }else if(other.gameObject.layer == LayerMask.NameToLayer("Tower")){
            other.GetComponent<ShootingTower>()?.ChangeStats(-debuffPercentage / 100);
        }
    }

    public void OnSlowAreaExit(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Ally")){
            other.GetComponent<Unit>().ChangeStats(debuffPercentage/100);
        }else if(other.gameObject.layer == LayerMask.NameToLayer("Tower")){
            other.GetComponent<ShootingTower>().ChangeStats(debuffPercentage / 100);
        }
    }
}
