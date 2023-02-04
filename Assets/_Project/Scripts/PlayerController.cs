using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float upgradedSpeed;
    [SerializeField] private float slowSpeed;
    [SerializeField] private float upgradedSlowSpeed;

    private bool _spedUp;
    
    private Rigidbody2D _rigidbody;
    private Camera _camera;
    private CameraController _cameraController;

    private Vector2 _inputDir;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _camera = Camera.main;
        _cameraController = _camera.GetComponent<CameraController>();
        _spedUp = false;
    }

    public void UpgradeSpeed()
    {
        _spedUp = true;
    }

    private void Update()
    {
        _inputDir = PlayerInput.GetMoveDirection(_rigidbody.position);
    }

    private void FixedUpdate()
    {
        Vector2 currentPos = _rigidbody.position;
        float s = _spedUp ? upgradedSpeed : speed;
        Vector2 newPos = currentPos + (_inputDir * s * Time.fixedDeltaTime);
        
        newPos = new Vector2(Mathf.Clamp(newPos.x, _cameraController.TopLeft.x, _cameraController.BottomRight.x), Mathf.Clamp(newPos.y, _cameraController.TopLeft.y, _cameraController.BottomRight.y));
        
        _rigidbody.MovePosition(newPos);
    }
    
}
