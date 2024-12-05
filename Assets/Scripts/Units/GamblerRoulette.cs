using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class GamblerRoulette : MonoBehaviour
{
    private Joker joker;
    public float goldRequired;
    public float towerCountRequired;
    public float uniqueTowerCountRequired;
    [SerializeField] private int cost;
    [SerializeField] private float coolDown;
    [SerializeField] private float buffPercentage;
    [SerializeField] private float debuffPercentage;
    [SerializeField] private float duration;
    [SerializeField] private float spinDuration;
    [SerializeField] private float radius;
    [SerializeField] private GameObject statsUI;
    [SerializeField] private GameObject area;
    [SerializeField] private GameObject blank;
    private bool canActivate = true;
    private bool canSpin = true;
    public List<GameObject> towers = new List<GameObject>();
    [HideInInspector] public List<string> towerNames = new List<string>();
    public List<Sprite> towerSymbols = new List<Sprite>();
    [HideInInspector] public List<string> uniqueTowerNames = new List<string>();
    [HideInInspector] public UnityEvent OnUpdateGamblerRoulette;

    private void Start()
    {
        joker = GetComponentInParent<Joker>();

        transform.localScale = new Vector2(radius, radius);
    }

    private void Update()
    {
        CheckGold();
    }

    private void CheckGold()
    {
        if(LevelManager.Instance.gold >= 1000 && canActivate){
            statsUI.SetActive(true);
            area.SetActive(true);
            OnUpdateGamblerRoulette?.Invoke();
        }else{
            statsUI.SetActive(false);
            area.SetActive(false);
        }
    }

    private IEnumerator Activate()
    {
        bool isGoldPass = LevelManager.Instance.gold >= goldRequired;
        bool isTowerCountPass = towers.Count >= towerCountRequired;
        bool isUniqueTowerCountPass = uniqueTowerNames.Count >= uniqueTowerCountRequired;

        if(isGoldPass && isTowerCountPass && isUniqueTowerCountPass && canActivate && joker.unitState != Unit.State.Skill){
            LevelManager.Instance.AddGold(cost);

            statsUI.SetActive(false);
            canSpin = true;
            joker.unitState = Unit.State.Skill;

            foreach(GameObject t in towers){
                StartCoroutine(Spin(t));
            }

            yield return new WaitForSeconds(spinDuration);
            joker.unitState = Unit.State.Neutral;
            canSpin = false;

            foreach(GameObject t in towers){
                Tower tScript = t.GetComponent<Tower>();
                int randomIndex = Random.Range(0, towerNames.Count);
                string randomTowerName = towerNames[randomIndex];
                Sprite randomTowerSymbol = towerSymbols[randomIndex];
                towerNames.Remove(randomTowerName);
                towerSymbols.Remove(randomTowerSymbol);

                if(tScript.towerName == randomTowerName){
                    StartCoroutine(tScript.ChangeStatsTimed(buffPercentage, duration));
                }else{
                    StartCoroutine(tScript.ChangeStatsTimed(debuffPercentage, duration));
                }

                Vector2 symbolPos = new Vector2(t.transform.position.x, t.transform.position.y + 1);
                GameObject blankObj = Instantiate(blank, symbolPos, Quaternion.identity);
                SpriteRenderer blankSymbol = blankObj.GetComponent<SpriteRenderer>();
                blankSymbol.sprite = randomTowerSymbol;
                Destroy(blankObj, duration);
            }

            towers.Clear();
            towerNames.Clear();
            towerSymbols.Clear();
            uniqueTowerNames.Clear();
            area.SetActive(false);
            
            StartCoroutine(CoolDown());
        }
    }

    private IEnumerator Spin(GameObject tower)
    {
        Vector2 symbolPos = new Vector2(tower.transform.position.x, tower.transform.position.y + 1);
        GameObject blankObj = Instantiate(blank, symbolPos, Quaternion.identity);
        SpriteRenderer blankSymbol = blankObj.GetComponent<SpriteRenderer>();

        while(canSpin){
            for(int i=0; i<towerSymbols.Count; i++){
                blankSymbol.sprite = towerSymbols[i];
                yield return new WaitForSeconds(0.15f);
            }
        }

        Destroy(blankObj);
    }

    private IEnumerator CoolDown()
    {
        canActivate = false;
        yield return new WaitForSeconds(coolDown);
        canActivate = true;
    }

    private void AddUniqueTower(string towerName)
    {
        foreach(string u in uniqueTowerNames){
            if(u == towerName){
                return;
            }
        }
        uniqueTowerNames.Add(towerName);
    }

    public void OnAreaEnter(GameObject tower, string towerName, Sprite towerSymbol)
    {
        towers.Add(tower);
        towerNames.Add(towerName);
        towerSymbols.Add(towerSymbol);

        AddUniqueTower(towerName);

        OnUpdateGamblerRoulette?.Invoke();
        StartCoroutine(Activate());
    }

    public void OnAreaExit(GameObject tower, string towerName, Sprite towerSymbol)
    {
        towers.Remove(tower);
        towerNames.Remove(towerName);
        towerSymbols.Remove(towerSymbol);
        uniqueTowerNames.Remove(towerName);

        OnUpdateGamblerRoulette?.Invoke();
        StartCoroutine(Activate());
    }
}
