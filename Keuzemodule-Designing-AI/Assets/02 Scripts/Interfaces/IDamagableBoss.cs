public interface IDamagableBoss
{
    int Health { get; set; }
    void TakeDamage(int _damageAmount);
}