using UnityEngine;

public class BossAttacker : MonoBehaviour, IShootable
{
	[SerializeField] private int damageAmount = 2;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		IDamagable damagePlayer = collision.gameObject.GetComponent<IDamagable>();
		if (damagePlayer != null )
		{
			damagePlayer.TakeDamage(damageAmount);
		}
	}
}