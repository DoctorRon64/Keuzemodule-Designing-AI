using UnityEngine;

public class MissileController : MonoBehaviour, IDamagable
{
	[SerializeField] float speed = 5f;
	[SerializeField] float rotationSpeed = 10f;
	[SerializeField] int damage = 10;
	public Transform Player = null;

	public int Health { get; set; }

	private void Update()
	{
		if (Player != null)
		{
			Vector3 direction = (Player.position - transform.position).normalized;
			transform.Translate(direction * speed * Time.deltaTime, Space.World);

			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.TryGetComponent<Player>(out Player _player))
		{
			_player.TakeDamage(damage);
			Die();
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

	public void Die()
	{
		Destroy(gameObject);
	}
}
