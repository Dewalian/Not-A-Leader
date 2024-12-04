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
    public int waveCount;
    [SerializeField] private float startEarlyDurationPercentage;
    [SerializeField] private float restDuration;
    [HideInInspector] public float maxWaveDuration;
    [HideInInspector] public int currentWave;
    [HideInInspector] public UnityEvent OnWaveCanStart;
    [HideInInspector] public UnityEvent OnStartWave;
    private Coroutine calcWaveCoroutine;

    private void Start()
    {
        currentWave = 0;

        OnWaveCanStart?.Invoke();
    }

    private IEnumerator CalcWaveStartTime()
    {
        while(waveTimer < maxWaveDuration * startEarlyDurationPercentage / 100){
            waveTimer += Time.deltaTime;
            yield return null;
        }

        OnWaveCanStart?.Invoke();

        while(waveTimer < maxWaveDuration){
            waveTimer += Time.deltaTime;
            yield return null;
        }

        StartWave();
    }

    public void StartWave()
    {
        if(currentWave < waveCount){
            maxWaveDuration = restDuration;
            maxWaveDuration = 0;
            currentWave++;
            OnStartWave?.Invoke();

            if(calcWaveCoroutine != null) StopCoroutine(calcWaveCoroutine);
            calcWaveCoroutine = StartCoroutine(CalcWaveStartTime());
        }
    }

    public void SetMaxWaveDuration(float waveDuration)
    {
        if(waveDuration + restDuration > maxWaveDuration){
            maxWaveDuration = waveDuration + restDuration;
            Debug.Log(maxWaveDuration);
            waveTimer = 0;
        }
    }
}
