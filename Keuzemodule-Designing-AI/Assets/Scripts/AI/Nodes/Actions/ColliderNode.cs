using System.Collections.Generic;
using UnityEngine;

public class ColliderNode : Node
{
	private readonly Collider2D enabledCollider;
	List<Collider2D> allColliders = new List<Collider2D>();

	public ColliderNode(Collider2D _colliderEnabled)
	{
		this.enabledCollider = _colliderEnabled;
	}

	protected override NodeStatus OnUpdate()
	{
		if (enabledCollider != null && enabledCollider.enabled == true)
		{
			return NodeStatus.Success;
		} 
		else
		{
			return NodeStatus.Failed;
		}
	}

	protected override void OnEnter()
	{
		base.OnEnter();
		blackboard.SetVariable(VariableNames.BossCurrentNode, $"{GetNodeName()}, Result: {allColliders}");
		allColliders = blackboard.GetVariable<List<Collider2D>>(VariableNames.BossColliders);
		foreach (var collider in allColliders)
		{
			collider.enabled = collider == enabledCollider;
		}
	}
}
