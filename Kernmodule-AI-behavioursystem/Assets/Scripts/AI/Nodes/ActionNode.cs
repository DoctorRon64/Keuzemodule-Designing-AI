public class ActionNode : BaseNode
{
    public delegate NodeStatus Tick();
    public Tick ProcessingMethod;

    public ActionNode(Tick _pm) 
    { 
        ProcessingMethod = _pm;
    }

    public override NodeStatus Process()
    {
        if (ProcessingMethod != null) {
            return ProcessingMethod();
        }
        return NodeStatus.Failed;
    }
}
