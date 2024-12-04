using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnerSkulls : MonoBehaviour
{
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private GameObject skull;
    [SerializeField] private List<GameObject> skulls;
    [SerializeField] private List<EnemySpawner> enemySpawners;

    private void Awake()
    {
        foreach(Transform child in waveManager.transform){
            enemySpawners.Add(child.GetComponent<EnemySpawner>());
        }
    }

    private void OnEnable()
    {
        for(int i=0; i<enemySpawners.Count; i++){
            enemySpawners[i].SetSpawnerIndex(i);
            enemySpawners[i].OnCheckSpawn.AddListener((int spawnerIndex, bool isShow) => ShowSkull(spawnerIndex, isShow));
        }

        waveManager.OnStartWave.AddListener(() => HideSkulls());
    }

    private void OnDisable()
    {
        for(int i=0; i<enemySpawners.Count; i++){
            enemySpawners[i].OnCheckSpawn.RemoveListener((int spawnerIndex, bool isShow) => ShowSkull(spawnerIndex, isShow));
        }

        waveManager.OnStartWave.RemoveListener(() => HideSkulls());
    }

    private void Start()
    {
        foreach(EnemySpawner e in enemySpawners){
            GameObject skullObj = Instantiate(skull, e.wayPoint[0].position, Quaternion.identity);
            skullObj.transform.SetParent(transform);
            skulls.Add(skullObj);
        }
    }

    private void ShowSkull(int spawnerIndex, bool isShow)
    {
        skulls[spawnerIndex].SetActive(isShow);
    }

    private void HideSkulls()
    {
        foreach(GameObject s in skulls){
            s.SetActive(false);
        }
    }
}
