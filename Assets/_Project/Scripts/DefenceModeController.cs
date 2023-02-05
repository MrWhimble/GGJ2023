using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DefenceModeController : MonoBehaviour
{
    private Camera _camera;
    private CameraController _cameraController;

    [SerializeField] private Transform roots;
    [SerializeField] private float startY;
    [SerializeField] private float endY;
    [SerializeField] private float speed;

    [SerializeField] private UnityEvent onDefenceFinished;

    private void Awake()
    {
        _camera = Camera.main;
        _cameraController = _camera.GetComponent<CameraController>();
    }
    
    public void StartDefence()
    {
        StartCoroutine(DoDefenceSequence());
    }

    private IEnumerator DoDefenceSequence()
    {
        _cameraController.ZoomIn();
        yield return new WaitForSeconds(1.5f);
        yield return RaiseRoots();
        float waitTime = Random.Range(7f, 13f);
        yield return new WaitForSeconds(waitTime);
        yield return LowerRoots();
        _cameraController.ZoomOut();
        yield return new WaitForSeconds(3);
        onDefenceFinished?.Invoke();
    }

    private IEnumerator RaiseRoots()
    {
        float yPos = startY;
        roots.localPosition = new Vector2(0, yPos);
        roots.gameObject.SetActive(true);
        
        while (yPos < endY - 0.01f)
        {
            yPos = Mathf.Lerp(yPos, endY, Time.fixedDeltaTime * speed);
            roots.localPosition = new Vector2(0, yPos);
            yield return new WaitForFixedUpdate();
        }
        roots.localPosition = new Vector2(0, endY);
    }

    private IEnumerator LowerRoots()
    {
        float yPos = endY;
        while (yPos > startY + 0.01f)
        {
            yPos = Mathf.Lerp(yPos, startY, Time.fixedDeltaTime * speed);
            roots.localPosition = new Vector2(0, yPos);
            yield return new WaitForFixedUpdate();
        }
        roots.localPosition = new Vector2(0, startY);
        roots.gameObject.SetActive(false);
    }
    
    
}
