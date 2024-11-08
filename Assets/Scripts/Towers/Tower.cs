using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    [SerializeField] private GameObject plot;
    protected int level = -1;

    public virtual void SellTower()
    {
        //dapet duit
        Instantiate(plot, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public virtual void UpgradeTower()
    {
        if(level < 2){
            level++;
        }
    }
}
