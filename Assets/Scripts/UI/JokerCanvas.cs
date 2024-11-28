using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JokerCanvas : UnitCanvas
{
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text towerCountText;
    [SerializeField] private TMP_Text uniqueTowerCountText;
    private Joker joker;

    protected override void Awake()
    {
        base.Awake();
        joker = GetComponentInParent<Joker>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        joker.OnUpdateStats.AddListener(() => UpdateStats());
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        joker.OnUpdateStats.RemoveListener(() => UpdateStats());
    }

    private void UpdateStats()
    {
        goldText.text = LevelManager.Instance.gold.ToString();
        towerCountText.text = joker.towers.Count.ToString();
        uniqueTowerCountText.text = joker.uniqueTowerNames.Count.ToString();
    }

}
