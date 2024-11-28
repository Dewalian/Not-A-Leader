using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour
{
    public GameObject[] towers;

    public void BuildTower(int tower)
    {
        Tower towerScript = towers[tower].GetComponent<Tower>();

        if(LevelManager.Instance.gold >= towerScript.costs[0]){
            Instantiate(towers[tower], transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
