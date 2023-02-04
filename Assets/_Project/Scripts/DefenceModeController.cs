using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceModeController : MonoBehaviour
{
    private Camera _camera;
    private CameraController _cameraController;

    private void Awake()
    {
        _camera = Camera.main;
        _cameraController = _camera.GetComponent<CameraController>();
    }
    
    public void StartDefence()
    {
        _cameraController.ZoomIn();
    }
}
