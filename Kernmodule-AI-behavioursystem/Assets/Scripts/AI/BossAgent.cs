using System;
using System.Collections.Generic;
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


		Blackboard blackboard = new Blackboard();
		blackboard.SetVariable(VariableNames.BossHealth, Health);
		blackboard.SetVariable(VariableNames.PlayerPosition, player.transform.position);
		blackboard.SetVariable(VariableNames.PlayerIsGrounded, player.isGrounded);
		blackboard.SetVariable(VariableNames.PlayerHealth, player.Health);

		//Add Nodes
		AnimationNode FaseIdle = new AnimationNode(animator, 1);
		AnimationNode FaseMissle = new AnimationNode(animator, 2);
		AnimationNode FaseLeft = new AnimationNode(animator, 3);
		AnimationNode FaseRight = new AnimationNode(animator, 4);

		SelectorNode rootSelector = new SelectorNode();
		SelectorNode secondSelector = new SelectorNode();

		IsGroundNode isGroundNode = new IsGroundNode(blackboard.GetVariable<bool>(VariableNames.PlayerIsGrounded));
		IsGroundNode isAirNode = new IsGroundNode(!blackboard.GetVariable<bool>(VariableNames.PlayerIsGrounded));
		PlayerXPosNode isPlayerLeft = new PlayerXPosNode(player.transform, -9, -5);
		PlayerXPosNode isPlayerRight = new PlayerXPosNode(player.transform, 5, 9);
		PlayerXPosNode isPlayerCenter = new PlayerXPosNode(player.transform, -2, 2);

		//link notes
		FaseIdle.referenceChildren(rootSelector);
		rootSelector.referenceChildren(new List<BaseNode>() { isGroundNode, isAirNode });
		isAirNode.referenceChildren(FaseMissle);
		isGroundNode.referenceChildren(secondSelector);
		secondSelector.referenceChildren(new List<BaseNode>() { isPlayerLeft, isPlayerRight, isPlayerCenter });

		// setup tree
		rootSelector.SetupBlackboard(blackboard);
		tree = rootSelector;
	}

	private void FixedUpdate()
	{
		NodeStatus result = tree.Processing();
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