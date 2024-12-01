using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class WaveManager : MonoBehaviour
{
    private float waveTimer;
    private bool waveCanStart;
    public bool waveStart;
    public int waveCount;
    [SerializeField] private float restDuration;
    [HideInInspector] public float waveDuration;
    [HideInInspector] public int currentWave;
    [HideInInspector] public UnityEvent OnWaveCanStart;
    [HideInInspector] public UnityEvent OnNewWave;

    private void Start()
    {
        currentWave = 0;
        waveStart = false;
        waveCanStart = true;

        OnWaveCanStart?.Invoke();
    }

    private void Update()
    {
        if(currentWave > 0){
            CalcWaveStartTime();
        }
    }

    private void CalcWaveStartTime()
    {
        waveTimer += Time.deltaTime;
        if(waveTimer >= waveDuration * 8 / 10){
            waveCanStart = true;
            OnWaveCanStart?.Invoke();
            if(waveTimer >= waveDuration){
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
            waveDuration = restDuration;
        }
    }

    public void SwitchWaveBool(bool waveBool)
    {
        waveStart = waveBool;
    }

    public void SetWaveDuration(float waveDuration)
    {
        if(waveDuration + restDuration > this.waveDuration){
            this.waveDuration = waveDuration + restDuration;
            waveTimer = 0;
            OnNewWave?.Invoke();
        }
    }
}
