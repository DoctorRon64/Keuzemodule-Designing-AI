using UnityEngine;

public class IsPlayerInAirCondition : BaseNode
{
	private bool IsGrounded;

	public IsPlayerInAirCondition()
	{
	}

	protected override NodeStatus OnUpdate()
	{
		IsGrounded = blackboard.GetVariable<bool>(VariableNames.PlayerIsGrounded);
		if (IsGrounded) { return NodeStatus.Failed; }
		return NodeStatus.Success;
	}

	protected override void OnEnter()
	{
		IsGrounded = false;
	}
}
