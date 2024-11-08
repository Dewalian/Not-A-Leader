using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotCanvas : MonoBehaviour
{
    [SerializeField] private GameObject plotUI;

    private void Update()
    {
        HideIfClickOutside();
    }

    private void HideIfClickOutside()
    {
        if((Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1)) &&
        !RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), Input.mousePosition, Camera.main)){    
            plotUI.SetActive(false);
        }
    }

    public void OnPlotClick()
    {
        if(!plotUI.activeSelf){
            plotUI.SetActive(true);
        }else{
            plotUI.SetActive(false);
        }
    }
}
