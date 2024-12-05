    using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject defeatPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject skill1Overlay;
    [SerializeField] private GameObject skill2Overlay;
    [SerializeField] private GameObject[] tutorialPanels;
    private int tutorialIndex;
    private bool waveCanStart;
    private float timer;
    private float heroMaxHealth;

    private void OnEnable()
    {
        LevelManager.Instance.OnAddGold.AddListener(() => UpdateGoldText());
        LevelManager.Instance.OnHeroHealthChanged.AddListener(() => UpdateHeroHealthUI());
        LevelManager.Instance.OnLifeBreak.AddListener(() => UpdateLifeText());

        LevelManager.Instance.OnWin.AddListener(() => Win());
        LevelManager.Instance.OnDefeat.AddListener(() => Defeat());

        LevelManager.Instance.OnSkill1CD.AddListener((float CD) => StartCoroutine(Skill1CD(CD)));
        LevelManager.Instance.OnSkill2CD.AddListener((float CD) => StartCoroutine(Skill2CD(CD)));

        waveManager.OnWaveCanStart.AddListener(() => WaveCanStart());
        waveManager.OnStartWave.AddListener(() => UpdateWaveText());
        waveManager.OnStartWave.AddListener(() => WaveCannotStart());
    }

    private void OnDisable()
    {
        LevelManager.Instance.OnAddGold.RemoveListener(() => UpdateGoldText());
        LevelManager.Instance.OnHeroHealthChanged.RemoveListener(() => UpdateHeroHealthUI());
        LevelManager.Instance.OnLifeBreak.RemoveListener(() => UpdateLifeText());

        LevelManager.Instance.OnWin.RemoveListener(() => Win());
        LevelManager.Instance.OnDefeat.RemoveListener(() => Defeat());

        LevelManager.Instance.OnSkill1CD.RemoveListener((float CD) => Skill1CD(CD));
        LevelManager.Instance.OnSkill2CD.RemoveListener((float CD) => Skill2CD(CD));

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

        winPanel.SetActive(false);
        defeatPanel.SetActive(false);

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

    private void Win()
    {
        winPanel.SetActive(true);
    }

    private void Defeat()
    {
        defeatPanel.SetActive(true);
    }

    private IEnumerator Skill1CD(float CD)
    {
        skill1Overlay.SetActive(true);
        yield return new WaitForSeconds(CD);
        skill1Overlay.SetActive(false);
    }

    private IEnumerator Skill2CD(float CD)
    {
        skill2Overlay.SetActive(true);
        yield return new WaitForSeconds(CD);
        skill2Overlay.SetActive(false);
    }

    public void Tutorial()
    {
        tutorialPanels[tutorialIndex].SetActive(false);
        tutorialIndex++;

        if(tutorialIndex < tutorialPanels.Length){
            tutorialPanels[tutorialIndex].SetActive(true);
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void Settings(bool isPause)
    {
        if(isPause){
            Time.timeScale = 0;
            settingsPanel.SetActive(true);
        }else{
            Time.timeScale = 1;
            settingsPanel.SetActive(false);
        }
    }

    public void StartWave()
    {
        if(waveCanStart){
            waveManager.StartWave();
        }
    }
}
