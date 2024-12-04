using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemySpawnerSO enemySpawnerSO;
    private EnemySpawnerSO.Wave currentWave;
    private EnemySpawnerSO.Wave nextWave;
    private WaveManager waveManager;
    private float waveDuration;
    private int spawnerIndex;
    [HideInInspector] public List<Transform> wayPoint;
    [HideInInspector] public UnityEvent<int, bool> OnCheckSpawn;

    private void Awake()
    {
        waveManager = GetComponentInParent<WaveManager>();

        foreach(Transform child in transform){
            wayPoint.Add(child);
        }
    }
    
    private void OnEnable()
    {
        waveManager.OnStartWave.AddListener(() => StartSpawn());
        waveManager.OnWaveCanStart.AddListener(() => CalcWaveDuration());
    }

    private void OnDisable()
    {
        waveManager.OnStartWave.RemoveListener(() => StartSpawn());
        waveManager.OnWaveCanStart.RemoveListener(() => CalcWaveDuration());
    }

    private void CalcWaveDuration()
    {
        waveDuration = 0;
        nextWave = enemySpawnerSO.waves[waveManager.currentWave];

        for(int i=0; i<nextWave.miniWave.Length; i++){
            waveDuration += nextWave.miniWaveSpawnCD;
            for(int j=0; j<nextWave.miniWave[i].enemies.Length; j++){
                waveDuration += nextWave.miniWave[i].enemiesSpawnCD;
            }
        }
        CheckSpawn();
    }

    private void CheckSpawn(){
        if(waveDuration > 0){
            OnCheckSpawn?.Invoke(spawnerIndex, true);
        }else{
            OnCheckSpawn?.Invoke(spawnerIndex, false);
        }
    }

    private void StartSpawn()
    {
        currentWave = enemySpawnerSO.waves[waveManager.currentWave - 1];
        waveManager.SetMaxWaveDuration(waveDuration);
        StartCoroutine(SpawnMiniWave());
    }

    

    private IEnumerator SpawnMiniWave()
    {
        foreach(EnemySpawnerSO.MiniWave mw in currentWave.miniWave){
            StartCoroutine(SpawnEnemies(mw));
            yield return new WaitForSeconds(currentWave.miniWaveSpawnCD);
        }
    }

    private IEnumerator SpawnEnemies(EnemySpawnerSO.MiniWave miniWave)
    {
        foreach(GameObject e in miniWave.enemies){
            GameObject enemyObj = Instantiate(e, wayPoint[0].position, Quaternion.identity);
            enemyObj.transform.SetParent(gameObject.transform);
            yield return new WaitForSeconds(miniWave.enemiesSpawnCD);
        }
    }

    public void SetSpawnerIndex(int spawnerIndex)
    {
        this.spawnerIndex = spawnerIndex;
    }
}
