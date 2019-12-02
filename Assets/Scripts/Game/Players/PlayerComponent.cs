using Game.GameSystemServices;
using Game.Players.Dashing;
using Game.Players.Input;
using Game.Players.Looking;
using Game.Players.Movement;
using UnityEngine;

namespace Game.Players
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

		private void OnDestroy() => Instance.OnDestroy();
	}
}