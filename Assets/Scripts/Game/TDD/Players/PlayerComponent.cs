using Game.TDD.GameSystemServices;
using Game.TDD.Players.Dashing;
using Game.TDD.Players.Input;
using Game.TDD.Players.Looking;
using Game.TDD.Players.Movement;
using UnityEngine;

namespace Game.TDD.Players
{
	public sealed class PlayerComponent : BaseComponent<Player>
	{
		[Header("Player component")]
		[SerializeField] private PlayerInputComponent playerInput;
		[SerializeField] private PlayerMovementComponent playerMovementComponent;
		[SerializeField] private PlayerDashingComponent playerDashingComponent;
		[SerializeField] private PlayerLookingComponent playerLookingComponent;

		protected override Player CreateInstance() => new Player(transform, playerInput.Instance,
			playerMovementComponent.Instance, playerDashingComponent.Instance, playerLookingComponent.Instance);

		private void FixedUpdate() => Instance.FixedUpdate();
	}
}