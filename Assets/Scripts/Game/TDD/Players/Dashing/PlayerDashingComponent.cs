using Game.TDD.GameSystemServices;
using Game.TDD.GameSystemServices.CoroutineRunners;

namespace Game.TDD.Players.Dashing
{
	public sealed class PlayerDashingComponent : BaseDashingComponent
	{
		protected override IDashingBehaviour CreateInstance() => new PlayerDashing(distance, speed, cooldown,
			movementCurve, new UnityCoroutineRunner(this), transform);
	}
}