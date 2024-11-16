using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private float waveTimer;
    private float waveDuration;
    private bool waveCanStart; // biar bisa munculin tombol wavestart duluan
    public bool waveStart = true;
    public int waveCount = 0;

    private void Update()
    {
        CalcWaveTime();
    }

    private void CalcWaveTime()
    {
        waveTimer += Time.deltaTime;
        if(waveTimer >= Mathf.FloorToInt(waveDuration * 8 / 10)){
            waveCanStart = true;
        }else{
            waveCanStart = false;
        }
    }

    public void SetWaveDuration(float waveDuration)
    {
        if(waveDuration > this.waveDuration){
            this.waveDuration = waveDuration;
            waveTimer = 0;
        }
    }    
}
