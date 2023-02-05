using System;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    private float _health;

    private bool _dead;

    [SerializeField] private UnityEvent onDeath;

    private void Awake()
    {
        _health = maxHealth;
        _dead = false;
    }

    public void Damage(float value)
    {
        if (_dead) return;

            _health -= value;
        if (_health <= 0)
        {
            _dead = true;
            onDeath?.Invoke();
        }
    }
}