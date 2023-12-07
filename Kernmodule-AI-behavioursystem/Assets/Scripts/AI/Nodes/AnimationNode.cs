using UnityEngine;

public class AnimationNode : BaseNode
{
    private Animator animator;
	private int animationFase;

    public AnimationNode(Animator _animator, int _animationFase, Blackboard _blackBoard) : base(_blackBoard)
	{
		this.animator = _animator;
		this.animationFase = _animationFase;
	}

	protected override NodeStatus Status()
	{
		int currentAnimationPhase = animator.GetInteger(VariableNames.FaseAnimations);
		if (currentAnimationPhase == animationFase)
		{
			return NodeStatus.Success;
		}
		else
		{
			return NodeStatus.Running;
		}
	}

	protected override void OnEnter()
	{
		animator.SetInteger(VariableNames.FaseAnimations, animationFase);
	}
}
