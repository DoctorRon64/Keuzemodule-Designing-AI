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

	[SerializeField] private Blackboard blackboard;
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

		//blackboard = new Blackboard();
		blackboard.SetVariable(VariableNames.BossHealth, Health);
		blackboard.SetVariable(VariableNames.PlayerPosition, player.transform.position);
		blackboard.SetVariable(VariableNames.PlayerIsGrounded, player.isGrounded);
		blackboard.SetVariable(VariableNames.PlayerHealth, player.Health);

		//Add Nodes
		SelectorNode rootSelector = new SelectorNode(blackboard);
		SelectorNode secondSelector = new SelectorNode(blackboard);

		AnimationNode FaseIdle = new AnimationNode(animator, 0, blackboard);
		AnimationNode FaseCenter = new AnimationNode(animator, 1, blackboard);
		AnimationNode FaseLeft = new AnimationNode(animator, 2, blackboard);
		AnimationNode FaseRight = new AnimationNode(animator, 3, blackboard);
		AnimationNode FaseMissle = new AnimationNode(animator, 4, blackboard);

		IsGroundNode isGroundNode = new IsGroundNode(blackboard);
		IsGroundNode isAirNode = new IsGroundNode(blackboard);
		PlayerXPosNode isPlayerLeft = new PlayerXPosNode(-9, -5, blackboard);
		PlayerXPosNode isPlayerRight = new PlayerXPosNode(5, 9, blackboard);
		PlayerXPosNode isPlayerCenter = new PlayerXPosNode(-2, 2, blackboard);

		//link notes
		FaseIdle.referenceChildren(rootSelector);
		rootSelector.referenceChildren(new List<BaseNode>() { isGroundNode, isAirNode });
		isAirNode.referenceChildren(FaseMissle);
		isGroundNode.referenceChildren(secondSelector);
		secondSelector.referenceChildren(new List<BaseNode>() { isPlayerLeft, isPlayerRight, isPlayerCenter });
		isPlayerLeft.referenceChildren(FaseLeft);
		isPlayerRight.referenceChildren(FaseRight);
		isPlayerCenter.referenceChildren(FaseCenter);

		// setup tree
		//rootSelector.SetupBlackboard(blackboard);
		tree = FaseIdle;
	}

	private void FixedUpdate()
	{
		blackboard.SetVariable(VariableNames.BossHealth, Health);
		blackboard.SetVariable(VariableNames.PlayerPosition, player.transform.position);
		blackboard.SetVariable(VariableNames.PlayerIsGrounded, player.isGrounded);
		blackboard.SetVariable(VariableNames.PlayerHealth, player.Health);

		NodeStatus result = tree.Processing();
		Debug.Log(result);
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