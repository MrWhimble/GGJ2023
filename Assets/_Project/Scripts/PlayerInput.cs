using System;
using Unity.VisualScripting;
using UnityEngine;

public enum InputTypes
{
    Keyboard,
    Gyro,
    Follow
}

public class PlayerInput : MonoBehaviour
{
    private static PlayerInput _instance;
    
    [SerializeField] private InputTypes inputType;
    [SerializeField] private bool useMouseAsFollow = false;
    private Gyroscope _gyro;

    private Camera _camera;
    
    private bool noTouchesLastFrame;
    private int touchId;
    private bool _following;
    private Vector2 _followPos;
    
    private void Awake()
    {
        _instance = this;

        _camera = Camera.main;
        
        if (!Input.gyro.enabled && inputType is InputTypes.Gyro)
        {
            Debug.LogError("Device doesn't have a gyro!!!");
            inputType = InputTypes.Follow;
        }
        _gyro = Input.gyro;
        _gyro.enabled = true;
    }

    public static Vector2 GetMoveDirection(Vector2 currentPosition)
    {
        switch (_instance.inputType)
        {
            case InputTypes.Keyboard:
            {
                return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            }
            case InputTypes.Follow:
            {
                if (!_instance._following)
                    return Vector2.zero;
                return new Vector2(Mathf.Clamp(_instance._followPos.x - currentPosition.x, -1, 1),
                    Mathf.Clamp(_instance._followPos.y - currentPosition.y, -1, 1));
            }
            case InputTypes.Gyro:
            {
                return new Vector2(0, 0);
            }
        }
        return Vector2.zero;
    }

    private void Update()
    {
        if (inputType is InputTypes.Follow)
        {
            Vector2 screenPos = Vector2.zero;
            _following = false;
            if (useMouseAsFollow)
            {
                screenPos = Input.mousePosition;
                _following = true;
            }
            else
            {
                Touch touch = default;
                if (noTouchesLastFrame && Input.touchCount != 0)
                {
                    touch = Input.GetTouch(0);
                    touchId = touch.fingerId;
                    if (touch.phase == TouchPhase.Ended)
                    {
                        touchId = -1;
                        touch = default;
                    }
                } else if (Input.touchCount > 0 && !noTouchesLastFrame)
                {
                    foreach (Touch t in Input.touches)
                    {
                        if (t.fingerId != touchId)
                            continue;
                        touch = t;
                    }
                }

                if (!touch.Equals((Touch) default))
                {
                    screenPos = touch.position;
                    _following = true;
                }
                
                noTouchesLastFrame = Input.touchCount == 0;
            }

            if (!_following)
            {
                return;
            }

            _followPos = _camera.ScreenToWorldPoint(screenPos);
            
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(20, 20, 1000, 20), $"{_gyro.enabled}");
        GUI.Label(new Rect(20, 50, 1000, 20), $"{_gyro.attitude.eulerAngles}");
        GUI.Label(new Rect(20, 80, 1000, 20), $"{_gyro.gravity}");
        GUI.Label(new Rect(20, 110, 1000, 20), $"{Input.acceleration}");
    }
}