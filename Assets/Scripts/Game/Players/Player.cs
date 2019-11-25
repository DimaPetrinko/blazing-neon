using Game.Players.TDD.Movement;
using UnityEngine;

namespace Game.Players
{
	public interface ICharacterController
	{
		IInputBehaviour InputBehaviour { get; }
		IMovementBehaviour MovementBehaviour { get; }
		ILookingBehaviour LookingBehaviour { get; }
	}

	[RequireComponent(typeof(PlayerInput))]
//	[RequireComponent(typeof(PlayerMovementMonoBehaviour))]
	[RequireComponent(typeof(PlayerLooking))]
	public sealed class Player : TestableMonoBehaviour, ICharacterController
	{
		public IInputBehaviour InputBehaviour { get; set; }
		public IMovementBehaviour MovementBehaviour { get; set; }
		public ILookingBehaviour LookingBehaviour { get; set; }

		public override void Init()
		{
			InputBehaviour = GetComponent<PlayerInput>();
			MovementBehaviour = GetComponent<PlayerMovementBehaviourComponent>()?.Behaviour ??
				GetComponent<PlayerMovement>();
			LookingBehaviour = GetComponent<PlayerLooking>();
			InputBehaviour.Dash += DashEventHandler;
		}

		private void DashEventHandler(object sender, Vector2EventArgs args) =>
			MovementBehaviour.PerformDash(args.Value);

		public override void FixedUpdateMethod()
		{
			MovementBehaviour.PerformMovement(InputBehaviour.MovementDirection);
			LookingBehaviour.PerformLooking(InputBehaviour.LookDirection);
		}
	}
}