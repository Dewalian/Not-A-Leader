using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MadGambler : MonoBehaviour
{
    private Joker joker;
    [SerializeField] private float towerBuffPercentage;
    [SerializeField] private float towerBuffDuration;
    [SerializeField] private float towerDebuffPercentage;
    [SerializeField] private float towerDebuffDuration;
    [SerializeField] private float radius;
    [SerializeField] private float jokerBuffDuration;
    [SerializeField] private float jokerResistanceBuff;
    [SerializeField] private float jokerDamageBuff;
    [SerializeField] private int jokerGoldBuff;
    [SerializeField] private float jokerDebuffDuration;
    [SerializeField] private float jokerResistanceDebuff;
    [SerializeField] private float jokerHurtDebuff;

    [SerializeField] private float trueTrueCD;
    [SerializeField] private float trueFalseCD;
    [SerializeField] private float falseTrueCD;
    [SerializeField] private float FalseFalseCD;
    [SerializeField] private float shuffleDuration;
    [SerializeField] private float showDuration;
    private bool canActivate = true;
    private bool canEffect = true;
    [SerializeField] private DeckSO deck;
    [SerializeField] private Transform card1Start;
    [SerializeField] private Transform card1End;
    [SerializeField] private Transform card2Start;
    [SerializeField] private Transform card2End;
    [SerializeField] private Color buffColor;
    [SerializeField] private Color debuffColor;
    [SerializeField] private GameObject blank;

    private void Start()
    {
        joker = GetComponentInParent<Joker>();
    }

    private void Update()
    {
        StartCoroutine(Activate());
    }

    private IEnumerator Activate()
    {
        if(Input.GetKeyDown(KeyCode.Q) && canActivate){
            joker.unitState = Unit.State.Skill;

            int face1 = Random.Range(1, 4);
            int value1 = Random.Range(1, 13);
            int face2 = Random.Range(1, 4);
            int value2 = Random.Range(1, 13);

            StartCoroutine(Shuffle(card1Start.position, card1End.position, face1, value1));
            yield return new WaitForSeconds(shuffleDuration / 6);
            StartCoroutine(Shuffle(card2Start.position, card2End.position, face2, value2));
            yield return new WaitForSeconds(shuffleDuration + showDuration);

            Collider2D[] towers = Physics2D.OverlapCircleAll(transform.position, radius, LayerMask.GetMask("Tower"));
            joker.unitState = Unit.State.Neutral;

            if(value1 == value2){
                foreach(Collider2D t in towers){
                    Tower tScript = t.GetComponent<Tower>();
                    StartCoroutine(tScript.ChangeStatsTimed(towerBuffPercentage, towerBuffDuration));
                    StartCoroutine(tScript.ChangeColor(towerBuffDuration, buffColor));
                }
            }else{
                foreach(Collider2D t in towers){
                    Tower tScript = t.GetComponent<Tower>();
                    StartCoroutine(tScript.ChangeStatsTimed(towerDebuffPercentage, towerDebuffDuration));
                    StartCoroutine(tScript.ChangeColor(towerDebuffDuration, debuffColor));
                }
            }

            if(face1 == face2 && value1 == value2){
                StartCoroutine(CoolDown(trueTrueCD));
                Debug.Log("11");
            }else if(face1 == face2 && value1 != value2){
                StartCoroutine(CoolDown(trueFalseCD));
                Debug.Log("10");
            }else if(face1 != face2 && value1 == value2){
                StartCoroutine(CoolDown(falseTrueCD));
                Debug.Log("01");
            }else if(face1 != face2 && value1 != value2){
                StartCoroutine(CoolDown(FalseFalseCD));
                Debug.Log("00");
            }

            if(face1 == face2){
                StartCoroutine(JokerBuff());
                yield return new WaitForSeconds(jokerBuffDuration);
            }else{
                StartCoroutine(JokerDebuff());
                yield return new WaitForSeconds(jokerDebuffDuration);
            }

            canEffect = false;
            yield return null;
            canEffect = true;
        }
    }

    private IEnumerator JokerBuff()
    {  
        StartCoroutine(joker.ChangeColor(jokerBuffDuration, buffColor));

        float physicResCopy = joker.physicRes;
        float magicResCopy = joker.magicRes;
        float attackDamageCopy = joker.attackDamagePhysic;

        joker.physicRes = jokerResistanceBuff;
        joker.magicRes = jokerResistanceBuff;
        joker.attackDamagePhysic = jokerDamageBuff;

        while(canEffect){
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, radius, LayerMask.GetMask("Enemy"));
            foreach(Collider2D e in enemies){

                Enemy eScript = e.GetComponent<Enemy>();
                if(eScript.unitState != Unit.State.Death){
                    yield return null;

                    if(eScript.unitState == Unit.State.Death){
                        LevelManager.Instance.AddGold(jokerGoldBuff);
                    }
                }
            }
            yield return null;
        }

        joker.physicRes = physicResCopy;
        joker.magicRes = magicResCopy;
        joker.attackDamagePhysic = attackDamageCopy;
        Debug.Log(physicResCopy);
    }

    private IEnumerator JokerDebuff()
    {
        StartCoroutine(joker.ChangeColor(jokerDebuffDuration, debuffColor));

        float physicResCopy = joker.physicRes;
        float magicResCopy = joker.magicRes;
        
        joker.physicRes = jokerResistanceDebuff;
        joker.magicRes = jokerResistanceDebuff;

        while(canEffect){
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, radius, LayerMask.GetMask("Enemy"));
            foreach(Collider2D e in enemies){

                Enemy eScript = e.GetComponent<Enemy>();
                if(eScript.unitState != Unit.State.Death){
                    yield return null;

                    if(eScript.unitState == Unit.State.Death){
                        joker.TakeDamage(jokerHurtDebuff, 0);
                    }
                }
            }
            yield return null;
        }

        joker.physicRes = physicResCopy;
        joker.magicRes = magicResCopy;
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
            yAdd += 0.1f;

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
}
