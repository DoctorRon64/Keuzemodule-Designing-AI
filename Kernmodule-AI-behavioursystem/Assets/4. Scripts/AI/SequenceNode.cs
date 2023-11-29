using System.Collections.Generic;
using System.Threading.Tasks;

public class SequenceNode : CompositeBaseNode
{
    private TaskStatus status { get; set; }

    public SequenceNode(List<BaseNode> _children) : base(_children)
    {
        
    }

    protected override TaskStatus OnUpdate()
    {
        return status;
    }
}