using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Tower : MonoBehaviour
{
    [SerializeField] private GameObject plot;
    [HideInInspector] public int level;
    public string towerName;
    public Sprite towerSymbol;
    public int[] costs = new int[3];
    public UnityEvent OnUpgrade;

    protected virtual void Start()
    {
        level = -1;
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
        level++;
        LevelManager.Instance.AddGold(-costs[level]);
    }
    public IEnumerator ChangeStatsTimed(float changePercentage, float time)
    {
        ChangeStats(changePercentage);
        yield return new WaitForSeconds(time);
        ChangeStats(-changePercentage);
    }

    public abstract void ChangeStats(float changePercentage);
    
    public abstract float GetRange();
}
