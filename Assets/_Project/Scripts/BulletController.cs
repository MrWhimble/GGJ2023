using System;
using UnityEngine;

public class BulletController : PooledBehaviour
{
    [SerializeField] private float damageValue;
    
    private Rigidbody2D _rigidbody;
    private Transform _child;
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider;
    private float _startTime;
    private float _lifeTime;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _child = transform.GetChild(0);
        _spriteRenderer = _child.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (_startTime + _lifeTime < Time.time)
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        col.collider.GetComponent<EnemyHealth>().Damage(damageValue);
        Die();
    }

    public override void Spawn()
    {
        gameObject.SetActive(true);
        _startTime = Time.time;
    }

    public void Init(WeaponData data, int bulletIndex, Vector2 origin)
    {
        transform.SetPositionAndRotation(origin + data.directions[bulletIndex].position, Quaternion.Euler(0, 0, data.directions[bulletIndex].angle));
        _rigidbody.velocity = transform.up * data.speed;
        _lifeTime = data.lifeTime;
        _child.localScale = new Vector3(data.size.x, data.size.y, 1);
        _spriteRenderer.sprite = data.sprite;
        _boxCollider.size = data.size / 2f;
    }

    public override void Die()
    {
        gameObject.SetActive(false);
    }
}