using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    int Health { get; set; }

    void TakeDamage(int _damage);
    void Die();
}
