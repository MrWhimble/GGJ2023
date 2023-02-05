using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    private float _health;

    private bool _dead;

    [SerializeField] private UnityEvent onDeath;

    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        _health = maxHealth;
        audioSource.playOnAwake = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Damage(1);
    }

    public void Damage(float value)
    {
        if (_dead)
            return;

        audioSource.Play();
        
        _health -= value;
        if (_health <= 0)
        {
            _dead = true;
            onDeath?.Invoke();
        }

        StartCoroutine(TookDamage());
    }

    IEnumerator TookDamage()
    {
        gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(.5f);
        gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;
    }
}

