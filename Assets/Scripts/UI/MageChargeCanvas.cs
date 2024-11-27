using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageChargeCanvas : MonoBehaviour
{
    [SerializeField] private MageTower mageTower;
    [SerializeField] private GameObject chargeContainer;
    [SerializeField] private List<GameObject> charges;
    [SerializeField] private Color falseChargeColor;
    private int trueChargeCount;

    private void Start()
    {
        mageTower.OnUpgrade.AddListener(() => IncreaseCharge());
        mageTower.OnDifferentTarget.AddListener(() => DifferentTarget());
        mageTower.OnSameTarget.AddListener(() => SameTarget());
    }

    private void IncreaseCharge()
    {
        GameObject chargeObj = Instantiate(chargeContainer, transform);
        charges.Add(chargeObj);
    }

    private void DifferentTarget()
    {
        foreach(GameObject c in charges){
            c.GetComponentInChildren<SpriteRenderer>().color = falseChargeColor;
        }
        trueChargeCount = 0;
    }

    private void SameTarget()
    {
        trueChargeCount++;
        trueChargeCount = Mathf.Min(3, trueChargeCount);
        charges[trueChargeCount-1].GetComponentInChildren<SpriteRenderer>().color = Color.white;
    }
}
