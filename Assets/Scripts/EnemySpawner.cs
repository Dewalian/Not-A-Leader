using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private WaveManager waveManager;

    [Serializable]
    public struct Wave{
        public MiniWave[] miniWave;
        public float spawnCD;
    }

    [Serializable]
    public struct MiniWave{
        public GameObject[] enemies;
        public float spawnCD;
    }
    [SerializeField] private List<Wave> waves;
    public Transform[] wayPoint;
    

    private void Start()
    {
        waveManager = GetComponentInParent<WaveManager>();
    }

    private void Update()
    {
        if(waveManager.waveStart){
            CalcWaveDuration();
            StartCoroutine(SpawnMiniWave());
        }
    }

    private void CalcWaveDuration()
    {
        float waveDuration = 0;
        Wave currentWave = waves[waveManager.waveCount];

        waveDuration += currentWave.spawnCD;
        foreach(MiniWave mw in currentWave.miniWave){
            waveDuration += mw.spawnCD;
        }

        waveManager.SetWaveDuration(waveDuration);
    }

    private IEnumerator SpawnMiniWave()
    {
        waveManager.waveStart = false;
        foreach(MiniWave mw in waves[waveManager.waveCount].miniWave){
            StartCoroutine(SpawnEnemies(mw));
            yield return new WaitForSeconds(waves[waveManager.waveCount].spawnCD);
        }
    }

    private IEnumerator SpawnEnemies(MiniWave miniWave)
    {
        foreach(GameObject e in miniWave.enemies){
            GameObject enemyObj = Instantiate(e, wayPoint[0].position, Quaternion.identity);
            enemyObj.transform.SetParent(gameObject.transform);
            yield return new WaitForSeconds(miniWave.spawnCD);
        }
    }
}
