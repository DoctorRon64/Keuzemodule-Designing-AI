public class RandomSelectorNode : Composite
{
	public RandomSelectorNode(params BaseNode[] children) : base(children)
	{
	}

	protected override NodeStatus OnUpdate()
	{
		ShuffleChildren();

		foreach (BaseNode child in children)
		{
			NodeStatus result = child.Tick();
			switch (result)
			{
				case NodeStatus.Success: return NodeStatus.Success;
				case NodeStatus.Failed: continue;
				case NodeStatus.Running: return NodeStatus.Running;
			}
		}
		return NodeStatus.Success;
	}

	private void ShuffleChildren()
	{
		for (int i = 0; i < children.Length; i++)
		{
			int randomIndex = UnityEngine.Random.Range(i, children.Length);
			BaseNode tempChild = children[i];
			children[i] = children[randomIndex];
			children[randomIndex] = tempChild;
		}
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
