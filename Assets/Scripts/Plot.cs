using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour
{
    [SerializeField] private GameObject[] towers;

    public void BuildArcherTower()
    {
        BuildTower(0);
    }

    public void BuildMageTower()
    {
        BuildTower(1);
    }

    public void BuildBarrackTower()
    {
        BuildTower(2);
    }

    public void BuildMortarTower()
    {
        BuildTower(3);
    }

    private void BuildTower(int tower)
    {
        Instantiate(towers[tower], transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
