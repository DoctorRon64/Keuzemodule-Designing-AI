using System.Collections.Generic;
using System.Threading.Tasks;

public class SelecterNode : CompositeBaseNode
{
    public TaskStatus Status { get; set; }

    public SelecterNode(List<BaseNode> _children) : base(_children)
    {

    }

    protected override TaskStatus OnUpdate()
    {
        return Status; 
    }
}
