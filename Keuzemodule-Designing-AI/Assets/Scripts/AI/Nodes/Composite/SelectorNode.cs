using System;
public class SelectorNode : Composite
{
	public SelectorNode(params BaseNode[] children) : base(children)
	{
	}

	protected override NodeStatus OnUpdate()
	{
		for (int i = 0; i < children.Length; i++)
		{
			NodeStatus result = children[i].Tick();
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