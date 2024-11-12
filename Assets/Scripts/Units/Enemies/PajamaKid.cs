using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PajamaKid : Enemy
{
    [SerializeField] private CircleCollider2D debuffArea;
    [SerializeField] private float debuffAreaRange;
    [SerializeField] private float slowMoveSpeedPower;
    [SerializeField] private float slowShootCDPower;

    protected override void Start()
    {
        base.Start();
        debuffArea.radius = debuffAreaRange;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Ally")){
            other.GetComponent<Unit>().ChangeMoveSpeed(slowMoveSpeedPower);
            Debug.Log("Mantap");
        }else if(other.CompareTag("Tower")){
            other.GetComponent<ShootingTower>()?.ChangeShootCD(slowShootCDPower);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Ally")){
            other.GetComponent<Unit>().ChangeMoveSpeed(-slowMoveSpeedPower);
        }else if(other.CompareTag("Tower")){
            other.GetComponent<ShootingTower>().ChangeShootCD(-slowShootCDPower);
        }
    }
}
