using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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
	public Action<int> onBossDied;

	[SerializeField] private MissleSpawner missleSpanwer;
	private Blackboard blackboard;
	private BaseNode tree;
	private BaseNode currentNode;
	private Animator animator;
	private Player player;

	[SerializeField] private List<Collider2D> bossColliders = new List<Collider2D>();

	private void Awake()
	{
		animator = GetComponentInChildren<Animator>();
		player = FindObjectOfType<Player>();
	}

	private void Start()
	{
		Health = MaxHealth;
		blackboard = new Blackboard();
		blackboard.SetVariable(VariableNames.PlayerTransform, player.transform);
		blackboard.SetVariable(VariableNames.BossColliders, bossColliders);
		blackboard.SetVariable(VariableNames.BossHealth, Health);
		blackboard.SetVariable(VariableNames.PlayerIsGrounded, player.isGrounded);
		blackboard.SetVariable(VariableNames.PlayerHealth, player.Health);
		missleSpanwer.SetupBlackBoard(blackboard);

		tree = new RandomSelectorNode(
			() => true, 
			new BaseNode[] {
				new SelectorNode(
					() => blackboard.GetVariable<bool>(VariableNames.PlayerIsGrounded),
					new BossFaseNode(animator, 4, bossColliders[0], 1) //in air fase
				 ),
				new RandomSelectorNode(
					() => true,
				    new BaseNode[] {
						new IsPlayerXPosNode(-9, -5),
						new BossFaseNode(animator, 2, bossColliders[2], 1), //Left fase
						new SelectorNode(
							() => blackboard.GetVariable<Transform>(VariableNames.PlayerTransform).position.x >= 5 &&
								  blackboard.GetVariable<Transform>(VariableNames.PlayerTransform).position.x <= 9,
							new BossFaseNode(animator, 3, bossColliders[3], 1), //right fase
							new BossFaseNode(animator, 1, bossColliders[1], 1) //center fase
						)
					}
				),
				new BossFaseNode(animator, 0, bossColliders[0], 10) // idle fase
			}
		);

		tree.SetupBlackboard(blackboard);
	}

	private void FixedUpdate()
	{
		blackboard.SetVariable(VariableNames.BossHealth, health);
		blackboard.SetVariable(VariableNames.PlayerIsGrounded, player.isGrounded);
		blackboard.SetVariable(VariableNames.PlayerHealth, player.Health);

		currentNode = tree;
		NodeStatus result = tree.Processing();
		Debug.Log($"Current Node: {currentNode.GetNodeName()}, {currentNode.GetNodeType()}, Tree Result: {result}");
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