using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAreaVisual : MonoBehaviour
{
    private Tower tower;

    private void Awake()
    {
        tower = GetComponentInParent<Tower>();
    }

    private void OnEnable()
    {
        tower.OnUpgrade.AddListener(() => IncreaseVisual());
    }

    private void IncreaseVisual()
    {
        float range = tower.GetRange();
        transform.localScale = new Vector2(range, range);
    }
}
