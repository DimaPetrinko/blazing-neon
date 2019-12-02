using Game.GameSystemServices.CoroutineRunners;

namespace Game.Players.Dashing
{
	public sealed class PlayerDashingComponent : BaseDashingComponent
	{
		protected override IDashingBehaviour CreateInstance() => new PlayerDashing(distance, speed, cooldown,
			movementCurve, new UnityCoroutineRunner(this), transform);
	}
}