using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossAgent : MonoBehaviour, IDamagableBoss, IShootable
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
	public Action<int> onBossDied;

	[SerializeField] private MissileSpawner missleSpanwer;
	[SerializeField] private Transform player;
	private Blackboard blackboard = new Blackboard();
	private RandomSelectorNode tree;
	private Animator animator;
	private Player playerScript;

	[SerializeField] private List<Collider2D> bossColliders = new List<Collider2D>();

	private void Awake()
	{
		playerScript = player.GetComponent<Player>();
		animator = GetComponentInChildren<Animator>();
	}

	private void Start()
	{
		blackboard.SetVariable(VariableNames.PlayerTransform, player);
		blackboard.SetVariable(VariableNames.BossColliders, bossColliders);
		blackboard.SetVariable(VariableNames.BossHealth, Health);
		blackboard.SetVariable(VariableNames.PlayerIsGrounded, playerScript.isGrounded);
		blackboard.SetVariable(VariableNames.PlayerHealth, playerScript.Health);
		missleSpanwer.SetupBlackBoard(blackboard);
		Health = MaxHealth;

		tree = new RandomSelectorNode(
			() => true,
			new BaseNode[] 
			{
				new SelectorNode(
					() => IsAirCondition(), //if player in air
					new BossFaseNode(animator, 4, bossColliders[0], 1) //in air fase
				),
				new RandomSelectorNode(
					() => true,
					new BaseNode[] 
					{
						new SelectorNode(
							() => IsPlayerInRange(-9, -5),
							new BossFaseNode(animator, 2, bossColliders[2], 1) //Left fase
						),
						new SelectorNode(
							() => IsPlayerInRange(-2, 2),
							new BossFaseNode(animator, 1, bossColliders[1], 1)  // center fase
						),
						new SelectorNode(
							() => IsPlayerInRange(5, 9),
							new BossFaseNode(animator, 3, bossColliders[3], 1) // right fase
						)
					}
				),
				new BossFaseNode(animator, 0, bossColliders[0], 1) // dle fase
			}
		);

		tree.SetupBlackboard(blackboard);
	}

	//conditions///----------------------------------------
	bool IsAirCondition()
	{
		return !blackboard.GetVariable<bool>(VariableNames.PlayerIsGrounded);
	}

	bool IsPlayerInRange(float min, float max)
	{
		Transform playerTransform = blackboard.GetVariable<Transform>(VariableNames.PlayerTransform);
		float xPosition = playerTransform.position.x;
		return xPosition >= min && xPosition <= max;
	}
	///--------------------------------------------

	private void FixedUpdate()
	{
		blackboard.SetVariable(VariableNames.BossHealth, health);
		blackboard.SetVariable(VariableNames.PlayerIsGrounded, playerScript.isGrounded);
		blackboard.SetVariable(VariableNames.PlayerHealth, playerScript.Health);

		NodeStatus result = tree.Tick();
		Debug.Log($"tree Node: {tree.GetNodeName()}, {tree.GetNodeType()}, Tree Result: {result}");
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
		onBossDied?.Invoke(1);
	}
}