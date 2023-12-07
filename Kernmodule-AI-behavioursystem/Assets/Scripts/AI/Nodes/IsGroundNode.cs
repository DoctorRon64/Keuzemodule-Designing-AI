public class IsGroundNode : BaseNode
{
	private bool isGrounded;

	public IsGroundNode(Blackboard _blackBoard) : base(_blackBoard)
	{

	}

	protected override NodeStatus Status()
	{
		bool isGrounded = blackBoard.GetVariable<bool>(VariableNames.PlayerIsGrounded);

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
