namespace Game.Players.Movement
{
	public sealed class PlayerMovementComponent : BaseMovementComponent
	{
		protected override IMovementBehaviour CreateInstance() => new PlayerMovement(speed, transform);
	}
}