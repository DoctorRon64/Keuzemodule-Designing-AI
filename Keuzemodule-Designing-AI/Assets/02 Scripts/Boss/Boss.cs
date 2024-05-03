using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class BossProjectileController<T> where T : BossProjectile<T>
{
    public GameObject prefab;
    public float speed = 10f;
    public float spawnRange = 0.5f;
    public int amount = 10;
    public ObjectPool<T> ObjectPool;

    public void InitializePool()
    {
        ObjectPool = new ObjectPool<T>(prefab.GetComponent<T>());
        for (int i = 0; i < amount; i++)
        {
            T poolObject = (T)ObjectPool.AddNewItemToPool();
            poolObject.Setup(ObjectPool);
        }
    }
}

public sealed class Boss : MonoBehaviour, IBossable, IDamagableBoss, IShootable
{
    [Header("projectile Controllers")]
    [SerializeField] private BossProjectileController<BossSmoke> smokeController;
    [SerializeField] private BossProjectileController<BossRockets> rocketController;
    [SerializeField] private BossProjectileController<BossFollowingRocket> followRocketController;

    [Header("Glass")] 
    [SerializeField] private List<GameObject> glassPrefabs;
    [SerializeField] private int glassAmount = 10;
    private ObjectPool<BossGlass> glassPool;

    [Header("bullet")] 
    [SerializeField] private int bossDamage = 2;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private float bulletSpawnDistance = 1.0f;
    [SerializeField] private float fireRate = 0.2f;
    private bool isShooting; //private flag
    private float nextFireTime;
    
    //Arms 
    private List<BossArms> bossArms;

    //Animations
    private Animator anim;
    private static readonly int hitParam = Animator.StringToHash("Hit");
    private const string bossHurtAnim = "Boss Hurt";

    [Header("health")]
    private AudioSource bossSoundPlayer;
    [SerializeField] private ParticleSystem hurtParticles;
    [SerializeField] private List<AudioClip> hurtSound;
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

    private void InvokeNewHealth(int _newHealth)
    {
        OnHealthChanged?.Invoke(_newHealth); ;
    }

    private void Awake()
    {
        health = maxHealth;
        bossSoundPlayer = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        
        shootingPoint.localPosition = new Vector3(bulletSpawnDistance, 0f, 0f);
        var bossArmsArray = FindObjectsOfType<BossArms>();
        bossArms = new List<BossArms>(bossArmsArray);
        
        glassPool = new ObjectPool<BossGlass>(glassPrefabs.Select(_prefab => _prefab.GetComponent<BossGlass>()).ToList());
        InitializePool(glassPool, glassAmount);

        smokeController.InitializePool();
        rocketController.InitializePool();
        followRocketController.InitializePool();
    }

    private static void InitializePool<T>(ObjectPool<T> _pool, int _amount) where T : BossProjectile<T>
    {
        for (int i = 0; i < _amount; i++)
        {
            T poolObject = (T)_pool.AddNewItemToPool();
            poolObject.Setup(_pool);
        }
    }

    private void OnCollisionEnter2D(Collision2D _other)
    {
        if (_other.gameObject.TryGetComponent(out IDamagable damagable))
        {
            damagable.TakeDamage(bossDamage);
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
            float randomY = Random.Range(-smokeController.spawnRange, smokeController.spawnRange);
            Vector3 transformPos = transform.position;
            Vector3 spawnPosition = new Vector3(transformPos.x, transformPos.y + randomY, transformPos.z);
            RequestGlass(spawnPosition);
        }
    }

    public void ShootFollowRocket(int _amount)
    {
        for (int i = 0; i < _amount; i++)
        {
            float random = Random.Range(-followRocketController.spawnRange, followRocketController.spawnRange);
            Vector3 spawnRange = new Vector3(random, random, 0f);
            RequestProjectile(followRocketController.ObjectPool, spawnRange, Vector2.up, followRocketController.speed);
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
            float random = Random.Range(-rocketController.spawnRange, rocketController.spawnRange);
            Vector3 spawnRange = new Vector3(random, 0f, 0f);
            RequestProjectile(rocketController.ObjectPool, spawnRange, Vector2.down, rocketController.speed);
        }
    }

    public void ShootSmoke(int _amount)
    {
        for (int i = 0; i < _amount; i++)
        {
            float random = Random.Range(-smokeController.spawnRange, smokeController.spawnRange);
            Vector3 spawnRange = new Vector3(0f, random, 0f);
            RequestProjectile(smokeController.ObjectPool, spawnRange, Vector2.left, smokeController.speed);
        }
    }

    private void RequestProjectile<T>(ObjectPool<T> _pool, Vector3 _spawnRange, Vector2 _direction, float _speed)
        where T : BossProjectile<T>
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
        if (Health <= 0)
        {
            OnBossDied.Invoke(1);
        }

        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (!stateInfo.IsName(bossHurtAnim))
        {
            anim.SetTrigger(hitParam);
        }
        hurtParticles.Play();
        
        if (bossSoundPlayer.isPlaying) return;
        AudioClip clipToPlay = hurtSound[Random.Range(0, hurtSound.Count)];
        bossSoundPlayer.PlayOneShot(clipToPlay);
    }
}