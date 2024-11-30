using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JokerCanvas : UnitCanvas
{
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text towerCountText;
    [SerializeField] private GamblerRoulette gamblerRoulette;
    private float goldRequired;
    private float towerCountRequired;
    private float uniqueTowerCountRequired;

    protected override void Start()
    {
        base.Start();
        goldRequired = gamblerRoulette.goldRequired;
        towerCountRequired = gamblerRoulette.towerCountRequired;
        uniqueTowerCountRequired = gamblerRoulette.uniqueTowerCountRequired;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        gamblerRoulette.OnUpdateStats.AddListener(() => UpdateStats());
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        gamblerRoulette.OnUpdateStats.RemoveListener(() => UpdateStats());
    }

    private void UpdateStats()
    {
        float gold = LevelManager.Instance.gold;
        float towerCount = gamblerRoulette.towers.Count;
        float uniqueTowerCount = gamblerRoulette.uniqueTowerNames.Count;

        goldText.text = string.Format("Gold: {0}/{1}", gold, goldRequired);
        towerCountText.text = string.Format("Tower: {0}/{1} ({2}/{3})", 
        gamblerRoulette.towers.Count, uniqueTowerCountRequired, towerCount, towerCountRequired);
    }

}
