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
    [HideInInspector] public UnityEvent OnAddGold;
    public UnityEvent OnHeroHealthChanged;
    [HideInInspector] public UnityEvent OnLifeBreak;

    private void Awake()
    {
        if(Instance == null){
            Instance = this;
        }
    }

    private void Update()
    {
        SwitchCam();
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
}
