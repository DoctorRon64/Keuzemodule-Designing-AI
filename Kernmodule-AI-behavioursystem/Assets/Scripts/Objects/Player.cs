using System;
using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour, IDamagable
{
	[Header("Shooting")]
	[SerializeField] private GameObject bulletPrefab;
	[SerializeField] private Transform shootingPoint;
	[SerializeField] private int bulletAmount = 10;
	[SerializeField] private float bulletSpawnDistance = 1.0f;
	[SerializeField] private float bulletSpeed = 10f;
	[SerializeField] private float fireRate = 0.2f;
	private ObjectPool<Bullet> bulletPool;
	private bool isShooting;
	private float nextFireTime;

	[Header("Movement")]
	[SerializeField] private float moveSpeed = 5f;
	[SerializeField] private float jumpForce = 10f;
	[SerializeField] private List<KeyCode> keys;
	private Rigidbody2D rb;
	public bool isGrounded;
	private bool isWallGliding = false;

	[Header("Health")]
	public int MaxHealth = 20;
	public Action<int> onPlayerDied;
	private int health;
	public int Health
	{
		get { return health; }
		set
		{
			if (health != value)
			{
				health = value;
				OnHealthChanged(health);
			}
		}
	}
	public Action<int> onHealthChanged;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();

		shootingPoint.localPosition = new Vector3(bulletSpawnDistance, 0f, 0f);
		Health = MaxHealth;

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
	}

	void FixedUpdate()
	{
		MovePlayer();
		ShootBullet();
	}
	protected virtual void OnHealthChanged(int newHealth)
	{
		onHealthChanged?.Invoke(newHealth);
	}

	void MovePlayer()
	{
		if (!isWallGliding || isGrounded)
		{
			float horizontalInput = Input.GetAxis("Horizontal");
			Vector2 moveDirection = new Vector2(horizontalInput, 0);
			rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y);
		}
	}

	void HandleActions()
	{
		if (Input.GetKeyDown(keys[0]) && isGrounded) { Jump(); }
		isShooting = Input.GetKey(keys[1]);
	}

	void Jump()
	{
		rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
	}

	void ShootBullet()
	{
        if (isShooting && Time.time > nextFireTime)
		{
			nextFireTime = Time.time + fireRate;

			//get mouse pos
			Vector3 mousePosition = Input.mousePosition;
			mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

			//let it be around the player
			Vector2 direction = (mousePosition - transform.position).normalized;
			Vector3 shootingPointPosition = transform.position + (Vector3)direction * bulletSpawnDistance;

			// Ensure shooting point stays on the circumference of the circle
			shootingPointPosition = transform.position + (shootingPointPosition - transform.position).normalized * bulletSpawnDistance;
			shootingPoint.position = shootingPointPosition;

			//spawn bullet
			Bullet bullet = bulletPool.RequestObject(shootingPoint.position) as Bullet;
			if (bullet != null)
			{
				bullet.SetDirection(direction, bulletSpeed);
			}
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.TryGetComponent<Wall>(out Wall wall))
		{
			isWallGliding = true;
		}

		if (collision.gameObject.TryGetComponent<Ground>(out Ground _ground))
		{
			isGrounded = true;
		}

		if (collision.gameObject.TryGetComponent<Platform>(out Platform _platform))
		{
			isGrounded = true;
		}
	}

	void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.TryGetComponent<Wall>(out Wall wall))
		{
			isWallGliding = false;
		}

		if (collision.gameObject.TryGetComponent<Ground>(out Ground _ground))
		{
			isGrounded = false;
		}

		if (collision.gameObject.TryGetComponent<Platform>(out Platform _platform))
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
		onPlayerDied?.Invoke(2);
    }
}
