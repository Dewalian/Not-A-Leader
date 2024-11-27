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
        LevelManager.Instance.OnAddGold.AddListener(() => UpdateGoldText());
        LevelManager.Instance.OnHeroHealthChanged.AddListener(() => UpdateHeroHealthUI());
        LevelManager.Instance.OnLifeBreak.AddListener(() => UpdateLifeText());

        waveManager.OnWaveCanStart.AddListener(() => WaveCanStart());
        waveManager.OnNewWave.AddListener(() => UpdateWaveText());

        UpdateGoldText();
        UpdateHeroHealthUI();
        UpdateLifeText();

        heroMaxHealth = LevelManager.Instance.heroHealth;
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
        Debug.Log("Nambah");
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
