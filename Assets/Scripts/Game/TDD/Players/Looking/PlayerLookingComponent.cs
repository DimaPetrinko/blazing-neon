using Game.Players.Old;
using Game.TDD.GameSystemServices;

namespace Game.TDD.Players.Looking
{
	public sealed class PlayerLookingComponent : BaseComponent<ILookingBehaviour>
	{
		protected override ILookingBehaviour CreateInstance() => new PlayerLooking(transform);
	}
}