using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
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
    private int waveIndex;

    void Update()
    {
        if(LevelManager.instance.waveStart){
            StartCoroutine(SpawnMiniWave());
        }
    }

    IEnumerator SpawnMiniWave(){
        LevelManager.instance.waveStart = false;
        foreach(MiniWave e in waves[waveIndex].miniWave){
            StartCoroutine(SpawnEnemies(e));
            yield return new WaitForSeconds(waves[waveIndex].spawnCD);
        }
    }

    IEnumerator SpawnEnemies(MiniWave mw){
        foreach(GameObject e in mw.enemies){
            Instantiate(e, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(mw.spawnCD);
        }
    }
}
