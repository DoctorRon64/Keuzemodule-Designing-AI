using UnityEngine;

public class Smoke : BossAttacker
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.TryGetComponent<Ground>(out Ground _ground))
		{
			Destroy(gameObject);
		}
	}
}
