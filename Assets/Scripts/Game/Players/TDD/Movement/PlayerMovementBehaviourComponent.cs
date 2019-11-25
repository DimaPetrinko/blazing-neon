namespace Game.Players.TDD.Movement
{
	public sealed class PlayerMovementBehaviourComponent : BaseMovementBehaviourComponent
	{
		protected override IMovementBehaviour CreateBehaviour() => new PlayerMovement(speed, dashSpeed, transform);
	}
}