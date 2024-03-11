public class SequenceNode : Composite
{
	private int currentChildIndex;

	public SequenceNode(params Node[] children) : base(children)
	{
		currentChildIndex = 0;
	}

	protected override NodeStatus OnUpdate()
	{
		for (int i = currentChildIndex; i < children.Length; i++)
		{
			NodeStatus result = children[i].Tick();
			switch (result)
			{
				case NodeStatus.Success: currentChildIndex++; break;
				case NodeStatus.Failed: OnReset(); return NodeStatus.Failed;
				case NodeStatus.Running: return NodeStatus.Running;
			}
		}
		OnReset();
		return NodeStatus.Success;
	}

	public override void OnReset()
	{
		foreach (var c in children)
		{
			c.OnReset();
		}
		currentChildIndex = 0;
	}

}
