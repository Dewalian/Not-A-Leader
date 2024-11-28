using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlotCanvas : ToggleCanvas
{
    [SerializeField] private Plot plot;
    [SerializeField] private TMP_Text[] towerCostTexts;

    private void Start()
    {
        SetTowerPrice();
    }

    private void SetTowerPrice()
    {
        for(int i=0; i<plot.towers.Length; i++){
            Tower tScript = plot.towers[i].GetComponent<Tower>();
            towerCostTexts[i].text = tScript.costs[0].ToString();
        }
    }
}
