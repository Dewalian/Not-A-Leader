using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Scriptable Objects/Enemy Spawner")]
public class EnemySpawnerSO : ScriptableObject
{
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
    public Wave[] waves = new Wave[10];
}
