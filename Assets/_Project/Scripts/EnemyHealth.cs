using System;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    private float _health;

    private bool _dead;

    [SerializeField] private UnityEvent onDeath;


    [SerializeField] private float timeBetweenSounds;
    private AudioSource _audioSource;
    private float _lastTimePlayed;

    private void Awake()
    {
        _health = maxHealth;
        _dead = false;

        _audioSource = GetComponent<AudioSource>();
    }

    public void Damage(float value)
    {
        if (_dead) 
            return;

        if (_lastTimePlayed + timeBetweenSounds < Time.time)
        {
            _audioSource.Play();
            _lastTimePlayed = Time.time;
        }
        
        _health -= value;
        if (_health <= 0)
        {
            _dead = true;
            onDeath?.Invoke();
        }
    }
}