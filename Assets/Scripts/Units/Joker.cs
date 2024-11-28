using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Joker : Ally
{
    [SerializeField] private float gamblerRouletteCD;
    [SerializeField] private float gamblerRouletteBuffPercentage;
    [SerializeField] private float gamblerRouletteDebuffPercentage;
    [SerializeField] private float gamblerRouletteDuration;
    [SerializeField] private float gamberRouletteActiveDuration;
    public float gamblerRoulleteRadius;
    [Serializable]
    private struct HealthRegenAccel{
        public float regenAccelCount;
        public float regenAccelValue;
    }
    [SerializeField] private List<HealthRegenAccel> healthRegenAccels;
    [SerializeField] private GameObject gamblerRoulette;
    [SerializeField] private GameObject gamblerRouletteUI;
    [SerializeField] private GameObject blank;
    private bool canGamblerRoulette = true;
    private bool canGamblerRouletteActive = true;
    [HideInInspector] public List<GameObject> towers = new List<GameObject>();
    [HideInInspector] public List<string> towerNames = new List<string>();
    public List<Sprite> towerSymbols = new List<Sprite>();
    [HideInInspector] public List<string> uniqueTowerNames = new List<string>();
    [HideInInspector] public UnityEvent OnUpdateStats;

    protected override void Start()
    {
        base.Start();
        LevelManager.Instance.UpdateHeroHealth(health);
        gamblerRoulette.transform.localScale = new Vector2(gamblerRoulleteRadius, gamblerRoulleteRadius);
    }

    protected override void Update()
    {
        base.Update();
        CheckGold();
    }

    protected override IEnumerator HealthRegen()
    {
        isStartRegen = true;
        yield return new WaitForSeconds(healthRegenDelay);

        float seconds = 0;
        int index = -1;
        while(health < healthCopy){

            if(index == -1){
                health += Mathf.Min(healthRegen, healthCopy - health);
            }else{
                health += Mathf.Min(healthRegenAccels[index].regenAccelValue, healthCopy - health);
            }
            OnHealthChanged?.Invoke();
            LevelManager.Instance.UpdateHeroHealth(health);

            yield return new WaitForSeconds(1f);
            seconds++;

            if(index+1 < healthRegenAccels.Count && seconds == healthRegenAccels[index+1].regenAccelCount){
                index++;
            }
        }
    }

    public override void TakeDamage(float attackDamagePhysic, float attackDamageMagic)
    {
        base.TakeDamage(attackDamagePhysic, attackDamageMagic);
        LevelManager.Instance.UpdateHeroHealth(health);
    }

    private void CheckGold()
    {
        if(LevelManager.Instance.gold >= 1000 && canGamblerRoulette){
            gamblerRouletteUI.SetActive(true);
            gamblerRoulette.SetActive(true);
            OnUpdateStats?.Invoke();
        }else{
            gamblerRouletteUI.SetActive(false);
            gamblerRoulette.SetActive(false);
        }
    }

    private IEnumerator GamblerRoulette()
    {
        if(LevelManager.Instance.gold >= 1000 && towers.Count >= 3 && uniqueTowerNames.Count >= 3 && canGamblerRoulette){ //Ini nanti diganti
            gamblerRouletteUI.SetActive(false);
            canGamblerRouletteActive = true;
            unitState = State.Skill;

            foreach(GameObject t in towers){
                StartCoroutine(GamblerRouletteActive(t));
            }

            yield return new WaitForSeconds(gamberRouletteActiveDuration);
            unitState = State.Neutral;
            canGamblerRouletteActive = false;

            foreach(GameObject t in towers){
                Tower tScript = t.GetComponent<Tower>();
                int randomIndex = Random.Range(0, towerNames.Count);
                string randomTowerName = towerNames[randomIndex];
                Sprite randomTowerSymbol = towerSymbols[randomIndex];
                towerNames.Remove(randomTowerName);
                towerSymbols.Remove(randomTowerSymbol);

                if(tScript.towerName == randomTowerName){
                    StartCoroutine(tScript.ChangeStatsTimed(gamblerRouletteBuffPercentage, gamblerRouletteDuration));
                }else{
                    StartCoroutine(tScript.ChangeStatsTimed(gamblerRouletteDebuffPercentage, gamblerRouletteDuration));
                }

                Vector2 symbolPos = new Vector2(t.transform.position.x, t.transform.position.y + 1);
                GameObject blankObj = Instantiate(blank, symbolPos, Quaternion.identity);
                SpriteRenderer blankSymbol = blankObj.GetComponent<SpriteRenderer>();
                blankSymbol.sprite = randomTowerSymbol;
                Destroy(blankObj, gamblerRouletteDuration);
            }

            towers.Clear();
            towerNames.Clear();
            towerSymbols.Clear();
            uniqueTowerNames.Clear();
            
            StartCoroutine(GamblerRouletteCD());
        }
    }

    private IEnumerator GamblerRouletteActive(GameObject tower)
    {
        Vector2 symbolPos = new Vector2(tower.transform.position.x, tower.transform.position.y + 1);
        GameObject blankObj = Instantiate(blank, symbolPos, Quaternion.identity);
        SpriteRenderer blankSymbol = blankObj.GetComponent<SpriteRenderer>();

        while(canGamblerRouletteActive){
            for(int i=0; i<towerSymbols.Count; i++){
                blankSymbol.sprite = towerSymbols[i];
                yield return new WaitForSeconds(0.15f);
            }
        }

        Destroy(blankObj);
    }

    private IEnumerator GamblerRouletteCD()
    {
        canGamblerRoulette = false;
        gamblerRoulette.SetActive(false);
        yield return new WaitForSeconds(gamblerRouletteCD);
        canGamblerRoulette = true;
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

    public void OnGamblerRouletteEnter(GameObject tower, string towerName, Sprite towerSymbol)
    {
        towers.Add(tower);
        towerNames.Add(towerName);
        towerSymbols.Add(towerSymbol);

        AddUniqueTower(towerName);

        OnUpdateStats?.Invoke();
        StartCoroutine(GamblerRoulette());
    }

    public void OnGamblerRouletteExit(GameObject tower, string towerName, Sprite towerSymbol)
    {
        towers.Remove(tower);
        towerNames.Remove(towerName);
        towerSymbols.Remove(towerSymbol);
        uniqueTowerNames.Remove(towerName);

        OnUpdateStats?.Invoke();
        StartCoroutine(GamblerRoulette());
    }
}
