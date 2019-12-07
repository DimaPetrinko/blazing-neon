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
		[SerializeField] private PlayerInputComponent _inputBehaviourComponent;
		[SerializeField] private PlayerMovementComponent _movementBehaviourComponent;
		[SerializeField] private PlayerDashingComponent _dashingBehaviourComponent;
		[SerializeField] private PlayerLookingComponent _lookingBehaviourComponent;

		protected override Player CreateInstance() => new Player(transform, _inputBehaviourComponent.Instance,
			_movementBehaviourComponent.Instance, _dashingBehaviourComponent.Instance,
			_lookingBehaviourComponent.Instance);

		private void FixedUpdate() => Instance.FixedUpdate();

		private void OnDestroy() => Instance.OnDestroy();
	}
}