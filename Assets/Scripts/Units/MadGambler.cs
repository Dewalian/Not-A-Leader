using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MadGambler : MonoBehaviour
{
    private Joker joker;
    [SerializeField] private float towerBuffPercentage;
    [SerializeField] private float towerDebuffPercentage;
    [SerializeField] private float radius;
    [SerializeField] private float jokerResistanceBuff;
    [SerializeField] private float jokerDamageBuff;
    [SerializeField] private int jokerGoldBuff;
    [SerializeField] private float jokerResistanceDebuff;
    [SerializeField] private float jokerHurtDebuff;
    [SerializeField] private float buffDuration;
    [SerializeField] private float debuffDuration;

    [SerializeField] private float trueTrueCD;
    [SerializeField] private float trueFalseCD;
    [SerializeField] private float falseTrueCD;
    [SerializeField] private float FalseFalseCD;
    [SerializeField] private float shuffleDuration;
    [SerializeField] private float showDuration;
    [SerializeField] private float projectileSpeed;
    private bool canActivate;
    [HideInInspector] public bool canEffect;
    private bool isJokerBuff;
    private bool isTowerBuff;

    [SerializeField] private DeckSO deck;
    [SerializeField] private GameObject area;
    [SerializeField] private Transform card1Start;
    [SerializeField] private Transform card1End;
    [SerializeField] private Transform card2Start;
    [SerializeField] private Transform card2End;
    [SerializeField] private Color buffColor;
    [SerializeField] private Color debuffColor;
    [SerializeField] private GameObject statsUI;
    [SerializeField] private GameObject blank;
    [SerializeField] private GameObject buffProjectile;
    [SerializeField] private GameObject debuffProjectile;
    [HideInInspector] public UnityEvent<bool, bool> OnUpdateMadGambler;

    private void Start()
    {
        joker = GetComponentInParent<Joker>();

        canActivate = true;
        canEffect = false;

        statsUI.SetActive(false);
        area.SetActive(false);
    }

    private void Update()
    {
        StartCoroutine(Activate());
    }

    private IEnumerator Activate()
    {
        if(Input.GetKeyDown(KeyCode.Q) && canActivate){

            int face1 = Random.Range(1, 4);
            int value1 = Random.Range(0, 12);
            int face2 = Random.Range(1, 4);
            int value2 = Random.Range(0, 12);

            bool sameFace = face1 == face2;
            bool sameValue = value1 == value2;

            joker.unitState = Unit.State.Skill;

            StartCoroutine(Shuffle(card1Start.position, card1End.position, face1, value1));
            yield return new WaitForSeconds(shuffleDuration / 6);
            StartCoroutine(Shuffle(card2Start.position, card2End.position, face2, value2));
            yield return new WaitForSeconds(shuffleDuration);

            OnUpdateMadGambler?.Invoke(sameFace, sameValue);
            statsUI.SetActive(true);

            Collider2D[] towers = Physics2D.OverlapCircleAll(transform.position, radius, LayerMask.GetMask("Tower"));
            joker.unitState = Unit.State.Neutral;

            if(value1 == value2){
                isTowerBuff = false;
            }else{
                isTowerBuff = true;
            }

            foreach(Collider2D t in towers){
                Tower tScript = t.GetComponent<Tower>();
                StartCoroutine(TowerBuffDebuff(tScript));
            }

            yield return new WaitForSeconds(showDuration);
            statsUI.SetActive(false);

            if(sameFace){
                isJokerBuff = true;
                StartCoroutine(JokerDebuff());
            }else{
                isJokerBuff = false;
                StartCoroutine(JokerBuff());
            }

            if(sameFace && sameValue){
                StartCoroutine(CoolDown(trueTrueCD));
            }else if(sameFace && !sameValue){
                StartCoroutine(CoolDown(trueFalseCD));
            }else if(!sameFace && sameValue){
                StartCoroutine(CoolDown(falseTrueCD));
            }else if(!sameFace && !sameValue){
                StartCoroutine(CoolDown(FalseFalseCD));
            }

        }
    }
    
    private IEnumerator JokerBuff()
    {  
        float physicResCopy = joker.physicRes;
        float magicResCopy = joker.magicRes;
        float attackDamageCopy = joker.attackDamagePhysic;

        area.SetActive(true);

        StartCoroutine(joker.ChangeColor(buffDuration, buffColor));

        joker.physicRes = jokerResistanceBuff;
        joker.magicRes = jokerResistanceBuff;
        joker.attackDamagePhysic = jokerDamageBuff;

        yield return StartCoroutine(EffectDuration(buffDuration));

        area.SetActive(false);

        joker.physicRes = physicResCopy;
        joker.magicRes = magicResCopy;
        joker.attackDamagePhysic = attackDamageCopy;
    }

    private IEnumerator JokerDebuff()
    {
        float physicResCopy = joker.physicRes;
        float magicResCopy = joker.magicRes;

        area.SetActive(true);

        StartCoroutine(joker.ChangeColor(debuffDuration, debuffColor));
        
        joker.physicRes = jokerResistanceDebuff;
        joker.magicRes = jokerResistanceDebuff;

        yield return StartCoroutine(EffectDuration(debuffDuration));

        area.SetActive(false);

        joker.physicRes = physicResCopy;
        joker.magicRes = magicResCopy;
    }

    private IEnumerator TowerBuffDebuff(Tower tower)
    {
        yield return new WaitForSeconds(showDuration / 1.5f);

        float timePassed = 0;
        GameObject projectile = Instantiate(buffProjectile, transform.position, Quaternion.identity);

        while(timePassed < showDuration / 3){
            timePassed += Time.deltaTime;
            float durationNorm = timePassed / (showDuration / 3);

            projectile.transform.position = Vector2.Lerp(joker.transform.position, tower.transform.position, durationNorm);

            yield return null;
        }
        
        if(isTowerBuff){
            StartCoroutine(tower.ChangeStatsTimed(towerBuffPercentage, buffDuration));
            StartCoroutine(tower.ChangeColor(buffDuration, buffColor));
        }else{
            tower.DestroyTower();
        }
    }   

    private IEnumerator Shuffle(Vector2 start, Vector2 end, int face, int value){
        float yAdd = 0;
        float oneCardDuration = shuffleDuration / 6;

        for(int i=0; i<5; i++){
            GameObject card = Instantiate(blank, start, Quaternion.identity);
            SpriteRenderer cardRenderer = card.GetComponent<SpriteRenderer>();

            if(i == 4){
                cardRenderer.sprite = deck.faces[face].values[value];
            }else{
                cardRenderer.sprite = deck.faces[0].values[0];
            }

            cardRenderer.sortingOrder = i;

            float timePassed = 0;
            yAdd += 0.05f;

            Vector2 endY = new Vector2(end.x, end.y + yAdd);

            while(timePassed < oneCardDuration){
                timePassed += Time.deltaTime;

                float durationNorm = timePassed / oneCardDuration;

                card.transform.position = Vector2.Lerp(start, endY, durationNorm);
                yield return null;
            }

            Destroy(card, showDuration);
        }
    }

    private IEnumerator CoolDown(float coolDown)
    {
        canActivate = false;
        yield return new WaitForSeconds(coolDown);
        canActivate = true;
    }

    private IEnumerator EffectDuration(float duration)
    {
        canEffect = true;
        yield return new WaitForSeconds(duration);
        canEffect = false;
    }

    public IEnumerator JokerEffect(Vector2 startPos)
    {
        GameObject projectile;

        if(isJokerBuff){
            projectile = Instantiate(buffProjectile, startPos, Quaternion.identity);
        }else{
            projectile = Instantiate(debuffProjectile, startPos, Quaternion.identity);
        }

        while(Vector2.Distance(projectile.transform.position, joker.transform.position) > 0.1f){
            projectile.transform.position = Vector2.MoveTowards(projectile.transform.position, joker.transform.position, projectileSpeed * Time.deltaTime);
            yield return null;
        }

        Destroy(projectile);

        if(isJokerBuff){
            LevelManager.Instance.AddGold(jokerGoldBuff);
        }else{
            joker.TakeDamage(jokerHurtDebuff, 0);
        }
    }
}
