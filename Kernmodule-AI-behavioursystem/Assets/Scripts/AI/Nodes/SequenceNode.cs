public class SequenceNode : BaseNode
{
    private int currentChildIndex = 0;

	public SequenceNode(Blackboard _blackBoard) : base(_blackBoard)
	{

	}

	protected override NodeStatus Status()
    {
        for(; currentChildIndex < childerenNodes.Count; currentChildIndex++)
        {
            NodeStatus result = childerenNodes[currentChildIndex].Processing();
            switch (result)
            {
                case NodeStatus.Success: continue;
                case NodeStatus.Running: return NodeStatus.Running;
                case NodeStatus.Failed: return NodeStatus.Failed;
            }
        }
        return NodeStatus.Success;
    }

	protected override void OnEnter() { currentChildIndex = 0; }
	protected override void OnExit() { currentChildIndex = 0; }

	public override void OnReset()
	{
		currentChildIndex = 0;
        foreach (BaseNode child in childerenNodes)
        { 
            child.OnReset(); 
        }
	}
}
