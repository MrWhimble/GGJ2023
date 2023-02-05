using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossAttackController : MonoBehaviour
{
    [SerializeField] private EnemyBulletController bulletPrefab;
    private ObjectPool<EnemyBulletController> _bullets;

    [SerializeField] private Transform root;

    [SerializeField] private Transform player;
    
    [Header("Shank Attack Settings")] 
    [SerializeField] private float shankWaitTimeToAttack;
    [SerializeField] private float shankExtendDistance;
    [SerializeField] private float shankExtendSpeed;
    [SerializeField] private float shankPostExtendWaitTime;
    [SerializeField] private float shankRetractSpeed;

    [Header("Bullet Settings")] 
    [SerializeField] private float bulletWaitTimeToAttack;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Vector2 bulletPosition;
    [SerializeField] private int bulletBurstCount;
    [SerializeField] private int bulletBulletsPerBurst;
    [SerializeField] private float bulletTimeBetweenShots;
    [SerializeField] private float bulletTimeBetweenBursts;

    private Camera _camera;
    private CameraController _cameraController;

    private bool _doAttack;
    private bool _attacking;
    
    private void Awake()
    {
        _bullets = new ObjectPool<EnemyBulletController>();
        _bullets.Init(bulletPrefab, 10);

        _camera = Camera.main;
        _cameraController = _camera.GetComponent<CameraController>();
    }

    public void StartAttacking()
    {
        _doAttack = true;
        _attacking = false;
    }

    private void Update()
    {
        if (!_doAttack)
            return;

        if (_attacking)
            return;
        
        DoAttack(Random.Range(0, 2));
    }
    
    private void DoAttack(int index)
    {
        StopAllCoroutines();
        _attacking = true;
        switch (index)
        {
            case 0:
                StartCoroutine(ShankAttack());
                return;
            case 1:
                StartCoroutine(BulletBurstAttack());
                return;
            default:
                _attacking = false;
                return;
        }
    }

    private IEnumerator ShankAttack()
    {
        yield return new WaitForSeconds(shankWaitTimeToAttack);
        
        float localYMin = _cameraController.TopRight.x;
        float localYMax = _cameraController.BottomLeft.x;
        float localYDiff = localYMax - localYMin;
        float localYLaneDelta = localYDiff / 4f;
        float localXPos = _cameraController.TopRight.y;
        
        int lane = Random.Range(0, 3);

        root.localPosition = new Vector2(localXPos, localYMin + localYLaneDelta * (lane + 1));
        
        

        Transform obj = root.GetChild(0);
        obj.gameObject.SetActive(true);
        float delta = Time.fixedDeltaTime * shankExtendSpeed;
        for (float x = 0f; x < shankExtendDistance; x += delta)
        {
            obj.localPosition = new Vector2(-x, 0);
            yield return new WaitForFixedUpdate();
        }
        obj.localPosition = new Vector2(-shankExtendDistance, 0);

        yield return new WaitForSeconds(shankPostExtendWaitTime);
        
        delta = Time.fixedDeltaTime * shankRetractSpeed;
        for (float x = shankExtendDistance; x > 0f; x -= delta)
        {
            obj.localPosition = new Vector2(-x, 0);
            yield return new WaitForFixedUpdate();
        }
        obj.localPosition = new Vector2(0, 0);
        obj.gameObject.SetActive(false);

        _attacking = false;
        yield break;
    }

    private IEnumerator BulletBurstAttack()
    {
        Vector2 startPosition = transform.TransformPoint(bulletPosition);

        yield return new WaitForSeconds(bulletWaitTimeToAttack);

        for (int i = 0; i < bulletBurstCount; i++)
        {
            for (int j = 0; j < bulletBulletsPerBurst; j++)
            {
                Vector2 direction = (Vector2)player.position - startPosition;
                float angle = Vector2.SignedAngle(Vector2.up, direction);
                EnemyBulletController bullet = _bullets.Spawn();
                bullet.Init(startPosition, direction.normalized, angle, bulletSpeed, 3f);
                yield return new WaitForSeconds(bulletTimeBetweenShots);
            }

            yield return new WaitForSeconds(bulletTimeBetweenBursts);
        }
        
        _attacking = false;
    }
}
