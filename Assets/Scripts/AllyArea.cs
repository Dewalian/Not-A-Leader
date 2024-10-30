using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
    protected virtual void Update()
    {
        DetectEnemy();

        foreach(GameObject a in allies){
            if(a.GetComponent<Ally>().isAlive == false){
                StartCoroutine(Respawn(a));
            }

            if(isAreaMoving){
                a.GetComponent<Ally>().RemoveFromFight();
            }
        }
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
                Debug.Log(a);
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

    protected IEnumerator Respawn(GameObject ally)
    {
        ally.GetComponent<Ally>().isAlive = true;
        ally.transform.position = respawnPos.position;
        yield return new WaitForSeconds(respawnCD);
        ally.SetActive(true);
    }

    protected virtual IEnumerator MoveArea()
    {
        yield return null;
    }
}
