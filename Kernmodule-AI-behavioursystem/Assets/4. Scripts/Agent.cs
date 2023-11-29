using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour, IDamagable
{
    private NavMeshAgent agent;
    public float Health { get; set; }
    
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        BlackBoard blackboard = new BlackBoard();
    }

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
