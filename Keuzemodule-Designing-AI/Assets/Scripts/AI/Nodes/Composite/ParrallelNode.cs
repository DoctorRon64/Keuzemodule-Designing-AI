using System;

public class ParrallelNode : Composite
{
	public ParrallelNode(params BaseNode[] children) : base(children)
	{
	}

	protected override NodeStatus OnUpdate()
	{
		bool allChildrenSucceeded = true;
		bool anyChildRunning = false;

		foreach (BaseNode child in children)
		{
			NodeStatus childStatus = child.Tick();
			switch(childStatus)
			{
				case NodeStatus.Success: break;
				case NodeStatus.Failed: allChildrenSucceeded = false; break;
				case NodeStatus.Running: anyChildRunning = true; break;
			}
		}
		if (anyChildRunning) { return NodeStatus.Running; }
		if (allChildrenSucceeded) { return NodeStatus.Success; }
		return NodeStatus.Failed;
	}

	public override void OnReset()
	{
		base.OnReset();
		foreach (var c in children)
		{
			c.OnReset();
		}
	}
}
