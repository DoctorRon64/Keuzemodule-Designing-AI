using UnityEngine;

public class IsPlayerXPosNode : BaseNode
{
	private float xMin, xMax;

	public IsPlayerXPosNode(float xMin, float xMax) : base()
	{
		this.xMin = xMin;
		this.xMax = xMax;
	}

	protected override NodeStatus OnUpdate()
	{
		Transform playerTransform = blackboard.GetVariable<Transform>(VariableNames.PlayerTransform);
		Vector3 playerPosition = playerTransform.position;

		if (playerPosition.x >= xMin && playerPosition.x <= xMax)
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