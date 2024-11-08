using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public static MouseManager Instance = null;
    [HideInInspector] public Collider2D leftMouseClick;
    [HideInInspector] public Collider2D rightMouseClick;
    
    public LayerMask leftClickLayer;
    public LayerMask rightClickLayer;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0)){
            GetClick(ref leftMouseClick, leftClickLayer);
        }else if(Input.GetKeyDown(KeyCode.Mouse1)){
            GetClick(ref rightMouseClick, rightClickLayer);
        }
    }

    private void GetClick(ref Collider2D mouseClick, LayerMask clickLayer)
    {
        mouseClick = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition), clickLayer);
    }
}
