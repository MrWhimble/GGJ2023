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

    private IEnumerator Start()
    {
        yield return WipeAttack();
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
                startCornerPos = _cameraController.BottomLeft;
                oppositeCornerPos = _cameraController.TopRight;
                break;
            case 1:
                startCornerPos = new Vector2(_cameraController.BottomLeft.x, _cameraController.TopRight.y);
                oppositeCornerPos = new Vector2(_cameraController.TopRight.x, _cameraController.BottomLeft.y);
                break;
            case 2:
                startCornerPos = _cameraController.TopRight;
                oppositeCornerPos = _cameraController.BottomLeft;
                break;
            case 3:
                startCornerPos = new Vector2(_cameraController.TopRight.x, _cameraController.BottomLeft.y);
                oppositeCornerPos = new Vector2(_cameraController.BottomLeft.x, _cameraController.TopRight.y);
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
        int side = Random.Range(0, 2);
        int goingUp = Random.Range(0, 2);
        float xPos = side == 0 ? _cameraController.BottomLeft.x : _cameraController.TopRight.x;
        Vector2 direction = side == 0 ? Vector2.right : Vector2.left;
        float angle = side == 0 ? -90 : 90;

        float yDiff = _cameraController.TopRight.y - _cameraController.BottomLeft.y;
        float yDelta = yDiff / (rootObjects.Length + 1f);

        for (int i = 0; i < rootObjects.Length; i++)
        {
            rootObjects[i].position = new Vector2(xPos, _cameraController.BottomLeft.y + (yDelta * (i + 1)));
            rootObjects[i].rotation = Quaternion.Euler(0, 0, angle);
        }

        yield return new WaitForSeconds(4f);
        
        float xDiff = _cameraController.TopRight.x - _cameraController.BottomLeft.x;
        
        for (int i = 0; i < rootObjects.Length; i++)
        {
            int index = goingUp == 0 ? i : rootObjects.Length - i - 1;
            StartCoroutine(MoveRootOutThenIn(rootObjects[index], xDiff - 1f, 50f, 20f, 2f));
            yield return new WaitForSeconds(0.5f);
        }

        AttackFinished();
        yield break;
    }

    private IEnumerator MoveRootOutThenIn(Transform root, float distance, float moveOutSpeed, float moveInSpeed, float waitTime)
    {
        Transform obj = root.GetChild(0);
        obj.localPosition = Vector3.zero;
        root.gameObject.SetActive(true);
        float delta = Time.fixedDeltaTime * moveOutSpeed;
        for (float y = 0f; y < distance; y += delta)
        {
            obj.localPosition = new Vector2(0, y);
            yield return new WaitForFixedUpdate();
        }
        obj.localPosition = new Vector2(0, distance);

        yield return new WaitForSeconds(waitTime);
        
        delta = Time.fixedDeltaTime * moveInSpeed;
        for (float y = distance; y > 0; y -= delta)
        {
            obj.localPosition = new Vector2(0, y);
            yield return new WaitForFixedUpdate();
        }
        obj.localPosition = new Vector2(0, 0);
        root.gameObject.SetActive(false);
    }

    private IEnumerator SequenceRetractAttack()
    {
        int side = Random.Range(0, 2);
        int goingUp = Random.Range(0, 2);
        float xPos = side == 0 ? _cameraController.BottomLeft.x : _cameraController.TopRight.x;
        Vector2 direction = side == 0 ? Vector2.right : Vector2.left;
        float angle = side == 0 ? -90 : 90;

        float yDiff = _cameraController.TopRight.y - _cameraController.BottomLeft.y;
        float yDelta = yDiff / (rootObjects.Length + 1f);

        for (int i = 0; i < rootObjects.Length; i++)
        {
            rootObjects[i].position = new Vector2(xPos, _cameraController.BottomLeft.y + (yDelta * (i + 1)));
            rootObjects[i].rotation = Quaternion.Euler(0, 0, angle);
        }

        yield return new WaitForSeconds(4f);
        
        float xDiff = _cameraController.TopRight.x - _cameraController.BottomLeft.x;
        
        for (int i = 0; i < rootObjects.Length; i++)
        {
            int index = goingUp == 0 ? i : rootObjects.Length - i - 1;
            StartCoroutine(MoveRootOutThenIn(rootObjects[index], xDiff - 1f, 50, 25, 0.05f));
            yield return new WaitForSeconds(0.3f);
        }

        AttackFinished();
        yield break;
    }

    private IEnumerator WipeAttack()
    {
        int side = Random.Range(0, 2);
        int goingRight = Random.Range(0, 2);
        
        float yPos = side == 0 ? _cameraController.BottomLeft.y : _cameraController.TopRight.y;
        float startX = goingRight == 0 ? _cameraController.BottomLeft.x : _cameraController.TopRight.x;
        float endX = goingRight == 0 ? _cameraController.TopRight.x : _cameraController.BottomLeft.x;
        float xDiff = endX - startX;
        Vector2 direction = goingRight == 0 ? Vector2.right : Vector2.left;
        float angle = side == 0 ? 0 : 180;
        
        rootObjects[0].position = new Vector2(startX, yPos);
        rootObjects[0].rotation = Quaternion.Euler(0, 0, angle);
        Transform obj = rootObjects[0].GetChild(0);
        obj.localPosition = Vector3.zero;
        
        yield return new WaitForSeconds(4f);
        
        rootObjects[0].gameObject.SetActive(true);
        float delta = Time.fixedDeltaTime * 40f;
        for (float y = 0f; y < 19f; y += delta)
        {
            obj.localPosition = new Vector2(0, y);
            yield return new WaitForFixedUpdate();
        }
        obj.localPosition = new Vector2(0, 19f);
        
        yield return new WaitForSeconds(1f);

        delta = Time.fixedDeltaTime * 10f;
        if (goingRight == 0)
        {
            for (float x = startX; x < endX; x += delta)
            {
                rootObjects[0].position = new Vector2(x, yPos);
                yield return new WaitForFixedUpdate();
            }
        }
        else
        {
            for (float x = startX; x > endX; x -= delta)
            {
                rootObjects[0].position = new Vector2(x, yPos);
                yield return new WaitForFixedUpdate();
            }
        }
        rootObjects[0].position = new Vector2(endX, yPos);
        
        yield return new WaitForSeconds(1f);
        
        delta = Time.fixedDeltaTime * 10f;
        if (goingRight == 0)
        {
            for (float x = endX; x > startX; x -= delta)
            {
                rootObjects[0].position = new Vector2(x, yPos);
                yield return new WaitForFixedUpdate();
            }
        }
        else
        {
            for (float x = endX; x < startX; x += delta)
            {
                rootObjects[0].position = new Vector2(x, yPos);
                yield return new WaitForFixedUpdate();
            }
        }
        rootObjects[0].position = new Vector2(startX, yPos);

        AttackFinished();
        yield break;
    }
}