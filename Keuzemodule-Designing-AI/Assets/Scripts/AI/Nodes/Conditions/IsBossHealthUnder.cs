public class IsBossHealthUnder : Node
{
	private readonly int health;

	public IsBossHealthUnder(int _health)
	{
		this.health = _health;
	}

	protected override NodeStatus OnUpdate()
	{
		if (blackboard?.GetVariable<int>(VariableNames.BossHealth) < health)
		{
			return NodeStatus.Success;
		}
		else
		{
			return NodeStatus.Failed;
		}
	}
}