using System.Threading.Tasks;

[System.Flags]
public enum NodeStatus
{
    Succes,
    Failed,
    Running
}

public abstract class BaseNode
{
    public BaseNode parent;
    protected BlackBoard blackBoard;

    public virtual void Setup(BlackBoard _blackBoard)
    {
        this.blackBoard = _blackBoard;
    }
    protected abstract TaskStatus OnUpdate();
    protected virtual void OnEnter() { }
    protected virtual void OnExit() { }
    public virtual void OnReset() { }
}