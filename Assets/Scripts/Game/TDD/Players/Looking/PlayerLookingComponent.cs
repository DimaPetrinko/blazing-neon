using Game.Players.Old;

namespace Game.TDD.Players.Looking
{
	public sealed class PlayerLookingComponent : BaseComponent<ILookingBehaviour>
	{
		protected override ILookingBehaviour CreateInstance() => new PlayerLooking(transform);
	}
}