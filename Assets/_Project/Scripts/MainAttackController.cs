using System.Collections;
using UnityEngine;

public class MainAttackController : MonoBehaviour
{
    private Camera _camera;
    private CameraController _cameraController;

    private bool _attacking;

    [SerializeField] private Transform[] rootObjects;


    private void Awake()
    {
        _camera = Camera.main;
        _cameraController = _camera.GetComponent<CameraController>();
        _attacking = false;
    }

    private void Start()
    {
        StartCoroutine(RandomCornerAttack());
    }

    private void DoRandomAttack()
    {
        _attacking = true;
        int index = Random.Range(0, 4);
        switch (index)
        {
            case 0:
                StartCoroutine(RandomCornerAttack());
                return;
            case 1:
                StartCoroutine(SequenceRemainAttack());
                return;
            case 2:
                StartCoroutine(SequenceRetractAttack());
                return;
            case 3:
                StartCoroutine(WipeAttack());
                return;
            default:
                _attacking = false;
                return;
        }
    }

    private void AttackFinished()
    {
        _attacking = false;
    }

    private IEnumerator RandomCornerAttack()
    {
        int corner = Random.Range(0, 4);
        Vector2 startCornerPos;
        Vector2 oppositeCornerPos;
        switch (corner)
        {
            case 0:
                startCornerPos = _cameraController.TopLeft;
                oppositeCornerPos = _cameraController.BottomRight;
                break;
            case 1:
                startCornerPos = new Vector2(_cameraController.BottomRight.x, _cameraController.TopLeft.y);
                oppositeCornerPos = new Vector2(_cameraController.TopLeft.x, _cameraController.BottomRight.y);
                break;
            case 2:
                startCornerPos = _cameraController.BottomRight;
                oppositeCornerPos = _cameraController.TopLeft;
                break;
            case 3:
                startCornerPos = new Vector2(_cameraController.TopLeft.x, _cameraController.BottomRight.y);
                oppositeCornerPos = new Vector2(_cameraController.BottomRight.x, _cameraController.TopLeft.y);
                break;
            default:
                AttackFinished();
                yield break;
        }

        Vector2 direction = oppositeCornerPos - startCornerPos;

        rootObjects[0].position = startCornerPos;
        float angle = Vector2.SignedAngle(Vector2.up, direction);
        rootObjects[0].rotation = Quaternion.Euler(0, 0, angle);
        Transform obj = rootObjects[0].GetChild(0);
        obj.localPosition = Vector3.zero;

        yield return new WaitForSeconds(4f);
        
        rootObjects[0].gameObject.SetActive(true);
        float delta = Time.fixedDeltaTime * 50f;
        for (float y = 0f; y < 21f; y += delta)
        {
            obj.localPosition = new Vector2(0, y);
            yield return new WaitForFixedUpdate();
        }
        obj.localPosition = new Vector2(0, 21);

        yield return new WaitForSeconds(2f);
        
        delta = Time.fixedDeltaTime * 20f;
        for (float y = 21f; y > 0; y -= delta)
        {
            obj.localPosition = new Vector2(0, y);
            yield return new WaitForFixedUpdate();
        }
        obj.localPosition = new Vector2(0, 0);
        rootObjects[0].gameObject.SetActive(false);
        
        AttackFinished();
        yield break;
    }

    private IEnumerator SequenceRemainAttack()
    {
        
        
        AttackFinished();
        yield break;
    }

    private IEnumerator SequenceRetractAttack()
    {
        
        
        AttackFinished();
        yield break;
    }

    private IEnumerator WipeAttack()
    {
        
        
        AttackFinished();
        yield break;
    }
}