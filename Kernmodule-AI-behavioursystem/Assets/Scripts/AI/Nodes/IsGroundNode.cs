

using UnityEngine;

public class IsGroundNode : BaseNode
{
	private bool isGrounded;

	public IsGroundNode(bool _isGrounded)
	{
		this.isGrounded = _isGrounded;
	}

	protected override NodeStatus Status()
	{
		if (isGrounded)
		{
			return NodeStatus.Success;
		}
		else
		{
			return NodeStatus.Failed;
		}
	}
}
