public interface IDamagable
{
	int Health { get; set; }
	void TakeDamage(int damageAmount);
	void Die();
}
