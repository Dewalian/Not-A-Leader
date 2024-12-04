using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCam : MonoBehaviour
{
    private Camera mainCam;
    [SerializeField] private float speed;
    [SerializeField] private float upperBound;
    [SerializeField] private float lowerBound;
    [SerializeField] private float leftBound;
    [SerializeField] private float rightBound;
    [SerializeField] private GameObject hero;

    private void Awake()
    {
        mainCam = GetComponent<Camera>();
    }

    private void Update()
    {
        MoveCamByMousePos();
        FocusToHero();
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

            bool boundX = moveCam.x <= leftBound || moveCam.x >= rightBound;
            bool boundY = moveCam.y <= lowerBound || moveCam.y >= upperBound;

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

    private void FocusToHero()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            Vector2 heroPos = hero.transform.position;
            float x = 0;
            float y = 0;
            
            if(heroPos.x > rightBound){
                x -= heroPos.x - rightBound;
            }else if(heroPos.x < leftBound){
                x -= heroPos.x - leftBound;
            }

            if(heroPos.y > upperBound){
                y -= heroPos.y - upperBound;
            }else if(heroPos.y < lowerBound){
                y -= heroPos.y - lowerBound;
            }

            transform.position = new Vector3(heroPos.x + x, heroPos.y + y, -10);
        }
    }
}
