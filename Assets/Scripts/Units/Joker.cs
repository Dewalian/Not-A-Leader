using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Joker : Ally
{
    [SerializeField] private GameObject gamblerRoulette;
    [SerializeField] private float gamblerRouletteCD;
    [SerializeField] private float gamblerRoulleteBuffPercentage;
    [SerializeField] private float gamblerRoulleteDebuffPercentage;
    [SerializeField] private float gamblerRoulleteDuration;
    [Serializable]
    private struct HealthRegenAccel{
        public float regenAccelCount;
        public float regenAccelValue;
    }
    [SerializeField] private List<HealthRegenAccel> healthRegenAccels;
    private bool canGamblerRoulette = true;
    private List<GameObject> towers = new List<GameObject>();
    private List<string> towerNames = new List<string>();

    protected override void Start()
    {
        base.Start();
        LevelManager.Instance.UpdateHeroHealth(health);
    }

    protected override void Update()
    {
        base.Update();
        GamblerRoulette();
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

    private void GamblerRoulette()
    {
        if(Input.GetKeyDown(KeyCode.Q) && towers.Count >= 3 && canGamblerRoulette){ //Ini nanti diganti
            foreach(GameObject t in towers){
                Tower tScript = t.GetComponent<Tower>();
                string randomTowerName = towerNames[Random.Range(0, towerNames.Count)];
                string chosenTowerName = randomTowerName;
                towerNames.Remove(randomTowerName);

                if(tScript.towerName == chosenTowerName){
                    StartCoroutine(tScript.ChangeStatsTimed(gamblerRoulleteBuffPercentage, gamblerRoulleteDuration));
                }else{
                    StartCoroutine(tScript.ChangeStatsTimed(gamblerRoulleteDebuffPercentage, gamblerRoulleteDuration));
                }
            }

            towers.Clear();
            towerNames.Clear();
            StartCoroutine(GamblerRouletteCD());
        }
    }

    private IEnumerator GamblerRouletteCD()
    {
        canGamblerRoulette = false;
        gamblerRoulette.SetActive(false);
        yield return new WaitForSeconds(gamblerRouletteCD);
        canGamblerRoulette = true;
        gamblerRoulette.SetActive(true);
    }

    public void OnGamblerRouletteEnter(GameObject tower, string towerName)
    {
        towers.Add(tower);
        towerNames.Add(towerName);
    }

    public void OnGamblerRouletteExit(GameObject tower, string towerName)
    {
        towers.Remove(tower);
        towerNames.Remove(towerName);
    }
}
