public interface IDamagable
{
	//most stuff that take damage such as the player
	int Health { get; set; }
	void TakeDamage(int damageAmount);
}
