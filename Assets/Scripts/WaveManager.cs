using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class WaveManager : MonoBehaviour
{
    private float waveTimer;
    private float waveDuration;
    private bool waveCanStart = true; // biar bisa munculin tombol wavestart duluan
    public bool waveStart;
    public int waveCount;
    [HideInInspector] public int currentWave;
    [HideInInspector] public UnityEvent OnWaveCanStart;
    [HideInInspector] public UnityEvent OnNewWave;

    private void Start()
    {
        currentWave = 0;
        StartWave();
    }

    private void Update()
    {
        CalcWaveStartTime();
    }

    private void CalcWaveStartTime()
    {
        waveTimer += Time.deltaTime;
        if(waveTimer >= waveDuration * 8 / 10){
            waveCanStart = true;
            OnWaveCanStart?.Invoke();
            if(waveTimer >= waveDuration + 5f){
                StartWave();
            }
        }
    }

    public void StartWave()
    {
        if(waveCanStart && currentWave < waveCount){
            SwitchWaveBool(true);
            waveCanStart = false;
            currentWave++;
            OnNewWave?.Invoke();
        }
    }

    public void SwitchWaveBool(bool waveBool)
    {
        waveStart = waveBool;
    }

    public void SetWaveDuration(float waveDuration)
    {
        if(waveDuration > this.waveDuration){
            this.waveDuration = waveDuration;
            waveTimer = 0;
        }
    }
}
