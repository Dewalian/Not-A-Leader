using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AllyArea : MonoBehaviour
{
    [SerializeField] protected GameObject[] allies;
    [SerializeField] protected GameObject area;
    [SerializeField] protected float aggroArea;
    [SerializeField] protected LayerMask enemyLayer;
    [SerializeField] protected Transform respawnPos;
    [SerializeField] protected float respawnCD;
    protected bool isAreaMoving = false;
    protected bool isAreaMoved = false;
    public UnityEvent OnMoveArea;
    
    protected virtual void Update()
    {
        DetectEnemy();
    }

    protected void DetectEnemy()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(area.transform.position, aggroArea, enemyLayer);

        if(enemies.Length > 0 && !isAreaMoving){
            SetAllyTarget(enemies);
        }
    }

    protected void SetAllyTarget(Collider2D[] enemies)
    {
        foreach(GameObject a in allies){
            Ally aScript = a.GetComponent<Ally>();

            if(a.activeSelf == true){
                foreach(Collider2D e in enemies){
                    if(e.GetComponent<Enemy>().enemyState == Unit.State.Neutral && aScript.isDuel == false){
                        aScript.SetTarget(e.gameObject);
                        aScript.isDuel = true;
                        return;
                    }
                }

                if(aScript.allyState == Unit.State.Neutral){
                    aScript.SetTarget(enemies[0].gameObject);
                }
            }
        }
    }

    private IEnumerator Respawn(GameObject ally)
    {
        ally.transform.position = respawnPos.position;
        yield return new WaitForSeconds(respawnCD);
        ally.SetActive(true);
    }

    public IEnumerator MoveArea(Vector2 targetPos)
    {
        isAreaMoved = true;
        OnMoveArea?.Invoke();
        if(isAreaMoved){
            isAreaMoved = false;
            isAreaMoving = true;
            Vector2 direction = (targetPos - (Vector2)area.transform.position).normalized;
            area.transform.up = direction;
            area.transform.position = targetPos;

            foreach(GameObject a in allies){
                if(a.activeSelf == true){
                    yield return new WaitUntil(() => Vector2.Distance(a.transform.position, a.GetComponent<Ally>().originalPos.position) < 0.1f);
                    break;
                }
            }

            isAreaMoving = false;
        }
    }

    public void StartRespawn(GameObject ally){
        StartCoroutine(Respawn(ally));
    }

    public void RemoveFromFights()
    {
        OnMoveArea?.Invoke();
    }

    // public void UpgradeAlly(float moveSpeed, float health, float attackRange, float damagePhysic, float damageMagic,
    // float attackCD, float physicRes, float magicRes)
    // {
    //     foreach(GameObject a in allies){
    //         Ally ally = a.GetComponent<Ally>();
            
    //     }
    // }
}