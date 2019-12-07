using Game.GameSystemServices.CoroutineRunners;

namespace Game.Players.Dashing
{
	public sealed class PlayerDashingComponent : BaseDashingComponent
	{
		protected override IDashingBehaviour CreateInstance() => new PlayerDashing(_distance, _speed, _cooldown,
			_movementCurve, new UnityCoroutineRunner(this), transform);
	}
}