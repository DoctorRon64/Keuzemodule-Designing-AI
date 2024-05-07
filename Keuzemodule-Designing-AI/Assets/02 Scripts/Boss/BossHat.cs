using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossHat : MonoBehaviour, IShootable, IDamagableBoss
{
    public delegate void HatDestroyedHandler();
    public event HatDestroyedHandler OnHatDestroyed;
    public Action<int> OnHealthChanged;

    [SerializeField] private float driftStrength = 1f;
    [SerializeField] private float maxAngularVelocity = 10f;
    public int maxHealth = 20;

    private Rigidbody2D rb;
    private Vector2 driftDirection;
    private bool onTimePositionSet = false;
    private int health;
    
    public int Health 
    {
        get => health;
        set
        {
            if (health == value) return;
            health = value;
            InvokeNewHealth(health);
            if (health <= 0)
            {
                DestroyHat();
            }
        }
    }

    protected virtual void InvokeNewHealth(int _newHealth)
    {
        OnHealthChanged?.Invoke(_newHealth);
    }
    
    public void SetPosition(Vector2 _pos)
    {
        if (onTimePositionSet) return;
        transform.position = _pos;
        onTimePositionSet = true;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Health = maxHealth;
        driftDirection = Random.insideUnitCircle.normalized;

        rb.angularVelocity = Random.Range(-maxAngularVelocity, maxAngularVelocity);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        rb.AddForce(driftDirection * (driftStrength * Time.deltaTime));
    }

    public void TakeDamage(int _damageAmount)
    {
        Health -= _damageAmount;
    }

    private void DestroyHat()
    {
        OnHatDestroyed?.Invoke();
        gameObject.SetActive(false);
    }
}