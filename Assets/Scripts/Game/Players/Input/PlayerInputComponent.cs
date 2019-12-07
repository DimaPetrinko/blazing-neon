namespace Game.Players.Input
{
	public sealed class PlayerInputComponent : BaseInputComponent
	{
		protected override IInputBehaviour CreateInstance() => new PlayerInput();

		private void OnEnable() => Instance.OnEnable();
		private void OnDisable() => Instance.OnDisable();
	}
}