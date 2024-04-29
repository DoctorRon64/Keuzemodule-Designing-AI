using UnityEngine;
public class Bullet : MonoBehaviour, IPoolable
{
	private Rigidbody2D rb;
	private ObjectPool<Bullet> objectPool;
	public bool Active { get; set; }
	private int damageValue = 1;
	public void SetupBullet(ObjectPool<Bullet> _pool)
	{
		rb = GetComponent<Rigidbody2D>();
		objectPool = _pool;
	}

	public void SetDirection(Vector2 _direction, float _speed)
	{
		rb.velocity = _direction.normalized * _speed;
	}

	public void SetRotation(Vector2 direction)
	{
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}

	void OnTriggerEnter2D(Collider2D _other)
	{
		if (!_other.gameObject.TryGetComponent<IShootable>(out IShootable _shootable)) return;
		DisablePoolable();
		objectPool.DeactivateItem(this);
	}

	public void SetPosition(Vector2 _position)
	{
		transform.position = _position;
	}

	public void DisablePoolable()
	{
		rb.velocity = Vector2.zero;
		gameObject.SetActive(false);
	}

	public void EnablePoolable()
	{
		SetDirection(Vector2.left, 10f);
		gameObject.SetActive(true);
	}
}