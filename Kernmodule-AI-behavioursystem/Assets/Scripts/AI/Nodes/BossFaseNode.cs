using System.Collections.Generic;
using UnityEngine;

public class BossFaseNode : BaseNode
{
	private Animator animator;
	private int animationFase;
	private Collider2D enabledCollider;
	private float activationTime;

	public BossFaseNode(Animator _animator, int _animationFase, Collider2D _colliderEnabled, float _activationTime)
	{
		this.enabledCollider = _colliderEnabled;
		this.animator = _animator;
		this.animationFase = _animationFase;
		this.activationTime = _activationTime;
	}

	protected override NodeStatus OnUpdate()
	{
		int currentAnimationPhase = animator.GetInteger(VariableNames.FaseAnimations);
        Debug.Log($"Checking {GetNodeName()} condition. Current Animation Phase: {currentAnimationPhase}, Target Animation Phase: {animationFase}");

		if (currentAnimationPhase == animationFase)
		{
			OnReset();
			return NodeStatus.Success;
		}
		else if (Time.time - activationTime > 1f)
		{
			OnReset();
			return NodeStatus.Failed;
		}
		return NodeStatus.Running;
	}

	protected override void OnEnter()
	{
		animator.SetInteger(VariableNames.FaseAnimations, animationFase);
		List<Collider2D> allColliders = blackboard.GetVariable<List<Collider2D>>(VariableNames.BossColliders);

		foreach (var collider in allColliders)
		{
			collider.enabled = collider == enabledCollider;
		}
	}

	public override string GetNodeType()
	{
		return $"{NodeName}-{animationFase}";
	}
}
