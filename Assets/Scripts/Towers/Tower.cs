using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    [SerializeField] private GameObject plot;
    public string towerName;
    public int level = -1;
    public int[] costs = new int[3];

    protected virtual void Start()
    {
        UpgradeTower();
    }

    public void DestroyTower()
    {
        Instantiate(plot, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public virtual void SellTower()
    {
        //dapet duit
        DestroyTower();
    }

    public virtual void UpgradeTower()
    {
        if(level < 2 && LevelManager.Instance.gold >= costs[level+1]){
            level++;
            LevelManager.Instance.gold -= costs[level];
        }
    }

    public virtual void ChangeStats(float changePercentage)
    {
        return;
    }

    public IEnumerator ChangeStatsTimed(float changePercentage, float time)
    {
        ChangeStats(changePercentage);
        yield return new WaitForSeconds(time);
        ChangeStats(-changePercentage);
    }
}
