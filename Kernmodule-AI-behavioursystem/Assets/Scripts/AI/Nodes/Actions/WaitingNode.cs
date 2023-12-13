using UnityEngine;

public class WaitingNode : BaseNode
{
	private float waitDuration; //in Seconds
	private float elapsedTime;

	public WaitingNode(float _waitDuration)
	{
		this.waitDuration = _waitDuration;
		this.elapsedTime = 0f;
	}

	protected override NodeStatus OnUpdate()
	{
		if (elapsedTime >= waitDuration)
		{
			return NodeStatus.Success;
		}
		if (elapsedTime < 0f)
		{
			return NodeStatus.Running;
		}

		elapsedTime += Time.deltaTime;

		return NodeStatus.Running;
	}

	protected override void OnEnter()
	{
		elapsedTime = 0f;
	}
}
