using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCanvas : PlotCanvas
{
    private Tower tower;
    private float AREAVISUAL_MODIFIER = 400f;
    [SerializeField] private Transform areaVisual;

    private void Awake()
    {
        tower = GetComponentInParent<Tower>();
    }

    private void OnEnable()
    {
        tower.OnUpgrade.AddListener(() => IncreaseAreaVisual());
    }

    private void IncreaseAreaVisual()
    {
        float range = tower.GetRange() * AREAVISUAL_MODIFIER;
        areaVisual.localScale = new Vector3(range, range, 0);
    }

    public void SellTower()
    {
        tower.SellTower();
    }

    public void UpgradeTower()
    {
        tower.UpgradeTower();
    }
}
