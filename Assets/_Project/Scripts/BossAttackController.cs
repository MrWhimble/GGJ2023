using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackController : MonoBehaviour
{
    [SerializeField] private EnemyBulletController bulletPrefab;
    private ObjectPool<EnemyBulletController> _bullets;

    [SerializeField] private Transform root; 

    [Header("Shank Attack Settings")] 
    [SerializeField] private float shankWaitTimeToAttack;
    [SerializeField] private float shankExtendDistance;
    [SerializeField] private float shankPostExtendWaitTime;

    private Camera _camera;
    private CameraController _cameraController;

    private void Awake()
    {
        _bullets = new ObjectPool<EnemyBulletController>();
        _bullets.Init(bulletPrefab, 10);

        _camera = Camera.main;
        _cameraController = _camera.GetComponent<CameraController>();
    }

    private IEnumerator ShankAttack()
    {
        float localYMin = _cameraController.BottomLeft.x;
        float localYMax = _cameraController.TopRight.x;
        float localYDiff = localYMax - localYMin;
        float localYLaneDelta = localYDiff / 5f;
        float localXPos = _cameraController.BottomLeft.y;
        
        int lane = Random.Range(0, 3);

        root.localPosition = new Vector2(localXPos, localYMin + localYLaneDelta * (lane + 1));

        yield break;
    }
}
