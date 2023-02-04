using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float slowSpeed;

    private Rigidbody2D _rigidbody;
    private Camera _camera;

    private Vector2 _topLeft;
    private Vector2 _bottomRight;

    private Vector2 _inputDir;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _camera = Camera.main;
        _topLeft = _camera.ScreenToWorldPoint(new Vector3(0, 0, 0));
        _bottomRight = _camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
    }

    private void Update()
    {
        _inputDir = PlayerInput.GetMoveDirection(_rigidbody.position);
        
        //transform.Translate(inputDir * speed * Time.deltaTime);

        //transform.position = new Vector3(Mathf.Clamp(transform.position.x, _topLeft.x, _bottomRight.x), Mathf.Clamp(transform.position.y, _topLeft.y, _bottomRight.y), 0);
    }

    private void FixedUpdate()
    {
        Vector2 currentPos = _rigidbody.position;
        Vector2 newPos = currentPos + (_inputDir * speed * Time.fixedDeltaTime);
        
        newPos = new Vector2(Mathf.Clamp(newPos.x, _topLeft.x, _bottomRight.x), Mathf.Clamp(newPos.y, _topLeft.y, _bottomRight.y));
        
        _rigidbody.MovePosition(newPos);
    }
    
}
