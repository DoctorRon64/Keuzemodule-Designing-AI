using UnityEngine;

public class BossAttacker : MonoBehaviour
{
	[SerializeField] private int damageAmount = 2;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.TryGetComponent<Player>(out Player _damagable))
		{
			_damagable.TakeDamage(damageAmount);
		}
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player _damagable))
		{
			_damagable.TakeDamage(damageAmount);
		}
    }
}