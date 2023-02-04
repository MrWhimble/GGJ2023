using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float slowSpeed;

    private Camera _camera;

    private Vector2 _topLeft;
    private Vector2 _bottomRight;

    private void Awake()
    {
        _camera = Camera.main;
        _topLeft = _camera.ScreenToWorldPoint(new Vector3(0, 0, 0));
        _bottomRight = _camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
    }
    
    private void Update()
    {
        Vector2 inputDir = PlayerInput.GetMoveDirection(transform.position);
        
        transform.Translate(inputDir * speed * Time.deltaTime);

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, _topLeft.x, _bottomRight.x), Mathf.Clamp(transform.position.y, _topLeft.y, _bottomRight.y), 0);
    }
    
}
