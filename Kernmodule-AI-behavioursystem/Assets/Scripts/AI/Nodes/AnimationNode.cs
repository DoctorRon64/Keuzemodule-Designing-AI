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

	protected override NodeStatus Status()
	{
		return NodeStatus.Running;
	}

	protected override void OnEnter()
	{
		animator.SetInteger(VariableNames.FaseAnimations, animationFase);
	}
}
