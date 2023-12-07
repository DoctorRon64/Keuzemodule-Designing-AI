public class SelectorNode : BaseNode
{
	public SelectorNode(Blackboard _blackBoard) : base(_blackBoard)
	{
	}

	protected override NodeStatus Status()
	{
        foreach (BaseNode child in childerenNodes)
        {
			NodeStatus result = child.Processing();
			switch (result)
			{
				case NodeStatus.Success: return NodeStatus.Success;
				case NodeStatus.Running: return NodeStatus.Running;
				case NodeStatus.Failed: continue;
			}
		}
        return NodeStatus.Success;
	}

	public override void OnReset()
	{
        foreach (BaseNode child in childerenNodes)
        {
            child.OnReset();
        }
	}
}
