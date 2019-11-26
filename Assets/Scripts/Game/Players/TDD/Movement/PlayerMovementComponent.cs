namespace Game.Players.TDD.Movement
{
	public sealed class PlayerMovementComponent : BaseMovementComponent
	{
		protected override IMovementBehaviour CreateBehaviour() => new PlayerMovement(speed, dashSpeed, transform);
	}
}