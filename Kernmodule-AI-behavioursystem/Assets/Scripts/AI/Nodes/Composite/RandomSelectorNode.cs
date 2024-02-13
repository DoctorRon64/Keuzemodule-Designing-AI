using System.Collections.Generic;

public class RandomSelectorNode : Composite
{
    private List<int> availableIndices;

    public RandomSelectorNode(params BaseNode[] children) : base(children)
    {
        ResetAvailableIndices();
    }

    protected override NodeStatus OnUpdate()
    {
        if (availableIndices.Count == 0)
        {
            ResetAvailableIndices();
        }

        ShuffleAvailableIndices();

        foreach (int index in availableIndices)
        {
            BaseNode child = children[index];
            NodeStatus result = child.Tick();
            switch (result)
            {
                case NodeStatus.Success:
                    ResetAvailableIndices();
                    return NodeStatus.Success;
                case NodeStatus.Failed:
                    continue;
                case NodeStatus.Running:
                    return NodeStatus.Running;
            }
        }

        ResetAvailableIndices();
        return NodeStatus.Failed;
    }

    private void ResetAvailableIndices()
    {
        availableIndices = new List<int>();
        for (int i = 0; i < children.Length; i++)
        {
            availableIndices.Add(i);
        }
    }

    private void ShuffleAvailableIndices()
    {
        for (int i = 0; i < availableIndices.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, availableIndices.Count);
            int tempIndex = availableIndices[i];
            availableIndices[i] = availableIndices[randomIndex];
            availableIndices[randomIndex] = tempIndex;
        }
    }

    public override void OnReset()
    {
        base.OnReset();
        ResetAvailableIndices();
        foreach (var c in children)
        {
            c.OnReset();
        }
    }
}
