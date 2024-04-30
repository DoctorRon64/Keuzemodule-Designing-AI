using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IDamagableBoss
{
    int Health { get; set; }
    void TakeDamage(int _damageAmount);
}

public class Boss : MonoBehaviour, IBossable, IDamagableBoss, IShootable
{
    [Header("Smoke")] 
    [SerializeField] private GameObject smokePrefab;
    [SerializeField] private float smokeSpeed = 10f;
    [SerializeField] private float ySmokeSpawnRange = 0.5f;
    [SerializeField] private int smokeAmount = 10;
    
    [Header("Rocket")]
    [SerializeField] private GameObject rocketPrefab;
    [SerializeField] private float rocketSpeed = 5f;
    [SerializeField] private float xRocketSpawnRange = 2.0f;
    [SerializeField] private int rocketAmount = 10;

    [Header("bullet")]
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private float bulletSpawnDistance = 1.0f;
    [SerializeField] private float fireRate = 0.2f;
    private ObjectPool<BossRockets> rocketPool;
    private ObjectPool<BossSmoke> smokePool;
    private bool isShooting;
    private float nextFireTime;

    [Header("Arms")] 
    private List<BossArms> bossArms;

    [Header("health")]
    public Action<int> OnBossDied;
    public Action<int> OnHealthChanged;
    public int maxHealth = 500;
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
    
    [Header("Glass")]
    [SerializeField] private List<GameObject> glassPrefab;
    [SerializeField] private int glassAmount = 10;
    private ObjectPool<BossGlass> glassPool;

    protected virtual void InvokeNewHealth(int _newHealth)
    {
        OnHealthChanged?.Invoke(_newHealth);
    }
    
    private void Awake()
    {
        health = maxHealth;
        
        shootingPoint.localPosition = new Vector3(bulletSpawnDistance, 0f, 0f);

        var bossArmsArray = FindObjectsOfType<BossArms>();
        bossArms = new List<BossArms>(bossArmsArray);
        
        smokePool = new ObjectPool<BossSmoke>(smokePrefab.GetComponent<BossSmoke>());
        rocketPool = new ObjectPool<BossRockets>(rocketPrefab.GetComponent<BossRockets>());
        glassPool = new ObjectPool<BossGlass>(glassPrefab.Select(_prefab => _prefab.GetComponent<BossGlass>()).ToList());
        
        InitializePool(glassPool, glassAmount);
        InitializePool(rocketPool, rocketAmount);
        InitializePool(smokePool, smokeAmount);
    }
    
    private void InitializePool<T>(ObjectPool<T> _pool, int _amount) where T : BossProjectile<T>
    {
        for (int i = 0; i < _amount; i++)
        {
            T poolObject = (T)_pool.AddNewItemToPool();
            poolObject.Setup(_pool);
        }
    }

    public void ActivateArms()
    {
        foreach (BossArms arm in bossArms)
        {
            arm.ActivateArms();
        }
    }

    public void DeactivateArms()
    {
        foreach (BossArms arm in bossArms)
        {
            arm.DeactivateArms();
        }
    }
    
    public void ThrowGlass(int _amount)
    {
        for (int i = 0; i < _amount; i++)
        {
            float randomY = UnityEngine.Random.Range(-ySmokeSpawnRange, ySmokeSpawnRange);
            Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + randomY, transform.position.z);
            RequestGlass(spawnPosition);
        }
    }

    private void RequestGlass(Vector3 _spawnPosition)
    {
        BossGlass glass = (BossGlass)glassPool.RequestObject(_spawnPosition);
        if (glass == null) return;

        glass.ThrowGlass();
    }

    public void ShootRockets(int _amount)
    {
        for (int i = 0; i < _amount; i++)
        {
            float random = UnityEngine.Random.Range(-xRocketSpawnRange, xRocketSpawnRange);
            Vector3 spawnRange = new Vector3(random, 0f, 0f);
            RequestProjectile(rocketPool, spawnRange, Vector2.down, rocketSpeed);
        }
    }
    
    public void ShootSmoke(int _amount)
    {
        for (int i = 0; i < _amount; i++)
        {
            float random = UnityEngine.Random.Range(-ySmokeSpawnRange, ySmokeSpawnRange);
            Vector3 spawnRange = new Vector3(0f, random, 0f);
            RequestProjectile(smokePool, spawnRange, Vector2.left, smokeSpeed);
        }
    }
    
    private void RequestProjectile<T>(ObjectPool<T> _pool, Vector3 _spawnRange, Vector2 _direction, float _speed) where T : BossProjectile<T>
    {
        nextFireTime = Time.time + fireRate;

        Vector3 bossPosition = transform.position;
        Vector3 spawnPosition = bossPosition + _spawnRange;
        shootingPoint.position = spawnPosition;

        T projectile = (T)_pool.RequestObject(spawnPosition);
        if (projectile == null) return;

        projectile.SetDirection(_direction, _speed);
        projectile.SetRotation(_direction);
    }

    public void TakeDamage(int _damageAmount)
    {
        Health -= _damageAmount;
        Debug.Log("boss health" + _damageAmount + "" + health);
        if (Health <= 0)
        {
            OnBossDied.Invoke(1);
        }
    }
}