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
    [HideInInspector] public UnityEvent OnAddGold, OnHeroHealthChanged, OnWaveStartEarly;

    private void Awake()
    {
        if(Instance == null){
            Instance = this;
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
}
