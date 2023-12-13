using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossAgent : MonoBehaviour, IDamagableBoss, IShootable
{
	[Header("Health")]
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

	[Header("Objects")]
	[SerializeField] private MissileSpawner missleSpanwer;
	[SerializeField] private Transform player;
	private Blackboard blackboard = new Blackboard();
	private RandomSelectorNode tree;
	private Animator animator;
	private Player playerScript;

	[Header("BossColliders")]
	[SerializeField] private List<Collider2D> bossColliders = new List<Collider2D>();

	[Header("FaseDurations")]
	[SerializeField] private List<float> durationFase = new List<float> { 3, 5, 10 };

	private void Awake()
	{
		playerScript = player.GetComponent<Player>();
		animator = GetComponent<Animator>();
	}

	private void Start()
	{
		blackboard.SetVariable(VariableNames.PlayerTransform, player);
		missleSpanwer.SetupBlackBoard(blackboard);
		blackboard.SetVariable(VariableNames.BossColliders, bossColliders);
		blackboard.SetVariable(VariableNames.BossHealth, Health);
		blackboard.SetVariable(VariableNames.PlayerIsGrounded, playerScript.isGrounded);
		blackboard.SetVariable(VariableNames.PlayerHealth, playerScript.Health);
		Health = MaxHealth;

		tree = new RandomSelectorNode(
			new BaseNode[]
			{
				//idle
				new ParrallelNode(
					new ColliderNode(bossColliders[0]),
					new AnimationNode(animator, 0),
					new WaitingNode(durationFase[0])
				),

				//InAir
				new SequenceNode(
					new IsPlayerInAirCondition(),
					new ParrallelNode(
						new ColliderNode(bossColliders[0]),
						new AnimationNode(animator, 4),
						new WaitingNode(durationFase[2])
					)
				),

				//OnGround
				new SelectorNode(
					//Left
					new SequenceNode(
						new IsObjectInRangeOf(blackboard.GetVariable<Transform>(VariableNames.PlayerTransform), -9.0f, -5.0f),
						new ParrallelNode(
							new ColliderNode(bossColliders[2]),
							new AnimationNode(animator, 2),
							new WaitingNode(durationFase[1])
						)
					),
					//Center
					new SequenceNode(
						new IsObjectInRangeOf(blackboard.GetVariable<Transform>(VariableNames.PlayerTransform), -2.0f, 2.0f),
						new ParrallelNode(
							new ColliderNode(bossColliders[1]),
							new AnimationNode(animator, 1),
							new WaitingNode(durationFase[1])
						)
					),
					//Right
					new SequenceNode(
						new IsObjectInRangeOf(blackboard.GetVariable<Transform>(VariableNames.PlayerTransform), 5.0f, 9.0f),
						new ParrallelNode(
							new ColliderNode(bossColliders[3]),
							new AnimationNode(animator, 3),
							new WaitingNode(durationFase[1])
						)
					)
				)
			}
		);
		tree.SetupBlackboard(blackboard);
	}

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