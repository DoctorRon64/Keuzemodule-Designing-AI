using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour, IDamagable
{
    public float Health { get; set; }
    public void TakeDamage(float _damage)
    {
        Health -= _damage;
        if (Health < 0 )
        {
            Die();
        }
    }
    private void Die()
    {

    }
}
