using System.Diagnostics;

public enum NodeStatus { Success, Failed, Running }
public abstract class BaseNode
{
	protected Blackboard blackboard;
	protected bool wasEntered = false;
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

		var result = OnUpdate();
		if (result != NodeStatus.Running)
		{
			OnExit();
			wasEntered = false;
		}
		return result;
	}

	public virtual string GetNodeType()
	{
		return this.GetType().Name;
	}

	public virtual string GetNodeName()
	{
		return NodeName;
	}

	public virtual void SetupBlackboard(Blackboard blackboard)
	{
		this.blackboard = blackboard;
	}

	protected abstract NodeStatus OnUpdate();
	protected virtual void OnEnter() { }
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