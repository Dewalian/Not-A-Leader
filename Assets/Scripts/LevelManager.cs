using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance = null;
    public int gold;
    public int life;
    public float heroHealth;
    public Camera mainCam;
    public Camera levelCam;
    public Transform[] ostradFlagPos;
    public EnemySpawner[] ostradSpawners;
    [HideInInspector] public UnityEvent OnAddGold;
    [HideInInspector] public UnityEvent OnHeroHealthChanged;
    [HideInInspector] public UnityEvent OnLifeBreak;
    [HideInInspector] public UnityEvent OnWin;
    [HideInInspector] public UnityEvent OnDefeat;

    private void Awake()
    {
        if(Instance == null){
            Instance = this;
        }
    }

    private void Update()
    {
        SwitchCam();
        
        if(life <= 0){
            Defeat();
        }
    }

    private void SwitchCam()
    {
        if(Input.GetKeyDown(KeyCode.Alpha2)){
            if(mainCam.enabled){
                levelCam.enabled = true;
                mainCam.enabled = false;
            }else{
                mainCam.enabled = true;
                levelCam.enabled = false;
            }
        }
    }

    public void AddGold(int gold)
    {
        this.gold += gold;
        OnAddGold?.Invoke();
    }

    public void UpdateHeroHealth(float heroHealth)
    {
        this.heroHealth = heroHealth;
        OnHeroHealthChanged?.Invoke();
    }

    public void LifeBreak(int lifeDamage)
    {
        life -= lifeDamage;
        OnLifeBreak?.Invoke();
    }

    public void Win()
    {
        Time.timeScale = 0;
        OnWin?.Invoke();
    }

    public void Defeat()
    {
        Time.timeScale = 0;
        OnDefeat?.Invoke();
    }
}
