using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JokerCanvas : UnitCanvas
{
    [SerializeField] private GamblerRoulette gamblerRoulette;
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text towerCountText;
    [SerializeField] private MadGambler madGambler;
    [SerializeField] private TMP_Text faceText;
    [SerializeField] private TMP_Text valueText;
    [SerializeField] private Color trueColor;
    [SerializeField] private Color falseColor;
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
        gamblerRoulette.OnUpdateGamblerRoulette.AddListener(() => UpdateGamblerRoulette());
        madGambler.OnUpdateMadGambler.AddListener((bool sameFace, bool sameValue) => UpdateMadGambler(sameFace, sameValue));
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        gamblerRoulette.OnUpdateGamblerRoulette.RemoveAllListeners();

    }

    private void UpdateGamblerRoulette()
    {
        float gold = LevelManager.Instance.gold;
        float towerCount = gamblerRoulette.towers.Count;
        float uniqueTowerCount = gamblerRoulette.uniqueTowerNames.Count;

        goldText.text = string.Format("Gold: {0}/{1}", gold, goldRequired);
        towerCountText.text = string.Format("Tower: {0}/{1} ({2}/{3})", 
        towerCount, towerCountRequired, uniqueTowerCount, uniqueTowerCountRequired);
    }

    private void UpdateMadGambler(bool sameFace, bool sameValue)
    {
        if(sameFace){
            faceText.text = "Same face";
        }else{
            faceText.text = "Different face";
        }

        if(sameValue){
            valueText.text = "Same value";
        }else{
            valueText.text = "Different value";
        }
    }
}
