using System;
public class SelectorNode : Composite
{
	public SelectorNode(params Node[] children) : base(children)
	{
	}

	protected override NodeStatus OnUpdate()
	{
		foreach (var t in children)
		{
			NodeStatus result = t.Tick();
			switch (result)
			{
				case NodeStatus.Success: return NodeStatus.Success;
				case NodeStatus.Failed: continue;
				case NodeStatus.Running: return NodeStatus.Running;
			}
		}

		return NodeStatus.Success;
	}

	public override void OnReset()
	{
		foreach (var c in children)
		{
			c.OnReset();
		}
	}
}