using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance = null;
    public int gold;
    public int life;

    private void Awake()
    {
        if(Instance == null){
            Instance = this;
        }
    }


}
