using System;
using UnityEngine;

public class BossAgent : MonoBehaviour, IDamagable, IShootable
{
	public int MaxHealth = 5000;
	private int health;
	public int Health
	{
		get { return health; }
		set
		{
			if (health != value)
			{
				health = value;
				OnHealthChanged(health);
			}
		}
	}
	public Action<int> onHealthChanged;

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
		Health = MaxHealth;
		animator.SetInteger(VariableNames.FaseAnimations, 0);

		//Create your Behaviour Tree here!
		Blackboard blackboard = new Blackboard();
		blackboard.SetVariable(VariableNames.BossHealth, Health);
		blackboard.SetVariable(VariableNames.PlayerPosition, player.transform.position);
		blackboard.SetVariable(VariableNames.PlayerHealth, player.Health);

		//tree =
		//    new BTRepeater(wayPoints.Length,
		//        new BTSequence(
		//            new BTGetNextPatrolPosition(wayPoints),
		//            new BTMoveToPosition(agent, moveSpeed, VariableNames.TARGET_POSITION, keepDistance)
		//           )
		//    );
		//
		//tree.SetupBlackboard(blackboard);
	}

	protected virtual void OnHealthChanged(int newHealth)
	{
		onHealthChanged?.Invoke(newHealth);
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