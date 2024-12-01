using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCam : MonoBehaviour
{
    private Camera mainCam;
    [SerializeField] private float speed;

    private void Awake()
    {
        mainCam = GetComponent<Camera>();
    }

    private void Update()
    {
        MoveCamByMousePos();
    }

    private void MoveCamByMousePos()
    {
        if(!mainCam.enabled) return;

        Vector3 mouseToCam = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10);
        Vector3 mousePosViewport = Camera.main.ScreenToViewportPoint(mouseToCam);
        Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(mouseToCam);

        bool moveX = mousePosViewport.x <= 0.01f || mousePosViewport.x >= 0.99f;
        bool moveY = mousePosViewport.y <= 0.01f || mousePosViewport.y >= 0.99f;
        
        if(moveX || moveY){
            Vector3 moveCam = Vector3.MoveTowards(transform.position, mousePosWorld, speed * Time.deltaTime);

            bool boundX = moveCam.x <= -3 || moveCam.x >= 15;
            bool boundY = moveCam.y <= -2 || moveCam.y >= 8;

            if(boundX && boundY){
                return;
            }else if(boundX){
                transform.position = new Vector3(transform.position.x, moveCam.y, moveCam.z);
            }else if(boundY){
                transform.position = new Vector3(moveCam.x, transform.position.y, moveCam.z);
            }else{
                transform.position = moveCam;
            }
        }
    }
}
