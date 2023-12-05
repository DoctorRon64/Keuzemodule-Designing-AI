using System.Collections.Generic;

public class SequenceNode : BaseNode
{
    public override NodeStatus Process()
    {
        NodeStatus childStatus = childerenNodes[currentIndex].Process();
        if (childStatus == NodeStatus.Running) { return NodeStatus.Running; }
        if (childStatus == NodeStatus.Failed) { return childStatus; }

        currentIndex++;
        if (currentIndex >= childerenNodes.Count) { currentIndex = 0; return NodeStatus.Success; }
        return NodeStatus.Running;
    }
}
