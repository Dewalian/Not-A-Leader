using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleCanvas : MonoBehaviour
{
    [SerializeField] private GameObject toggleUI;

    private void Update()
    {
        HideIfClickOutside();
    }

    private void HideIfClickOutside()
    {
        if((Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1)) &&
        !RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), Input.mousePosition, Camera.main)){    
            toggleUI.SetActive(false);
        }
    }

    public void OnToggleClick()
    {
        if(!toggleUI.activeSelf){
            toggleUI.SetActive(true);
        }else{
            toggleUI.SetActive(false);
        }
    }
}
