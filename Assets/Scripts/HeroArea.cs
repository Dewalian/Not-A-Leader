using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroArea : AllyArea
{
    protected override void Update()
    {
        base.Update();

        if(Input.GetKeyDown(KeyCode.Mouse1)){
            StopCoroutine(MoveArea());
            isAreaMoved = true;
        }
        StartCoroutine(MoveArea());

        Debug.Log(isAreaMoving);
    }

    protected override IEnumerator MoveArea()
    {
        if(Input.GetKeyDown(KeyCode.Mouse1) && isAreaMoved){
            isAreaMoved = false;
            isAreaMoving = true;
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePos - (Vector2)area.transform.position).normalized;
            area.transform.up = direction;
            area.transform.position = mousePos;
            yield return new WaitUntil(() => Vector2.Distance(allies[0].transform.position, mousePos) < 0.1f);
            isAreaMoving = false;
        }
    }
}
