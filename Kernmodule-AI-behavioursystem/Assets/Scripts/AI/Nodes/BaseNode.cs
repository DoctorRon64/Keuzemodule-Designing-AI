using System.Collections.Generic;
public enum NodeStatus { Success, Failed, Running }
public abstract class BaseNode
{
    public NodeStatus Status;
    public List<BaseNode> childerenNodes = new List<BaseNode>();
    protected int currentIndex = 0;
    protected Blackboard blackBoard;

    public virtual void referenceChildren(List<BaseNode> _childerenNodes)
    {
        foreach (var node in _childerenNodes)
        {
            childerenNodes.Add(node);
        }
    }

    public virtual void SetupBlackboard(Blackboard _blackBoard)
    {
        this.blackBoard = _blackBoard;
        foreach (BaseNode node in childerenNodes)
        {
            node.SetupBlackboard(_blackBoard);
        }
    }

    public virtual NodeStatus Process()
    {
        return childerenNodes[currentIndex].Process();
    }
}