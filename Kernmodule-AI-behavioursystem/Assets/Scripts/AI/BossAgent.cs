using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BossAgent : MonoBehaviour, IDamagable, IShootable
{
    public int Health { get; set; } = 1000;
    private BaseNode tree;
    private Animator animator;
    private Player player;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        //Create your Behaviour Tree here!
        Blackboard blackboard = new Blackboard();
        blackboard.SetVariable(VariableNames.BossHealth, Health);
        blackboard.SetVariable(VariableNames.PlayerPosition, player.transform.position);
        blackboard.SetVariable(VariableNames.PlayerHealth, player.Health);

        /*tree =
            new BTRepeater(wayPoints.Length,
                new BTSequence(
                    new BTGetNextPatrolPosition(wayPoints),
                    new BTMoveToPosition(agent, moveSpeed, VariableNames.TARGET_POSITION, keepDistance)
                   )
            );*/

        //tree.SetupBlackboard(blackboard);
    }

    public void TakeDamage(int _damage)
    {
        Health -= _damage;
        if (Health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {

    }
}