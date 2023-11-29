using System.Collections.Generic;

public abstract class CompositeBaseNode : BaseNode
{
    public List<BaseNode> children;
    
    public CompositeBaseNode(List<BaseNode> _children)
    {
        this.children = _children;
    }

    public override void Setup(BlackBoard _blackBoard)
    {
        base.Setup(_blackBoard);

        foreach(BaseNode child in children)
        {
            child.Setup(_blackBoard);
        }
    }
}