using System.Collections.Generic;

public enum NodeStatus { Success, Failed, Running }
public abstract class BaseNode
{
    public List<BaseNode> childerenNodes = null;
    private bool wasNodeEntered = false;
    protected Blackboard blackBoard;

	public BaseNode(Blackboard _blackBoard)
	{
		this.blackBoard = _blackBoard;
		if (childerenNodes != null)
		{
			foreach (BaseNode node in childerenNodes)
			{
				node.SetupBlackboard(_blackBoard);
			}
		}
	}

	public NodeStatus Processing()
    {
        if (!wasNodeEntered)
        {
            OnEnter();
            wasNodeEntered = true;
        }

        NodeStatus result = Status();
        if (result != NodeStatus.Running)
        {
            OnExit();
            wasNodeEntered = false;
        }
        return result;
    }

	public void referenceChildren(BaseNode _childerenNodes)
	{
        childerenNodes = new List<BaseNode>();
		childerenNodes.Add(_childerenNodes);
	}

	public void referenceChildren(List<BaseNode> _childerenNodes)
    {
		childerenNodes = new List<BaseNode>();
		foreach (var node in _childerenNodes)
        {
            childerenNodes.Add(node);
        }
    }

    public void SetupBlackboard(Blackboard _blackBoard)
    {
        this.blackBoard = _blackBoard;
        if (childerenNodes != null)
        {
            foreach (BaseNode node in childerenNodes)
            {
                node.SetupBlackboard(_blackBoard);
            }
        }
    }
    public virtual void OnReset() { }
    protected abstract NodeStatus Status();
	protected virtual void OnEnter() { }
	protected virtual void OnExit() { }
}