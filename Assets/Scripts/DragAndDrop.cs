using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    Vector3 dragOffset;
    Camera cam;
    Transform topBorder;
    Transform botBorder;
    Transform leftBorder;
    Transform rightBorder;

    void Awake(){
        cam = Camera.main;
        topBorder = GameObject.Find("topBorder").transform;
        botBorder = GameObject.Find("botBorder").transform;
        leftBorder = GameObject.Find("leftBorder").transform;
        rightBorder = GameObject.Find("rightBorder").transform;
    }

    void OnMouseDown(){
        dragOffset = transform.position - GetMousePos();
    }

    void OnMouseDrag(){
        transform.position = GetMousePos() + dragOffset;
        if(transform.position.x < leftBorder.position.x || transform.position.x > rightBorder.position.x){
            transform.position = new Vector3(0, 0, 0);
        }
        if(transform.position.y < botBorder.position.y || transform.position.y > topBorder.position.y){
            transform.position = new Vector3(0, 0, 0);
        }
    }
    void Upodate(){
        if(transform.position.x < leftBorder.position.x || transform.position.x > rightBorder.position.x){
            transform.position = new Vector3(0, 0, 0);
        }
        if(transform.position.y < botBorder.position.y || transform.position.y > topBorder.position.y){
            transform.position = new Vector3(0, 0, 0);
        }
    }
    void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.tag == "DeathZone"){
            transform.position = new Vector3(0, 0, 0);
        }
    }

    Vector3 GetMousePos(){
        var mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return mousePos;
    }
}
