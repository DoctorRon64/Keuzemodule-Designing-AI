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
		float elapsedTime = Time.time - startTime;

		if (elapsedTime >= waitDuration)
		{
			return NodeStatus.Success;
		}

		return NodeStatus.Running;
	}

	protected override void OnEnter()
	{
		base.OnEnter();
		blackboard.SetVariable(VariableNames.BossCurrentNode, $"{GetNodeName()}, Result: {startTime}");
		startTime = Time.time;
	}
}
