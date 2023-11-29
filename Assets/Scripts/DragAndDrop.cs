using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private Vector3 _dragOffset;
    private Camera _cam;
    private Transform _topBorder;
    private Transform _botBorder;
    private Transform _leftBorder;
    private Transform _rightBorder;

    private void Awake(){
        _cam = Camera.main;
        _topBorder = GameObject.Find("topBorder").transform;
        _botBorder = GameObject.Find("botBorder").transform;
        _leftBorder = GameObject.Find("leftBorder").transform;
        _rightBorder = GameObject.Find("rightBorder").transform;
    }

    private void OnMouseDown(){
        _dragOffset = transform.position - GetMousePos();
    }

    private void OnMouseDrag(){
        transform.position = GetMousePos() + _dragOffset;
        if(transform.position.x < _leftBorder.position.x || transform.position.x > _rightBorder.position.x){
            transform.position = new Vector3(0, 0, 0);
        }
        if(transform.position.y < _botBorder.position.y || transform.position.y > _topBorder.position.y){
            transform.position = new Vector3(0, 0, 0);
        }
    }

    private void Upodate(){
        if(transform.position.x < _leftBorder.position.x || transform.position.x > _rightBorder.position.x){
            transform.position = new Vector3(0, 0, 0);
        }
        if(transform.position.y < _botBorder.position.y || transform.position.y > _topBorder.position.y){
            transform.position = new Vector3(0, 0, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.CompareTag("DeathZone")){
            transform.position = new Vector3(0, 0, 0);
        }
    }

    private Vector3 GetMousePos(){
        var mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return mousePos;
    }
}
