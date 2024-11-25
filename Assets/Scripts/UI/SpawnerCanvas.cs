using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerCanvas : MonoBehaviour
{
    private EnemySpawner enemySpawner;
    [SerializeField] private GameObject startWaveButton;

    private void Awake()
    {
        enemySpawner = GetComponentInParent<EnemySpawner>();
    }
}
