using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField] private WeaponData[] weapons;
    public int weaponIndex;
    
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
        
    }

    private void Update()
    {
        if (PlayerInput.IsShooting() && Time.time > _lastShotTime + weapons[weaponIndex].deltaTime)
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        var data = weapons[weaponIndex];
        for (int i = 0; i < data.directions.Length; i++)
        {
            BulletController bc = _bullets.Spawn();
            bc.Init(data, i, origin.position);
        }
        _lastShotTime = Time.time;
    }
}