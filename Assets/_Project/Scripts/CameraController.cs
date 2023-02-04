using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float zoomedInSize;
    [SerializeField] private float zoomedOutSize;
    [SerializeField] private float transitionSpeed;

    private bool _zoomingIn;
    private bool _transitioning;

    private Camera _camera;

    private Vector2 _topLeft;
    public Vector2 TopLeft => _topLeft;

    private Vector2 _bottomRight;
    public Vector2 BottomRight => _bottomRight;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        UpdateCorners();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (!_transitioning)
            return;

        float currentSize = _camera.orthographicSize;
        float endSize = _zoomingIn ? zoomedInSize : zoomedOutSize;

        _camera.orthographicSize = Mathf.Lerp(currentSize, endSize, transitionSpeed * Time.deltaTime);

        if (Mathf.Abs(_camera.orthographicSize - endSize) < 0.01f)
        {
            _camera.orthographicSize = endSize;
            _transitioning = false;
        }

        UpdateCorners();
    }

    private void UpdateCorners()
    {
        _topLeft = _camera.ScreenToWorldPoint(new Vector3(0, 0, 0));
        _bottomRight = _camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
    }

    public void ZoomIn()
    {
        _transitioning = true;
        _zoomingIn = true;
    }

    public void ZoomOut()
    {
        _transitioning = true;
        _zoomingIn = false;
    }

    public void InstantZoomIn()
    {
        _camera.orthographicSize = zoomedInSize;
        UpdateCorners();
    }

    public void InstantZoomOut()
    {
        _camera.orthographicSize = zoomedOutSize;
        UpdateCorners();
    }
}
