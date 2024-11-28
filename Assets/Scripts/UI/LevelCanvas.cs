using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelCanvas : MonoBehaviour
{
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text lifeText;
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private Slider heroHealthBar;
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private Sprite waveStartButtonFalse;
    [SerializeField] private Sprite waveStartButtonTrue;
    [SerializeField] private Image waveStartButtonImage;
    private float heroMaxHealth;
    private void Start()
    {


        heroMaxHealth = LevelManager.Instance.heroHealth;
    }

    private void OnEnable()
    {
        LevelManager.Instance.OnAddGold.AddListener(() => UpdateGoldText());
        LevelManager.Instance.OnHeroHealthChanged.AddListener(() => UpdateHeroHealthUI());
        LevelManager.Instance.OnLifeBreak.AddListener(() => UpdateLifeText());

        waveManager.OnWaveCanStart.AddListener(() => WaveCanStart());
        waveManager.OnNewWave.AddListener(() => UpdateWaveText());
    }

    private void OnDisable()
    {
        LevelManager.Instance.OnAddGold.RemoveListener(() => UpdateGoldText());
        LevelManager.Instance.OnHeroHealthChanged.RemoveListener(() => UpdateHeroHealthUI());
        LevelManager.Instance.OnLifeBreak.RemoveListener(() => UpdateLifeText());

        waveManager.OnWaveCanStart.RemoveListener(() => WaveCanStart());
        waveManager.OnNewWave.RemoveListener(() => UpdateWaveText());
    }

    private void UpdateGoldText()
    {
        goldText.text = LevelManager.Instance.gold.ToString();
    }

    private void UpdateHeroHealthUI()
    {
        heroHealthBar.value = LevelManager.Instance.heroHealth / heroMaxHealth;
    }

    private void UpdateLifeText()
    {
        lifeText.text = LevelManager.Instance.life.ToString();
    }

    private void UpdateWaveText()
    {
        waveText.text = waveManager.currentWave.ToString();
    }

    private void WaveCanStart()
    {
        waveStartButtonImage.sprite = waveStartButtonTrue;
    }

    public void StartWave()
    {
        waveManager.StartWave();
        waveStartButtonImage.sprite = waveStartButtonFalse;
    }
}
