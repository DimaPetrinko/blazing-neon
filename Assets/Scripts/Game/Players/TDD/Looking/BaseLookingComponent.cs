namespace Game.Players.TDD.Looking
{
	public abstract class BaseLookingComponent : BaseComponent<ILookingBehaviour>
	{
		private ILookingBehaviour movementBehaviour;

		public override ILookingBehaviour Behaviour => movementBehaviour ?? (movementBehaviour = CreateBehaviour());
	}
}