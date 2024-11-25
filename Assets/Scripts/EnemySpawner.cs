using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private WaveManager waveManager;
    private Wave currentWave;

    [Serializable]
    public struct Wave{
        public MiniWave[] miniWave;
        public float miniWaveSpawnCD;
    }

    [Serializable]
    public struct MiniWave{
        public GameObject[] enemies;
        public float enemiesSpawnCD;
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
            currentWave = waves[waveManager.currentWave-1];
            CalcWaveDuration();
            StartCoroutine(SpawnMiniWave());
            waveManager.SwitchWaveBool(false);
        }
    }

    private void CalcWaveDuration()
    {
        float waveDuration = 0;

        for(int i=0; i<currentWave.miniWave.Length; i++){
            waveDuration += currentWave.miniWaveSpawnCD;
            for(int j=0; j<currentWave.miniWave[i].enemies.Length; j++){
                waveDuration += currentWave.miniWave[i].enemiesSpawnCD;
            }
        }

        waveManager.SetWaveDuration(waveDuration);
    }

    private IEnumerator SpawnMiniWave()
    {
        foreach(MiniWave mw in currentWave.miniWave){
            StartCoroutine(SpawnEnemies(mw));
            yield return new WaitForSeconds(currentWave.miniWaveSpawnCD);
        }
    }

    private IEnumerator SpawnEnemies(MiniWave miniWave)
    {
        foreach(GameObject e in miniWave.enemies){
            GameObject enemyObj = Instantiate(e, wayPoint[0].position, Quaternion.identity);
            enemyObj.transform.SetParent(gameObject.transform);
            yield return new WaitForSeconds(miniWave.enemiesSpawnCD);
        }
    }
}
