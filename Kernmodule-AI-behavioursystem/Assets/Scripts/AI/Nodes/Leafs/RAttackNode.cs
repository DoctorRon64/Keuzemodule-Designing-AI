using System.Collections.Generic;

public class RAttackNode : BaseNode
{
    public delegate NodeStatus Tick();
    public Tick ProcessingMethod;

    public RAttackNode(Tick _pm)
    {
        ProcessingMethod = _pm;
    }

    public override NodeStatus Process()
    {
        if (ProcessingMethod != null)
        {
            return ProcessingMethod();
        }
        return NodeStatus.Failed;
    }

    public override void AddChildren(List<BaseNode> _childerenNodes)
    {
        //no children so no base.blablabal
    }
}
