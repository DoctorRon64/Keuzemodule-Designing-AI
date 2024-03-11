public enum NodeStatus { Success, Failed, Running }
public abstract class Node
{
	protected Blackboard blackboard;
	private bool wasEntered = false;
    protected bool isActive = false;
    public string NodeName { get; protected set; }
	public virtual void OnReset() 
	{
		wasEntered = false;
	}

    public NodeStatus Tick()
    {
        if (!wasEntered)
        {
            OnEnter();
            wasEntered = true;
        }

        NodeStatus result2 = OnUpdate();
        if (result2 != NodeStatus.Running)
        {
            OnExit();
            wasEntered = false;
        }
        return result2;
    }

    protected virtual string GetNodeName()
	{
		return this.ToString();
	}

	public virtual void SetupBlackboard(Blackboard _blackboard)
	{
		this.blackboard = _blackboard;
	}

	protected abstract NodeStatus OnUpdate();
	protected virtual void OnEnter() 
	{
		blackboard.SetVariable(VariableNames.BossCurrentNode, $"{GetNodeName()}, Result: {wasEntered}");
	}
	protected virtual void OnExit() { }

}

public abstract class Composite : Node
{
	protected Node[] children;

	protected Composite(params Node[] children)
	{
		this.children = children;
	}

	public override void SetupBlackboard(Blackboard _blackboard)
	{
		base.SetupBlackboard(_blackboard);
		foreach (Node node in children)
		{
			node.SetupBlackboard(_blackboard);
		}
	}
}