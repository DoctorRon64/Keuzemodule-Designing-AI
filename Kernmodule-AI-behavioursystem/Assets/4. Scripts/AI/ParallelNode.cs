using System.Collections.Generic;
using System.Threading.Tasks;

public class ParallelNode : CompositeBaseNode
{
    private TaskStatus status { get; set; }

    public ParallelNode(List<BaseNode> _children) : base(_children)
    {

    }

    protected override TaskStatus OnUpdate()
    {
        return status;
    }
}