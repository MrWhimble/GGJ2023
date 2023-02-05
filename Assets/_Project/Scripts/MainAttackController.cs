using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

public class MainAttackController : MonoBehaviour
{
    private Camera _camera;
    private CameraController _cameraController;

    private bool _attacking;

    [SerializeField] private Transform[] rootObjects;

    [SerializeField] private UnityEvent onAttackingFinished;

    [Header("Corner Attack Settings")] 
    [SerializeField] private float cornerTimeBeforeAttack = 4f;
    [SerializeField] private float cornerInSpeed = 50f;
    [SerializeField] private float cornerWaitTime = 2f;
    [SerializeField] private float cornerOutSpeed = 20f;
    [SerializeField] private float cornerMaxExtend = 21f;

    [Header("Remain Sequence Attack Settings")]
    [SerializeField] private float sequenceRemainTimeBeforeAttack = 4f;
    [SerializeField] private float sequenceRemainInSpeed = 50f;
    [SerializeField] private float sequenceRemainWaitTime = 2f;
    [SerializeField] private float sequenceRemainOutSpeed = 20f;
    [SerializeField] private float sequenceRemainTimeBetweenRoots = 0.5f;
    
    [Header("Retract Sequence Attack Settings")]
    [SerializeField] private float sequenceRetractTimeBeforeAttack = 4f;
    [SerializeField] private float sequenceRetractInSpeed = 50f;
    [SerializeField] private float sequenceRetractWaitTime = 0.05f;
    [SerializeField] private float sequenceRetractOutSpeed = 25f;
    [SerializeField] private float sequenceRetractTimeBetweenRoots = 0.3f;

    [Header("Wipe Attack Settings")]
    [SerializeField] private float wipeTimeBeforeAttack = 4f;
    [SerializeField] private float wipeExtendSpeed = 40f;
    [SerializeField] private float wipeMaxExtend = 19f;
    [SerializeField] private float wipePostExtendWaitTime = 1f;
    [SerializeField] private float wipeSwipeSpeed = 10f;
    [SerializeField] private float wipePostSwipeWaitTime = 1f;
    [SerializeField] private float wipeRetractSpeed = 20f;

    private bool _isAttackingStage;
    public bool IsAttackingStage
    {
        get => _isAttackingStage;
        set => _isAttackingStage = value;
    }
    private int _maxAttackIndex;
    private int _attackIndex;
    private bool _doingAttack;
    private void Awake()
    {
        _camera = Camera.main;
        _cameraController = _camera.GetComponent<CameraController>();
        _attacking = false;
    }

    private void Update()
    {
        if (!_isAttackingStage)
            return;

        if (!_doingAttack)
            return;
        
        if (_attacking)
            return;

        if (_attackIndex >= _maxAttackIndex)
        {
            if (_doingAttack)
            {
                _doingAttack = false;
                Debug.Log("Attacking Finished");
                onAttackingFinished?.Invoke();
            }
            return;
        }
        

        _attackIndex++;
        DoAttack(Random.Range(0, 4));
    }

    public void StartAttack()
    {
        _doingAttack = true;
        _attacking = false;
        _maxAttackIndex = Random.Range(3, 8);
        _attackIndex = 0;
    }

    private void DoAttack(int index)
    {
        StopAllCoroutines();
        _attacking = true;
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

    /*
    private IEnumerator DoAttackSequence()
    {
        int attackCount = Random.Range(3, 8);
        for (int i = 0; i < attackCount; i++)
        {
            int index = Random.Range(0, 4);
            yield return DoAttack(index);
        }
    }*/

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

        rootObjects[0].GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSeconds(cornerTimeBeforeAttack);
        rootObjects[0].GetChild(1).gameObject.SetActive(false);
        
        obj.gameObject.SetActive(true);
        rootObjects[0].GetComponent<AudioSource>().Play();
        float delta = Time.fixedDeltaTime * cornerInSpeed;
        for (float y = 0f; y < cornerMaxExtend; y += delta)
        {
            obj.localPosition = new Vector2(0, y);
            yield return new WaitForFixedUpdate();
        }
        obj.localPosition = new Vector2(0, cornerMaxExtend);

        yield return new WaitForSeconds(cornerWaitTime);
        
        delta = Time.fixedDeltaTime * cornerOutSpeed;
        for (float y = cornerMaxExtend; y > 0; y -= delta)
        {
            obj.localPosition = new Vector2(0, y);
            yield return new WaitForFixedUpdate();
        }
        obj.localPosition = new Vector2(0, 0);
        obj.gameObject.SetActive(false);
        
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

        for (int i = 0; i < rootObjects.Length; i++)
            rootObjects[i].GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSeconds(sequenceRemainTimeBeforeAttack);
        for (int i = 0; i < rootObjects.Length; i++)
            rootObjects[i].GetChild(1).gameObject.SetActive(false);
        
        float xDiff = _cameraController.TopRight.x - _cameraController.BottomLeft.x;

        Coroutine[] coroutines = new Coroutine[rootObjects.Length];
        for (int i = 0; i < rootObjects.Length; i++)
        {
            int index = goingUp == 0 ? i : rootObjects.Length - i - 1;
            coroutines[i] = StartCoroutine(MoveRootOutThenIn(rootObjects[index], xDiff - 1f, sequenceRemainInSpeed, sequenceRemainOutSpeed, sequenceRemainWaitTime));
            yield return new WaitForSeconds(sequenceRemainTimeBetweenRoots);
        }

        yield return coroutines[0];
        yield return coroutines[1];
        yield return coroutines[2];

        AttackFinished();
        yield break;
    }

