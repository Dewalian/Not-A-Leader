using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroArea : AllyArea
{
    private bool canTeleport;
    private bool isClickPortal;

    [SerializeField] private float teleportDuration;

    private void Start()
    {
        canTeleport = true;
    }

    protected override void Update()
    {
        base.Update();

        bool hasClicked = MouseManager.Instance.rightMouseClick != null;

        if(Input.GetKeyDown(KeyCode.Mouse1)){
            StopAllCoroutines();
            StartCoroutine(MoveArea(Camera.main.ScreenToWorldPoint(Input.mousePosition)));

            isClickPortal = hasClicked && MouseManager.Instance.rightMouseClick.CompareTag("Portal");
            if(isClickPortal){
                canTeleport = true;
            }
        }

        if(!isAreaMoving && isClickPortal && canTeleport){
            StartCoroutine(CheckPortal());
        }
    }

    private IEnumerator CheckPortal()
    {
        canTeleport = false;
        
        Portal portal = MouseManager.Instance.rightMouseClick.GetComponent<Portal>();
        yield return new WaitForSeconds(teleportDuration);
        area.transform.position = portal.Teleport();

        foreach(GameObject a in allies) {
            a.GetComponent<Ally>().TeleportToOriginalPos();
        }        
    }
}
