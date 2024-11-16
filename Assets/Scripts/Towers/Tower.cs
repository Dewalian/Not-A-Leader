using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    [SerializeField] private GameObject plot;
    public string towerName;
    public int level = 0;
    public int[] costs = new int[3];

    public virtual void SellTower()
    {
        //dapet duit
        Instantiate(plot, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public virtual void UpgradeTower()
    {
        if(level < 2 && LevelManager.Instance.gold >= costs[level+1]){
            level++;
            LevelManager.Instance.gold -= costs[level];
        }
        Debug.Log(level);
    }
}
