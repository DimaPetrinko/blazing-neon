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
	[RequireComponent(typeof(PlayerMovement))]
	[RequireComponent(typeof(PlayerLooking))]
	public sealed class Player : TestableMonoBehaviour, ICharacterController
	{
		public IInputBehaviour InputBehaviour { get; private set; }
		public IMovementBehaviour MovementBehaviour { get; private set; }
		public ILookingBehaviour LookingBehaviour { get; private set; }


		public override void Init()
		{
			InputBehaviour = GetComponent<PlayerInput>();
			MovementBehaviour = GetComponent<PlayerMovement>();
			LookingBehaviour = GetComponent<PlayerLooking>();
			InputBehaviour.Dash += MovementBehaviour.PerformDash;
		}

		public override void FixedUpdateMethod()
		{
			MovementBehaviour.PerformMovement(InputBehaviour.MovementDirection);
			LookingBehaviour.PerformLooking(InputBehaviour.LookDirection);
		}
	}
}