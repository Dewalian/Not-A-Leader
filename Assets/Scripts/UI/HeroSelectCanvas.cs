using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSelectCanvas : MonoBehaviour
{
    [SerializeField] private GameObject border;
    [SerializeField] private GameObject[] buttons;

    public void SelectHero(int button)
    {
        border.transform.SetParent(buttons[button].transform);
        border.transform.localPosition = Vector2.zero;
    }
}
