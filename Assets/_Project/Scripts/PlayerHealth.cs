using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    private float _health;

    private bool _dead;

    [SerializeField] private UnityEvent onDeath;

    private void Awake()
    {
        _health = maxHealth;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Damage(1);
    }

    public void Damage(float value)
    {
        _health -= value;
        if (_health <= 0)
        {
            _dead = true;
            onDeath?.Invoke();
        }
    }
}