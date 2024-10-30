using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance = null;
    public Transform[] wayPoint;
    public bool waveStart = true;

    
    void Awake()
    {
        instance = this;
    }
}
