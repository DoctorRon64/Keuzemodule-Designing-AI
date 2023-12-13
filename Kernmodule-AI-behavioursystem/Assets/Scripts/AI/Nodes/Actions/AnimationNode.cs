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
		Debug.Log($"Checking {GetNodeName()} condition. Current Animation Phase: {currentAnimationPhase}, Target Animation Phase: {animationFase}");

		if (currentAnimationPhase == animationFase)
		{
			OnReset();
			return NodeStatus.Success;
		}
		else
		{
			OnReset();
			return NodeStatus.Failed;
		}
	}

	protected override void OnEnter()
	{
		animator.SetInteger(VariableNames.FaseAnimations, animationFase);
	}

	public override string GetNodeType()
	{
		return $"{NodeName}-{animationFase}";
	}
}