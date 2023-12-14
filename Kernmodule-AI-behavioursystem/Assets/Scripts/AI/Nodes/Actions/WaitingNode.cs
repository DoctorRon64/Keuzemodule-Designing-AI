using UnityEngine;

public class WaitingNode : BaseNode
{
	private float waitDuration; //in Seconds
	private float startTime;
	private bool isWaiting;

	public WaitingNode(float _waitDuration)
	{
		this.waitDuration = _waitDuration;
		this.startTime = 0f;
		this.isWaiting = false;
	}

	protected override NodeStatus OnUpdate()
	{
		if (!isWaiting)
		{
			startTime = Time.time;
			isWaiting = true;
		}

		float elapsedTime = Time.time - startTime;
		Debug.Log(elapsedTime);

		if (elapsedTime >= waitDuration)
		{
			isWaiting = false;
			return NodeStatus.Success;
		}

		return NodeStatus.Running;
	}

	protected override void OnEnter()
	{
		isWaiting = false;
		startTime = Time.time;
	}

	protected override void OnExit()
	{
		isWaiting = false;
	}
}
