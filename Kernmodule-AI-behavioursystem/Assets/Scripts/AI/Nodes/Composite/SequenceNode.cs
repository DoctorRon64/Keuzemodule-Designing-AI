public class SequenceNode : Composite
{
	public SequenceNode(params BaseNode[] children) : base(children)
	{
	}

	protected override NodeStatus OnUpdate()
	{
		for (int i = 0; i < children.Length; i++)
		{
			NodeStatus result = children[i].Tick();
			switch (result)
			{
				case NodeStatus.Success: continue;
				case NodeStatus.Failed: return NodeStatus.Failed;
				case NodeStatus.Running: continue;
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
