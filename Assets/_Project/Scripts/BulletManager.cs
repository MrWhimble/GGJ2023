using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField] private BulletController prefab;
    [SerializeField] private Transform origin;
    
    private ObjectPool<BulletController> _bullets;

    private float _lastShotTime;

    private void Awake()
    {
        _bullets = new ObjectPool<BulletController>();
        _bullets.Init(prefab, 10);
    }

    private void Start()
    {
        Shoot();
    }

    private void Update()
    {
        if (PlayerInput.IsShooting() && Time.time > _lastShotTime + 1f)
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        BulletController bc = _bullets.Spawn();
        bc.Init(((Vector2)origin.position) + new Vector2(1, 1), 45, 1);

        _lastShotTime = Time.time;
    }
}