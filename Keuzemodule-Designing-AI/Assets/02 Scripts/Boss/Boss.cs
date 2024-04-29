using System;
using UnityEngine;

public class Boss : MonoBehaviour, IDamagableBoss
{
    [Header("Shooting")] 
    [SerializeField] private GameObject rocketPrefab;
    [SerializeField] private GameObject smokePrefab;
    [SerializeField] private float xRocketSpawnRange = 2.0f;
    [SerializeField] private float ySmokeSpawnRange = 0.5f;

    [SerializeField] private int rocketAmount = 10;
    [SerializeField] private int smokeAmount = 10;
    
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private float bulletSpawnDistance = 1.0f;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float fireRate = 0.2f;
    private ObjectPool<BossRockets> rocketPool;
    private ObjectPool<BossSmoke> smokePool;
    private bool isShooting;
    private float nextFireTime;

    private void Awake()
    {
        shootingPoint.localPosition = new Vector3(bulletSpawnDistance, 0f, 0f);

        smokePool = new ObjectPool<BossSmoke>(smokePrefab.GetComponent<BossSmoke>());
        rocketPool = new ObjectPool<BossRockets>(rocketPrefab.GetComponent<BossRockets>());
        
        InitializePool(rocketPool, rocketAmount);
        InitializePool(smokePool, smokeAmount);
    }
    
    private void InitializePool<T>(ObjectPool<T> _pool, int _amount) where T : BossObject<T>
    {
        for (int i = 0; i < _amount; i++)
        {
            T poolObject = (T)_pool.AddNewItemToPool();
            poolObject.Setup(_pool);
        }
    }
    
    public void ShootRockets(int _amount)
    {
        for (int i = 0; i < _amount; i++)
        {
            float random = UnityEngine.Random.Range(-xRocketSpawnRange, xRocketSpawnRange);
            Vector3 spawnRange = new Vector3(random, 0f, 0f);
            RequestProjectile(rocketPool, spawnRange, Vector2.down);
        }
    }
    
    public void ShootSmoke(int _amount)
    {
        for (int i = 0; i < _amount; i++)
        {
            float random = UnityEngine.Random.Range(-ySmokeSpawnRange, ySmokeSpawnRange);
            Vector3 spawnRange = new Vector3(0f, random, 0f);
            RequestProjectile(smokePool, spawnRange, Vector2.left);
        }
    }
    
    private void RequestProjectile<T>(ObjectPool<T> _pool, Vector3 _spawnRange, Vector2 _direction) where T : BossObject<T>
    {
        nextFireTime = Time.time + fireRate;

        Vector3 bossPosition = transform.position;
        Vector3 spawnPosition = bossPosition + _spawnRange;
        shootingPoint.position = spawnPosition;

        T projectile = (T)_pool.RequestObject(spawnPosition);
        if (projectile == null) return;

        projectile.SetDirection(_direction, bulletSpeed);
        projectile.SetRotation(_direction);
    }
}