using System.Collections.Generic;
using UnityEngine;

public class ColliderNode : BaseNode
{
	private Collider2D enabledCollider;
	List<Collider2D> allColliders = new List<Collider2D>();

	public ColliderNode(Collider2D _colliderEnabled)
	{
		this.enabledCollider = _colliderEnabled;
	}

	protected override NodeStatus OnUpdate()
	{
		Debug.Log($"Checking {GetNodeName()} condition");

		if (enabledCollider != null && enabledCollider.enabled == true)
		{
			OnReset();
			return NodeStatus.Success;
		} 
		else
		{
			OnReset();
			return NodeStatus.Failed;
		}
	}

	protected override void OnEnter()
	{
		allColliders = blackboard.GetVariable<List<Collider2D>>(VariableNames.BossColliders);
		foreach (var collider in allColliders)
		{
			collider.enabled = collider == enabledCollider;
		}
	}
}
