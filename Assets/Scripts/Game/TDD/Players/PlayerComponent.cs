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
		[SerializeField] private PlayerInputComponent inputBehaviourComponent;
		[SerializeField] private PlayerMovementComponent movementBehaviourComponent;
		[SerializeField] private PlayerDashingComponent dashingBehaviourComponent;
		[SerializeField] private PlayerLookingComponent lookingBehaviourComponent;

		protected override Player CreateInstance() => new Player(transform, inputBehaviourComponent.Instance,
			movementBehaviourComponent.Instance, dashingBehaviourComponent.Instance,
			lookingBehaviourComponent.Instance);

		private void FixedUpdate() => Instance.FixedUpdate();
	}
}