public enum NodeStatus { Success, Failed, Running }
public abstract class BaseNode
{
	protected Blackboard blackboard;
	protected bool wasEntered = false;
    protected bool isActive = false;
    public string NodeName { get; protected set; }
	public virtual void OnReset() 
	{
		wasEntered = false;
	}

    public NodeStatus Tick()
    {
        if (isActive)
        {
            var result = OnUpdate();
            if (result != NodeStatus.Running)
            {
                OnExit();
                wasEntered = false;
                isActive = false;
            }
            return result;
        }

        if (!wasEntered)
        {
            OnEnter();
            wasEntered = true;
            isActive = true;
        }

        var finalResult = OnUpdate();
        if (finalResult != NodeStatus.Running)
        {
            OnExit();
            wasEntered = false;
            isActive = false;
        }
        return finalResult;
    }

    public virtual string GetNodeName()
	{
		return this.ToString();
	}

	public virtual void SetupBlackboard(Blackboard blackboard)
	{
		this.blackboard = blackboard;
	}

	protected abstract NodeStatus OnUpdate();
	protected virtual void OnEnter() 
	{
		blackboard.SetVariable(VariableNames.BossCurrentNode, $"{GetNodeName()}, Result: {wasEntered}");
	}
	protected virtual void OnExit() { }

}

public abstract class Composite : BaseNode
{
	protected BaseNode[] children;
	public Composite(params BaseNode[] children)
	{
		this.children = children;
	}

	public override void SetupBlackboard(Blackboard blackboard)
	{
		base.SetupBlackboard(blackboard);
		foreach (BaseNode node in children)
		{
			node.SetupBlackboard(blackboard);
		}
	}
}