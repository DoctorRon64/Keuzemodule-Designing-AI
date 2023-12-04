using System.Collections.Generic;
using System.Threading.Tasks;

public enum NodeStatus { Success, Failed, Running }
public abstract class BaseNode
{
	public NodeStatus Status;
	public string name;

	protected abstract NodeStatus OnUpdate();
	protected virtual void OnEnter() { }
	protected virtual void OnExit() { }
}

public abstract class BaseCompositeNode : BaseNode
{
	public List<BaseNode> childerenNodes = new List<BaseNode>();

	public void AddChildNode(BaseNode _node)
	{
		childerenNodes.Add(_node);
	}
}

public abstract class BaseDecoratorw : BaseNode
{
	public List<BaseNode> childerenNodes = new List<BaseNode>();

	public void AddChildNode(BaseNode _node)
	{
		childerenNodes.Add(_node);
	}
}
