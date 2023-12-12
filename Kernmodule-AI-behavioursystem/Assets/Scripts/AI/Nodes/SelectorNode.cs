using System;
public class SelectorNode : Composite
{
	private Func<bool> condition;

	public SelectorNode(Func<bool> condition, params BaseNode[] children) : base(children)
	{
		this.condition = condition;
	}

	protected override NodeStatus OnUpdate()
	{
		for (var i = 0; i < children.Length; i++)
		{
			var result = children[i].Tick();
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