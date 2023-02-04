using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField] private BulletController prefab;
    
    private ObjectPool<BulletController> _bullets;

    private void Awake()
    {
        _bullets.Init(prefab, 10);
    }
}