using System.Collections.Generic;
public class SelectorNode : BaseNode
{
    public override NodeStatus Process()
    {
        NodeStatus childStatus = childerenNodes[currentIndex].Process();
        if (childStatus == NodeStatus.Running) { return NodeStatus.Running; }
        if (childStatus == NodeStatus.Success)
        {
            currentIndex = 0;
            return NodeStatus.Success;
        }
        
        currentIndex++;
        if (currentIndex >= childerenNodes.Count)
        {
            currentIndex = 0;
            return NodeStatus.Failed;
        }
        return NodeStatus.Running;
    }
}