    private IEnumerator MoveRootOutThenIn(Transform root, float distance, float moveOutSpeed, float moveInSpeed, float waitTime)
    {
        Transform obj = root.GetChild(0);
        obj.localPosition = Vector3.zero;
        obj.gameObject.SetActive(true);
        root.GetComponent<AudioSource>().Play();
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
        obj.gameObject.SetActive(false);
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

        for (int i = 0; i < rootObjects.Length; i++)
            rootObjects[i].GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSeconds(sequenceRetractTimeBeforeAttack);
        for (int i = 0; i < rootObjects.Length; i++)
            rootObjects[i].GetChild(1).gameObject.SetActive(false);
        
        float xDiff = _cameraController.TopRight.x - _cameraController.BottomLeft.x;
        
        Coroutine[] coroutines = new Coroutine[rootObjects.Length];
        for (int i = 0; i < rootObjects.Length; i++)
        {
            int index = goingUp == 0 ? i : rootObjects.Length - i - 1;
            coroutines[i] = StartCoroutine(MoveRootOutThenIn(rootObjects[index], xDiff - 1f, 50, 25, 0.05f));
            yield return new WaitForSeconds(0.3f);
        }
        
        yield return coroutines[0];
        yield return coroutines[1];
        yield return coroutines[2];

        AttackFinished();
        yield break;
    }

    private IEnumerator WipeAttack()
    {
        int side = Random.Range(0, 2);
        int goingRight = Random.Range(0, 2);
        
        float yPos = side == 0 ? _cameraController.BottomLeft.y : _cameraController.TopRight.y;
        float startX = goingRight == 0 ? _cameraController.BottomLeft.x + 1.5f : _cameraController.TopRight.x - 1.5f;
        float endX = goingRight == 0 ? _cameraController.TopRight.x - 3.5f : _cameraController.BottomLeft.x + 3.5f;
        Vector2 direction = goingRight == 0 ? Vector2.right : Vector2.left;
        float angle = side == 0 ? 0 : 180;
        
        rootObjects[0].position = new Vector2(startX, yPos);
        rootObjects[0].rotation = Quaternion.Euler(0, 0, angle);
        Transform obj = rootObjects[0].GetChild(0);
        obj.localPosition = Vector3.zero;
        
        rootObjects[0].GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSeconds(wipeTimeBeforeAttack);
        rootObjects[0].GetChild(1).gameObject.SetActive(false);
        
        rootObjects[0].GetComponent<AudioSource>().Play();
        obj.gameObject.SetActive(true);
        float delta = Time.fixedDeltaTime * wipeExtendSpeed;
        for (float y = 0f; y < wipeMaxExtend; y += delta)
        {
            obj.localPosition = new Vector2(0, y);
            yield return new WaitForFixedUpdate();
        }
        obj.localPosition = new Vector2(0, wipeMaxExtend);
        
        yield return new WaitForSeconds(wipePostExtendWaitTime);

        delta = Time.fixedDeltaTime * wipeSwipeSpeed;
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
        
        yield return new WaitForSeconds(wipePostSwipeWaitTime);
        
        delta = Time.fixedDeltaTime * wipeSwipeSpeed;
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
        
        yield return new WaitForSeconds(wipePostExtendWaitTime);
        
        delta = Time.fixedDeltaTime * wipeRetractSpeed;
        for (float y = wipeMaxExtend; y > 0; y -= delta)
        {
            obj.localPosition = new Vector2(0, y);
            yield return new WaitForFixedUpdate();
        }
        obj.localPosition = new Vector2(0, 0);
        obj.gameObject.SetActive(false);
        

        AttackFinished();
        yield break;
    }
}