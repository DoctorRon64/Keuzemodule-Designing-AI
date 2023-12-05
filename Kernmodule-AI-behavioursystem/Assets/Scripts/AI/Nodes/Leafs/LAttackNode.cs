using System.Collections.Generic;

public class LAttackNode : BaseNode
{
    public delegate NodeStatus Tick();
    public Tick ProcessingMethod;

    public LAttackNode(Tick _pm)
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
