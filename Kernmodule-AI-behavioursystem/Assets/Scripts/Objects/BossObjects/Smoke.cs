using UnityEngine;

public class Smoke : BossAttacker, IDamagableBoss
{
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
	}
}
