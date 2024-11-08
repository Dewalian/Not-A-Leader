using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCanvas : PlotCanvas
{
    [SerializeField] private Tower tower;

    private void Start()
    {
        tower = GetComponentInParent<Tower>();
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
