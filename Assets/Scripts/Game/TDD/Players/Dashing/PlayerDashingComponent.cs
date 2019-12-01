using Game.TDD.GameSystemServices;

namespace Game.TDD.Players.Dashing
{
	public sealed class PlayerDashingComponent : BaseDashingComponent
	{
		protected override IDashingBehaviour CreateInstance() => new PlayerDashing(dashDistance, dashDuration,
			dashSpeedCurve, new UnityCoroutineRunner(this), transform);
	}
}