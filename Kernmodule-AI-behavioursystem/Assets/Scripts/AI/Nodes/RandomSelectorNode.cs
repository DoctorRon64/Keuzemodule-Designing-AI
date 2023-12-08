using System;

public class RandomSelectorNode : Composite
{
	private Func<bool> condition;

	public RandomSelectorNode(Func<bool> condition, params BaseNode[] children) : base(children)
	{
		this.condition = condition;
	}

	protected override NodeStatus OnUpdate()
	{
		if (condition.Invoke())
		{
			return NodeStatus.Success;
		}

		for (int i = 0; i < children.Length; i++)
		{
			int randomIndex = UnityEngine.Random.Range(i, children.Length);
			BaseNode randomChild = children[randomIndex];

			NodeStatus result = randomChild.Processing();

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
