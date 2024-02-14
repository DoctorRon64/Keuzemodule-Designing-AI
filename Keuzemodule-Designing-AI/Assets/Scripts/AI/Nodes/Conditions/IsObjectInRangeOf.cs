using UnityEngine;

public class IsObjectInRangeOf : BaseNode
{
	private Transform Transform;
	private float minRange;
	private float maxRange;

	public IsObjectInRangeOf(Transform _transform, float _minRange, float _maxRange)
	{
		this.Transform = _transform;
		this.minRange = _minRange;
		this.maxRange = _maxRange;
	}

	protected override NodeStatus OnUpdate()
	{
		if (Transform != null)
		{
			float xPosition = Transform.position.x;
			if (xPosition >= minRange && xPosition <= maxRange)
			{
				return NodeStatus.Success;
			} 
			else
			{
				return NodeStatus.Failed;
			}
		} 
		else
		{
			return NodeStatus.Failed;
		}
	}
}
