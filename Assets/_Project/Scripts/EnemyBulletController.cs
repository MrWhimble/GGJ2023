using UnityEngine;

public class EnemyBulletController : PooledBehaviour
{
    [SerializeField] private float damageValue = 1;
    private Rigidbody2D _rigidbody;
    private float _startTime;
    private float _lifeTime;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    
    public override void Spawn()
    {
        gameObject.SetActive(true);
        _startTime = Time.time;
    }

    public void Init(Vector2 origin, Vector2 direction, float angle, float speed, float lifeTime)
    {
        transform.SetPositionAndRotation(origin, Quaternion.Euler(0, 0, angle));
        _rigidbody.velocity = transform.up * speed;
        _lifeTime = lifeTime;
    }
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        col.collider.GetComponent<PlayerHealth>().Damage(damageValue);
        Die();
    }

    public override void Die()
    {
        gameObject.SetActive(false);
    }
}