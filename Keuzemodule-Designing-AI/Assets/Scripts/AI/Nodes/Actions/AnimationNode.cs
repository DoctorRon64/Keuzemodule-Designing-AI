using UnityEngine;

public class AnimationNode : BaseNode
{
	private Animator animator;
	private int animationFase;

	public AnimationNode(Animator _animator, int _animationFase)
	{
		this.animator = _animator;
		this.animationFase = _animationFase;
	}

	protected override NodeStatus OnUpdate()
	{
		int currentAnimationPhase = animator.GetInteger(VariableNames.FaseAnimations);
		if (currentAnimationPhase == animationFase)
		{
			return NodeStatus.Success;
		}
		else
		{
			return NodeStatus.Failed;
		}
	}

	protected override void OnEnter()
	{
		base.OnEnter();
		blackboard.SetVariable(VariableNames.BossCurrentNode, $"{GetNodeName()}, Result: {animationFase}");
		animator.SetInteger(VariableNames.FaseAnimations, animationFase);
	}
}