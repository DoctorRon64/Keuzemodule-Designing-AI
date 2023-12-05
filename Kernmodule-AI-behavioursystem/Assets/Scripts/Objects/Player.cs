using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour
{
	[Header("Shooting")]
	[SerializeField] private GameObject bulletPrefab;
	[SerializeField] private Transform shootingPoint;
	[SerializeField] private int bulletAmount = 10;
	[SerializeField] private float bulletSpawnDistance = 1.0f;
	[SerializeField] private float bulletSpeed = 10f;

	[Header("Movement")]
	[SerializeField] private float moveSpeed = 5f;
	[SerializeField] private float jumpForce = 10f;
	[SerializeField] private List<KeyCode> keys;

	private ObjectPool<Bullet> bulletPool;
	private Rigidbody2D rb;
	private bool isGrounded;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
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
		MovePlayer();
		HandleActions();
	}

	void MovePlayer()
	{
		float horizontalInput = Input.GetAxis("Horizontal");
		Vector2 moveDirection = new Vector2(horizontalInput, 0);
		rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y);
	}

	void HandleActions()
	{
		if (Input.GetKeyDown(keys[0]) && isGrounded)
		{
			rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
		}

		if (bulletPool != null && Input.GetKeyDown(keys[1]))
		{
			ShootBullet();
		}
	}

	void ShootBullet()
	{
		Vector3 mousePosition = Input.mousePosition;
		mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

		Vector2 direction = (mousePosition - transform.position).normalized;
		float angle = Mathf.Atan2(direction.y, direction.x);
		float shootingPointX = Mathf.Cos(angle) * bulletSpawnDistance;
		float shootingPointY = Mathf.Sin(angle) * bulletSpawnDistance;
		shootingPoint.position = new Vector3(transform.position.x + shootingPointX, transform.position.y + shootingPointY, 0f);

		Bullet bullet = bulletPool.RequestObject(shootingPoint.position) as Bullet;
		if (bullet != null)
		{
			bullet.SetDirection(direction, bulletSpeed);
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.TryGetComponent<Ground>(out Ground _ground))
		{
			isGrounded = true;
		}
	}

	void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.TryGetComponent<Ground>(out Ground _ground))
		{
			isGrounded = false;
		}
	}
}
