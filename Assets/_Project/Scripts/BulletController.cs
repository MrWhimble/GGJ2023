using UnityEngine;

public class BulletController : PooledBehaviour
{
    private Rigidbody _rigidbody;
    private Transform child;
    private SpriteRenderer spriteRenderer;
    private float _startTime;
    private float _lifeTime;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        child = transform.GetChild(0);
        spriteRenderer = child.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (_startTime + _lifeTime < Time.time)
        {
            Die();
        }
    }
    
    public override void Spawn()
    {
        gameObject.SetActive(true);
        _startTime = Time.time;
    }

    public void Init(Vector2 origin, float angle, float speed, float lifeTime)
    {
        transform.SetPositionAndRotation(origin, Quaternion.Euler(0, 0, angle));
        _rigidbody.velocity = transform.up * speed;
        _lifeTime = lifeTime;
    }

    public void Init(WeaponData data, int bulletIndex, Vector2 origin)
    {
        transform.SetPositionAndRotation(origin + data.directions[bulletIndex].position, Quaternion.Euler(0, 0, data.directions[bulletIndex].angle));
        _rigidbody.velocity = transform.up * data.speed;
        _lifeTime = data.lifeTime;
        child.localScale = new Vector3(data.size.x, data.size.y, 1);
        spriteRenderer.sprite = data.sprite;
    }

    public override void Die()
    {
        gameObject.SetActive(false);
    }
}