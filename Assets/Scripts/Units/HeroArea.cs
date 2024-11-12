using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroArea : AllyArea
{
    protected override void Update()
    {
        base.Update();

        if(Input.GetKeyDown(KeyCode.Mouse1)){
            StopAllCoroutines();
            StartCoroutine(MoveArea(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
        }


        if(!isAreaMoving){
            StartCoroutine(CheckPortal());
        }
    }

    private IEnumerator CheckPortal()
    {
        if(MouseManager.Instance.rightMouseClick == null){
            StopCoroutine(CheckPortal());
        }else if(MouseManager.Instance.rightMouseClick.CompareTag("Portal")){
            Portal portal = MouseManager.Instance.rightMouseClick.GetComponent<Portal>();
            yield return new WaitForSeconds(2f);
            area.transform.position = portal.Teleport();
            foreach(GameObject a in allies) {
                a.GetComponent<Ally>().TeleportToOriginalPos();
            }
        }
    }
}
