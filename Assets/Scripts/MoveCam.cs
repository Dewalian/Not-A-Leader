using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCam : MonoBehaviour
{
    [SerializeField] private float speed;

    private void Update()
    {
        MoveCamByMousePos();
    }

    private void MoveCamByMousePos()
    {
        Vector3 mouseToCam = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10);
        Vector3 mousePosViewport = Camera.main.ScreenToViewportPoint(mouseToCam);
        Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(mouseToCam);
        


        if(mousePosViewport.x <= 0.01f || mousePosViewport.x >= 0.99f || mousePosViewport.y <= 0.01f || mousePosViewport.y >= 0.99f){
            transform.position = Vector3.MoveTowards(transform.position, mousePosWorld, speed * Time.deltaTime);
        }
    }
}
