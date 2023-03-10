using System;
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
    [SerializeField] private Vector2 followOffset;
    private Gyroscope _gyro;

    private Camera _camera;


    private Quaternion _originalRotation;
    private bool _noTouchesLastFrame;
    private int _touchId;
    private bool _following;
    private Vector2 _followPos;

    private bool _isInDefenceMode;

    public bool IsInDefenceMode
    {
        get => _isInDefenceMode;
        set => _isInDefenceMode = value;
    }
    
    private void Awake()
    {
        _instance = this;

        _camera = Camera.main;
        
        if (!SystemInfo.supportsGyroscope && inputType is InputTypes.Gyro)
        {
            Debug.LogError("Device doesn't have a gyro!!!");
            inputType = InputTypes.Follow;
        }
        _gyro = Input.gyro;
        _gyro.enabled = true;
        _originalRotation = _gyro.attitude;
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
                Quaternion localRot = Quaternion.Inverse(_instance._originalRotation) * Input.gyro.attitude;
                Vector3 forward = localRot * Vector3.forward;
                return new Vector2(forward.x, forward.y);
            }
        }
        return Vector2.zero;
    }

    public static bool IsShooting()
    {
        
        if (_instance._isInDefenceMode)
            return false;
        
        switch (_instance.inputType)
        {
            case InputTypes.Keyboard:
            {
                return true;
            }
            case InputTypes.Follow:
            {
                return _instance._following;
            }
            case InputTypes.Gyro:
            {
                return true;
            }
        }

        return true;
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
                if (_noTouchesLastFrame && Input.touchCount != 0)
                {
                    touch = Input.GetTouch(0);
                    _touchId = touch.fingerId;
                    if (touch.phase == TouchPhase.Ended)
                    {
                        _touchId = -1;
                        touch = default;
                    }
                } else if (Input.touchCount > 0 && !_noTouchesLastFrame)
                {
                    foreach (Touch t in Input.touches)
                    {
                        if (t.fingerId != _touchId)
                            continue;
                        touch = t;
                    }
                }

                if (!touch.Equals((Touch) default))
                {
                    screenPos = touch.position;
                    _following = true;
                }
                
                _noTouchesLastFrame = Input.touchCount == 0;
            }

            if (!_following)
            {
                return;
            }

            _followPos = (Vector3)followOffset + _camera.ScreenToWorldPoint(screenPos);
            
        }
    }

    /*
    private void OnGUI()
    {
        GUI.Label(new Rect(20, 20, 1000, 20), $"{_gyro.enabled}");
        GUI.Label(new Rect(20, 50, 1000, 20), $"{_gyro.attitude.eulerAngles}");
        GUI.Label(new Rect(20, 80, 1000, 20), $"{_gyro.gravity}");
        GUI.Label(new Rect(20, 110, 1000, 20), $"{Input.acceleration}");
    }*/
}