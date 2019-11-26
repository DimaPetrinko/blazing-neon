namespace Game.Players.TDD.Looking
{
	public sealed class PlayerLookingComponent : BaseLookingComponent
	{
		protected override ILookingBehaviour CreateBehaviour() => new PlayerLooking(transform);
	}
}