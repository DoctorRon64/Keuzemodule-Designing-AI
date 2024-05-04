using System;
using Cinemachine;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamagable
{
    public int maxHealth = 50;
    private int health;
    public int Health
    {
        get => health;
        set
        {
            if (health == value) return;
            health = value;
            InvokeNewHealth(health);
        }
    }

    public Action<int> OnPlayerDied;
    public Action<int> OnHealthChanged;
    private CinemachineImpulseSource shakeImpulseSource;

    void Start()
    {
        health = maxHealth;
        shakeImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Shake()
    {
        shakeImpulseSource.GenerateImpulse();
    }

    public void TakeDamage(int damage)
    {
        Shake();
        Health -= damage;
        if (Health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnPlayerDied?.Invoke(2);
    }

    protected virtual void InvokeNewHealth(int newHealth)
    {
        OnHealthChanged?.Invoke(newHealth);
    }
}