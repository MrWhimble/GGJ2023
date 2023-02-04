using UnityEngine;

public class BulletController : PooledBehaviour
{
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    public override void Spawn()
    {
        gameObject.SetActive(true);
    }

    public void Init(Vector2 origin, float angle, float speed)
    {
        transform.SetPositionAndRotation(origin, Quaternion.Euler(0, 0, angle));
        _rigidbody.velocity = transform.up * speed;
    }

    public override void Die()
    {
        gameObject.SetActive(false);
    }
}