using UnityEngine;

public class PlayerXPosNode : BaseNode
{
	private Transform playerTransform;
	private float xMin, xMax;

	public PlayerXPosNode(float xMin, float xMax, Blackboard _blackBoard) : base(_blackBoard)
	{
		this.xMin = xMin;
		this.xMax = xMax;
	}

	protected override NodeStatus Status()
	{
		Transform playerTransform = blackBoard.GetVariable<Transform>(VariableNames.PlayerPosition);

		Vector3 playerPosition = playerTransform.position;

		if (playerPosition.x >= xMin && playerPosition.x <= xMax)
		{
			return NodeStatus.Success;
		}
		else
		{
			return NodeStatus.Failed;
		}
	}
}
