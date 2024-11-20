using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageChargeCanvas : MonoBehaviour
{
    [SerializeField] private MageTower mageTower;
    [SerializeField] private List<GameObject> charges;
    [SerializeField] private List<GameObject> chargesToAdd;
    [SerializeField] private Color falseChargeColor;
    private int trueChargeCount;

    private void Start()
    {
        mageTower.OnUpgrade.AddListener(() => Upgrade());
        mageTower.OnDifferentTarget.AddListener(() => DifferentTarget());
        mageTower.OnSameTarget.AddListener(() => SameTarget());
    }

    private void Upgrade()
    {
        charges.Add(chargesToAdd[0]);
        charges[charges.Count - 1].SetActive(true);
        chargesToAdd.RemoveAt(0);
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
