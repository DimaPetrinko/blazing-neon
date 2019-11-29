using System;
using System.Collections.Generic;
using UnityEngine;
using DeviceType = Game.TDD.Players.Input.DeviceType;

namespace Game.Players.Old
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
		private Dictionary<DeviceType, Action<Vector2>> performLookingActions;

		public IInputBehaviour InputBehaviour { get; set; }
		public IMovementBehaviour MovementBehaviour { get; set; }
		public ILookingBehaviour LookingBehaviour { get; set; }

		public override void Init()
		{
			InputBehaviour = GetComponent<PlayerInput>();
			MovementBehaviour = GetComponent<PlayerMovement>();
			LookingBehaviour = GetComponent<PlayerLooking>();
			InputBehaviour.Dash += DashEventHandler;

			performLookingActions = new Dictionary<DeviceType, Action<Vector2>>
			{
				{DeviceType.Mouse, LookingBehaviour.PerformLookingAtPosition},
				{DeviceType.Gamepad, LookingBehaviour.PerformLookingInDirection}
			};
		}

		private void DashEventHandler(object sender, Vector2EventArgs args) =>
			MovementBehaviour.PerformDash(args.Value);

		public override void FixedUpdateMethod()
		{
			MovementBehaviour.PerformMovement(InputBehaviour.MovementDirection);

			if (performLookingActions.ContainsKey(InputBehaviour.LookingDeviceType))
				performLookingActions[InputBehaviour.LookingDeviceType].Invoke(InputBehaviour.LookDirection);
		}
	}
}