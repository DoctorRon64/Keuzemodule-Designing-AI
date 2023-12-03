using UnityEngine;

public class Player : MonoBehaviour
{
	public float moveSpeed = 5f;
	public float jumpForce = 10f;
	public Transform shootPoint;
	public ObjectPool<Bullet> bulletPool;

	private Rigidbody2D rb;
	private bool isGrounded;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		bulletPool = new ObjectPool<Bullet>();
	}

	void Update()
	{
		MovePlayer();
		HandleJump();
		HandleShoot();
	}

	void MovePlayer()
	{
		float horizontalInput = Input.GetAxis("Horizontal");
		Vector2 moveDirection = new Vector2(horizontalInput, 0);
		rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y);
	}

	void HandleJump()
	{
		if (Input.GetButtonDown("Jump") && isGrounded)
		{
			rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
		}
	}

	void HandleShoot()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			Bullet bullet = (Bullet)bulletPool.RequestObject(shootPoint.position);
			if (bullet != null)
			{
				bullet.gameObject.SetActive(true);
			}
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Ground"))
		{
			isGrounded = true;
		}
	}

	void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Ground"))
		{
			isGrounded = false;
		}
	}
}
