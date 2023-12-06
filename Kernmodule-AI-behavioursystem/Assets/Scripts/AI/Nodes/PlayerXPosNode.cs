using UnityEngine;

public class PlayerXPosNode : BaseNode
{
	private Transform playerTransform;
	private float xMin, xMax;

	public PlayerXPosNode(Transform playerTransform, float xMin, float xMax)
	{
		this.playerTransform = playerTransform;
		this.xMin = xMin;
		this.xMax = xMax;
	}

	protected override NodeStatus Status()
	{
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
