public interface IDamagableBoss
{
    //bossobjects that take damage
    int Health { get; set; }
    void TakeDamage(int _damageAmount);
}