using Game.TDD.GameSystemServices;

namespace Game.TDD.Players.Movement
{
	public sealed class PlayerMovementComponent : BaseMovementComponent
	{
		protected override IMovementBehaviour CreateInstance() => new PlayerMovement(speed, dashDistance, dashDuration,
			dashSpeedCurve, new UnityCoroutineRunner(this), transform);
	}
}