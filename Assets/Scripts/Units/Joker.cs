using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joker : Ally
{
    [SerializeField] private GameObject gamblerRoulette;
    [SerializeField] private float gamblerRouletteCD;
    [SerializeField] private float gamblerRoulleteBuffPercentage;
    [SerializeField] private float gamblerRoulleteDebuffPercentage;
    [SerializeField] private float gamblerRoulleteDuration;
    private bool canGamblerRoulette = true;
    private List<GameObject> towers = new List<GameObject>();
    private List<string> towerNames = new List<string>();

    protected override void Update()
    {
        base.Update();
        GamblerRoulette();
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
