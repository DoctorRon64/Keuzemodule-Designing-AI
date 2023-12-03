using UnityEngine;

public class Bullet : MonoBehaviour, IPoolable
{
	private Rigidbody2D rb;
	private ObjectPool<Bullet> objectPool;
	public bool Active { get; set; }

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	public void SetObjectPool(ObjectPool<Bullet> _pool)
	{
		objectPool = _pool;
	}

	public void SetDirection(Vector2 _direction)
	{
		rb.velocity = _direction.normalized * 10f;
		Debug.Log(_direction.normalized);
	}

	void OnTriggerEnter2D(Collider2D _other)
	{
		Debug.Log("Bullet OnTriggerEnter2D");
		DisablePoolabe();
		objectPool.DeactivateItem(this);
	}

	public void SetPosition(Vector2 _position)
	{
		transform.position = _position;
	}

	public void DisablePoolabe()
	{
		gameObject.SetActive(false);
		rb.velocity = Vector2.zero;
	}

	public void EnablePoolabe()
	{
		gameObject.SetActive(true);
		rb.velocity = Vector2.right * 10f;
	}
}
