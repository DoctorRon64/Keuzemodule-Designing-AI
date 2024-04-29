using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamagable
{
    private Animator anim;
    private Transform playerTransform;
    private Camera mainCamera;

    [Header("Shooting")] [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private int bulletAmount = 10;
    [SerializeField] private float bulletSpawnDistance = 1.0f;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float fireRate = 0.2f;
    private ObjectPool<Bullet> bulletPool;
    private bool isShooting;
    private float nextFireTime;

    [Header("Movement")] [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private List<KeyCode> keys;
    private Rigidbody2D rb;
    public bool isGrounded;
    private bool isWallGliding = false;
    private bool isWalking = false;
    private Collider2D playerCollider;

    [Header("Health")] public int maxHealth = 50;
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
    public Action<int> onHealthChanged;

    //beter animation int to string
    private static readonly int walking = Animator.StringToHash("isWalking");
    private static readonly int shoot = Animator.StringToHash("Shoot");
    private static readonly int isWalkingShoot = Animator.StringToHash("isWalkingShoot");

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        anim.SetInteger("Player", 0);
        playerTransform = transform;
        playerCollider = GetComponent<Collider2D>();
        mainCamera = Camera.main;
        health = maxHealth;

        shootingPoint.localPosition = new Vector3(bulletSpawnDistance, 0f, 0f);

        bulletPool = new ObjectPool<Bullet>(bulletPrefab.GetComponent<Bullet>());
        for (int i = 0; i < bulletAmount; i++)
        {
            Bullet bullet = (Bullet)bulletPool.AddNewItemToPool();
            bullet.SetupBullet(bulletPool);
        }
    }

    void Update()
    {
        HandleActions();
        UpdateAnimation();
    }

    void FixedUpdate()
    {
        MovePlayer();

        if (isShooting && Time.time > nextFireTime)
        {
            ShootBullet();
        }
    }

    protected virtual void InvokeNewHealth(int newHealth)
    {
        onHealthChanged?.Invoke(newHealth);
    }

    void MovePlayer()
    {
        if (!isWallGliding || isGrounded)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
            isWalking = (horizontalInput != 0);
            anim.SetBool(walking, horizontalInput != 0);
        }
    }

    void HandleActions()
    {
        if (Input.GetKeyDown(keys[0]) && isGrounded)
        {
            Jump();
        }

        isShooting = Input.GetKey(keys[1]);
    }

    void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    void ShootBullet()
    {
        nextFireTime = Time.time + fireRate;
        anim.SetTrigger(shoot);

        Vector3 mousePosition = Input.mousePosition;
        mousePosition = mainCamera.ScreenToWorldPoint(mousePosition);

        Vector2 direction = (mousePosition - playerTransform.position).normalized;
        Vector3 shootingPointPosition = playerTransform.position + (Vector3)direction * bulletSpawnDistance;

        shootingPointPosition = playerTransform.position +
                                (shootingPointPosition - playerTransform.position).normalized * bulletSpawnDistance;
        shootingPoint.position = shootingPointPosition;

        Bullet bullet = bulletPool.RequestObject(shootingPoint.position) as Bullet;
        if (bullet != null)
        {
            bullet.SetDirection(direction, bulletSpeed);
            bullet.SetRotation(direction);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out Wall wall))
        {
            isWallGliding = true;
        }

        if (collision.collider.TryGetComponent(out Ground _ground) ||
            collision.collider.TryGetComponent(out Platform _platform))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out Wall wall))
        {
            //isWallGliding = false;
        }

        if (collision.collider.TryGetComponent(out Ground _ground) ||
            collision.collider.TryGetComponent(out Platform _platform))
        {
            isGrounded = false;
        }
    }

    public void TakeDamage(int _damage)
    {
        Health -= _damage;
        if (Health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnPlayerDied?.Invoke(2);
    }

    void UpdateAnimation()
    {
        if (isWalking && isShooting)
            anim.SetBool(isWalkingShoot, (isWalking && isShooting));
        else
            anim.SetBool(isWalkingShoot, false);
    }
}