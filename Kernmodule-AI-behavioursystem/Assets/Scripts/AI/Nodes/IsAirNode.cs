using UnityEngine;

public class IsAirNode : BaseNode
{
	public IsAirNode() : base()
	{

	}

	protected override NodeStatus OnUpdate()
	{
		bool isGrounded = blackboard.GetVariable<bool>(VariableNames.PlayerIsGrounded);

		if (!isGrounded)
		{
			return NodeStatus.Success;
		}
		else
		{
			OnReset();
			return NodeStatus.Failed;
		}
	}
}