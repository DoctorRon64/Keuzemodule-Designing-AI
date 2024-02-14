using UnityEngine;

public class Smoke : MonoBehaviour, IDamagableBoss, IShootable
{
	[SerializeField] private int damageAmount = 1;
	public int Health { get; set; } = 2;

	public void Die()
	{
		Destroy(gameObject);
	}

	public void TakeDamage(int _damage)
	{
		Health -= _damage;
		if (Health <= 0)
		{
			Die();
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.TryGetComponent<Ground>(out Ground _ground))
		{
			Destroy(gameObject);
		}

		if (collision.TryGetComponent<Player>(out Player _player))
		{
			_player.TakeDamage(damageAmount);
		}
	}
}
