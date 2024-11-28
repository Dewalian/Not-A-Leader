using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TowerCanvas : ToggleCanvas
{
    private Tower tower;
    [SerializeField] private Transform areaVisual;
    [SerializeField] private GameObject starContainer;
    [SerializeField] private Transform starCharge;
    [SerializeField] private TMP_Text upgradeCostText;
    [SerializeField] private GameObject upgradeButton;

    private void Awake()
    {
        tower = GetComponentInParent<Tower>();
    }

    private void Start()
    {
        SetCost();
        AddStar();
    }

    private void OnEnable()
    {
        tower.OnUpgrade.AddListener(() => IncreaseAreaVisual());
    }

    private void OnDisable()
    {
        tower.OnUpgrade.RemoveListener(() => IncreaseAreaVisual());
    }

    private void SetCost()
    {
        if(tower.level < 2){
            upgradeCostText.text = tower.costs[tower.level+1].ToString();
        }
    }

    private void AddStar()
    {
        Instantiate(starContainer, starCharge);
    }

    private void IncreaseAreaVisual()
    {
        float range = tower.GetRange() * 400;
        areaVisual.localScale = new Vector3(range, range, 0);
    }

    public void SellTower()
    {
        tower.SellTower();
    }

    public void UpgradeTower()
    {
        if(tower.level < 2 && LevelManager.Instance.gold >= tower.costs[tower.level+1]){
            tower.UpgradeTower();
            SetCost();
            AddStar();
        }

        if(tower.level == 2){
            upgradeButton.SetActive(false);
        }
    }
}
