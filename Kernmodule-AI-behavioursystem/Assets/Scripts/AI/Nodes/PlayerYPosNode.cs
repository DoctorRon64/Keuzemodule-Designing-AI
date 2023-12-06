using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerYPosNode : BaseNode
{
	private Transform playerTransform;
	private float yMin, yMax;

	public PlayerYPosNode(Transform playerTransform, float yMin, float yMax)
	{
		this.playerTransform = playerTransform;
		this.yMin = yMin;
		this.yMax = yMax;
	}

	protected override NodeStatus Status()
	{
		Vector3 playerPosition = blackBoard.GetVariable<Vector3>(VariableNames.PlayerPosition);

		if (playerPosition.y >= yMin && playerPosition.y <= yMax)
		{
			return NodeStatus.Success;
		}
		else
		{
			return NodeStatus.Failed;
		}
	}
}
