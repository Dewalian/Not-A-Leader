using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ostrad : Enemy
{
    [SerializeField] private float warScreamRange;
    [SerializeField] private float warScreamCD;
    [SerializeField] private float earthShakerRange;
    [SerializeField] private float earthShakerCD;
    [SerializeField] private LayerMask towerLayer;
    [SerializeField] private LayerMask unitLayer;
    private bool canWarScream;
    private bool canEarthShaker;

    protected override void Update()
    {
        base.Update();
        WarScreamDetect();
        EarthShakerDetect();
    }

    private void WarScreamDetect()
    {
        if(canWarScream){
            Collider2D[] unitInWarScream = Physics2D.OverlapCircleAll(transform.position, warScreamRange, unitLayer);
            if(unitInWarScream.Length >= 10){
                unitState = State.Skill;
                WarScream(unitInWarScream);
            }
        }
    }

    private void WarScream(Collider2D[] unitInWarScream)
    {
        canWarScream = false;
        foreach(Collider2D u in unitInWarScream){
            u.GetComponent<Unit>().Death();
        }
        unitState = State.Neutral;
        StartCoroutine(WarScreamCD());
    }

    private IEnumerator WarScreamCD()
    {
        yield return new WaitForSeconds(warScreamCD);
        canWarScream = true;
    }

    private void EarthShakerDetect()
    {
        if(canEarthShaker){
            Collider2D[] towerInEarthShaker = Physics2D.OverlapCircleAll(transform.position, warScreamRange, towerLayer);
            if(towerInEarthShaker.Length >= 1){
                unitState = State.Skill;
                EarthShaker(towerInEarthShaker);
            }
        }
    }

    private void EarthShaker(Collider2D[] towerInEarthShaker)
    {
        canEarthShaker = false;
        foreach(Collider2D t in towerInEarthShaker){
            t.GetComponent<Tower>().DestroyTower();
        }
        StartCoroutine(EarthShakerCD());
    }

    private IEnumerator EarthShakerCD()
    {
        yield return new WaitForSeconds(earthShakerCD);
        canEarthShaker = true;
    }
}
