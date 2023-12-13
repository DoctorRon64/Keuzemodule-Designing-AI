using UnityEngine;

public class WaitingNode : BaseNode
{
	private float waitDuration; //in Seconds
	private float startTime;

	public WaitingNode(float _waitDuration)
	{
		this.waitDuration = _waitDuration;
		this.startTime = 0f;
	}

	protected override NodeStatus OnUpdate()
	{
		if (!wasEntered)
		{
			startTime = Time.time;
			wasEntered = true;
		}

		float elapsedTime = Time.time - startTime;
		Debug.Log(elapsedTime);

		if (elapsedTime >= waitDuration)
		{
			return NodeStatus.Success;
		}

		return NodeStatus.Running;
	}

	protected override void OnEnter()
	{
		wasEntered = false;
	}
}
