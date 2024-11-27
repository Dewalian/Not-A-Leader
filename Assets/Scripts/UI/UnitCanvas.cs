using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitCanvas : MonoBehaviour
{
    [SerializeField] private float healthUITimer;
    private Unit unit;
    private Slider healthBar;
    private float originalHealth;
    private Coroutine hideHealthCoroutine;

    private void Awake()
    {
        unit = GetComponentInParent<Unit>();
        healthBar = GetComponentInChildren<Slider>();
    }

    private void OnEnable()
    {
        unit.OnHealthChanged.AddListener(() => UpdateHealthUI());
    }

    private void OnDisable()
    {
        unit.OnHealthChanged.RemoveListener(() => UpdateHealthUI());
    }

    private void Start()
    {
        healthBar.gameObject.SetActive(false);
        originalHealth = unit.health;
    }

    private IEnumerator HideHealthUI()
    {
        yield return new WaitForSeconds(healthUITimer);
        healthBar.gameObject.SetActive(false);
    }

    public void UpdateHealthUI()
    {
        // if(hideHealthCoroutine != null) StopCoroutine(hideHealthCoroutine);
        StopAllCoroutines();

        healthBar.gameObject.SetActive(true);
        healthBar.value = unit.health / originalHealth;

        StartCoroutine(HideHealthUI());
    }

    
}
