using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class BarrackTower : Tower
{
    [SerializeField] private AllyArea knightArea;
    [SerializeField] private float knightRespawnCD;
    [SerializeField] private Transform startPosChecker;
    [SerializeField] private List<Ally> knights;
    [SerializeField] private List<Ally> knightsToAdd;

    [Serializable]
    private struct Stats{
        public float flagRange;
        public float knightMoveSpeed;
        public float knightHealth;
        public float knightAttackRange;
        public float knightDamagePhysic;
        public float knightDamageMagic;
        public float knightAttackCD;
        public float knightPhysicRes;
        public float knightMagicRes;
        public float knightHealthRegen;
    }
    [SerializeField] private Stats[] stats;
    [SerializeField] private float flagRange;
    [SerializeField] private float knightMoveSpeed;
    [SerializeField] private float knightHealth;
    [SerializeField] private float knightAttackRange;
    [SerializeField] private float knightDamagePhysic;
    [SerializeField] private float knightDamageMagic;
    [SerializeField] private float knightAttackCD;
    [SerializeField] private float knightPhysicRes;
    [SerializeField] private float knightMagicRes;
    [SerializeField] private float knightHealthRegen;
    [SerializeField] private LayerMask pathLayer;

    protected override void Start()
    {
        base.Start();
        SetStartingPos();
        knightArea.respawnCD = knightRespawnCD;
    }

    private IEnumerator MoveFlag()
    {
        yield return null;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Mouse0));
        if(Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.position) <= flagRange &&
        MouseManager.Instance.leftMouseClick.gameObject.layer == LayerMask.NameToLayer("Path")){
            StartCoroutine(knightArea.MoveArea(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
        }
    }

    private void SetStartingPos()
    {
        Collider2D startingPos;
        do{
            startPosChecker.position = (Vector2)transform.position + Random.insideUnitCircle * flagRange;
            startingPos = Physics2D.OverlapPoint(startPosChecker.position, MouseManager.Instance.leftClickLayer);
        }while(startingPos.gameObject.layer != Mathf.Log(pathLayer.value, 2));
        StartCoroutine(knightArea.MoveArea(startPosChecker.position));
        Destroy(startPosChecker.gameObject);
    }

    public void StartMoveFlag()
    {
        StopAllCoroutines();
        StartCoroutine(MoveFlag());
    }

    public override void SellTower()
    {
        knightArea.RemoveFromFights();
        base.SellTower();
    }

    public override void UpgradeTower()
    {
        if(level >= 2){
            return;
        }

        base.UpgradeTower();

        flagRange = stats[level].flagRange;
        knightMoveSpeed = stats[level].knightMoveSpeed;
        knightHealth = stats[level].knightHealth;
        knightAttackRange = stats[level].knightAttackRange;
        knightDamagePhysic = stats[level].knightDamagePhysic;
        knightDamageMagic = stats[level].knightDamageMagic;
        knightAttackCD = stats[level].knightAttackCD;
        knightPhysicRes = stats[level].knightPhysicRes;
        knightMagicRes = stats[level].knightMagicRes;
        knightHealthRegen = stats[level].knightHealthRegen;

        knights.Add(knightsToAdd[0]);
        knights[knights.Count - 1].gameObject.SetActive(true);
        knightsToAdd.RemoveAt(0);

        foreach(Ally k in knights){
            k.Upgrade(knightMoveSpeed, knightHealth, knightAttackRange, knightDamagePhysic, knightDamageMagic,
            knightAttackCD, knightPhysicRes, knightMagicRes, knightHealthRegen);
        }
    }

    public override void ChangeStats(float changePercentage)
    {
        changePercentage /= 100;

        flagRange += stats[level].flagRange * changePercentage;
        knightArea.respawnCD += knightRespawnCD * changePercentage;
    }
}