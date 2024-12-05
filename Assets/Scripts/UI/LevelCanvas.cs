using System;
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
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private Sprite waveStartButtonFalse;
    [SerializeField] private Sprite waveStartButtonTrue;
    [SerializeField] private Image waveStartButtonImage;
    private bool waveCanStart;
    private float timer;
    private float heroMaxHealth;

    private void OnEnable()
    {
        LevelManager.Instance.OnAddGold.AddListener(() => UpdateGoldText());
        LevelManager.Instance.OnHeroHealthChanged.AddListener(() => UpdateHeroHealthUI());
        LevelManager.Instance.OnLifeBreak.AddListener(() => UpdateLifeText());

        waveManager.OnWaveCanStart.AddListener(() => WaveCanStart());
        waveManager.OnStartWave.AddListener(() => UpdateWaveText());
        waveManager.OnStartWave.AddListener(() => WaveCannotStart());
    }

    private void OnDisable()
    {
        LevelManager.Instance.OnAddGold.RemoveListener(() => UpdateGoldText());
        LevelManager.Instance.OnHeroHealthChanged.RemoveListener(() => UpdateHeroHealthUI());
        LevelManager.Instance.OnLifeBreak.RemoveListener(() => UpdateLifeText());

        waveManager.OnWaveCanStart.RemoveListener(() => WaveCanStart());
        waveManager.OnStartWave.RemoveListener(() => UpdateWaveText());
        waveManager.OnStartWave.RemoveListener(() => WaveCannotStart());
    }

    private void Start()
    {
        heroMaxHealth = LevelManager.Instance.heroHealth;
        UpdateGoldText();
        UpdateHeroHealthUI();
        UpdateLifeText();
        UpdateWaveText();

        waveCanStart = true;
        timer = 0;
    }

    private void Update()
    {
        UpdateTimerText();
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
        waveText.text = String.Format("Wave: {0}/{1}", waveManager.currentWave, waveManager.waveCount);
        timer = waveManager.maxWaveDuration;        
    }

    private void UpdateTimerText()
    {
        if(timer > 1){
            timer -= Time.deltaTime;

            int minutes = Mathf.FloorToInt(timer / 60);
            int seconds = Mathf.FloorToInt(timer % 60);

            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    private void WaveCanStart()
    {
        waveStartButtonImage.sprite = waveStartButtonTrue;
        waveCanStart = true;
    }

    private void WaveCannotStart()
    {
        waveStartButtonImage.sprite = waveStartButtonFalse;
        waveCanStart = false;
    }

    public void StartWave()
    {
        if(waveCanStart){
            waveManager.StartWave();
        }
    }
}
