using System;
using System.Collections.Generic;
using Game.Players.Old;
using UnityEngine;
using DeviceType = Game.TDD.Players.Input.DeviceType;
using PlayerInput = Game.TDD.Players.Input.PlayerInput;
using PlayerLooking = Game.TDD.Players.Looking.PlayerLooking;
using PlayerMovement = Game.TDD.Players.Movement.PlayerMovement;

namespace Game.TDD.Players
{
	public sealed class Player : ISceneObject
	{
		private Dictionary<DeviceType, Action<Vector2>> performLookingActions;

		public IInputBehaviour InputBehaviour { get; }
		public IMovementBehaviour MovementBehaviour { get; }
		public ILookingBehaviour LookingBehaviour { get; }
		public ITransformProvider TransformProvider { get; set; }

		#region Constructors

		public Player(ITransformProvider transformProvider = null, IInputBehaviour inputBehaviour = null,
			IMovementBehaviour movementBehaviour = null, ILookingBehaviour lookingBehaviour = null)
		{
			if (transformProvider == null)
			{
				if (movementBehaviour != null) TransformProvider = movementBehaviour.TransformProvider;
				else if (lookingBehaviour != null) TransformProvider = lookingBehaviour.TransformProvider;
				else TransformProvider = new UnityTransformProvider(new GameObject("Player").transform);
			}
			else TransformProvider = transformProvider;


			if (movementBehaviour != null)
			{
				MovementBehaviour = movementBehaviour;
				MovementBehaviour.TransformProvider = TransformProvider;
			}
			else MovementBehaviour = new PlayerMovement(TransformProvider);

			if (lookingBehaviour != null)
			{
				LookingBehaviour = lookingBehaviour;
				LookingBehaviour.TransformProvider = TransformProvider;
			}
			else LookingBehaviour = new PlayerLooking(TransformProvider);

			InputBehaviour = inputBehaviour ?? new PlayerInput();

			Init();
		}

		public Player(Transform transform, IInputBehaviour inputBehaviour = null,
			IMovementBehaviour movementBehaviour = null, ILookingBehaviour lookingBehaviour = null)
			: this(new UnityTransformProvider(transform), inputBehaviour, movementBehaviour, lookingBehaviour) {}

		#endregion

		private void Init()
		{
			InputBehaviour.Dash += (sender, args) => MovementBehaviour.PerformDash(args.Value);

			performLookingActions = new Dictionary<DeviceType, Action<Vector2>>
			{
				{DeviceType.Mouse, LookingBehaviour.PerformLookingAtPosition},
				{DeviceType.Gamepad, LookingBehaviour.PerformLookingInDirection}
			};
		}

		public void FixedUpdate()
		{
			MovementBehaviour.PerformMovement(InputBehaviour.MovementDirection);

			if (performLookingActions.ContainsKey(InputBehaviour.LookingDeviceType))
				performLookingActions[InputBehaviour.LookingDeviceType].Invoke(InputBehaviour.LookDirection);
		}
	}
}