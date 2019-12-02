using System;
using System.Collections.Generic;
using Game.GameSystemServices;
using Game.GameSystemServices.TransformProviders;
using Game.Players.Dashing;
using Game.Players.Input;
using UnityEngine;
using DeviceType = Game.Players.Input.DeviceType;
using ILookingBehaviour = Game.Players.Looking.ILookingBehaviour;
using IMovementBehaviour = Game.Players.Movement.IMovementBehaviour;
using PlayerLooking = Game.Players.Looking.PlayerLooking;
using PlayerMovement = Game.Players.Movement.PlayerMovement;

namespace Game.Players
{
	public sealed class Player : ISceneObject
	{
		private Dictionary<DeviceType, Action<Vector2>> performLookingActions;

		public IInputBehaviour InputBehaviour { get; }
		public IMovementBehaviour MovementBehaviour { get; }
		public IDashingBehaviour DashingBehaviour { get; }
		public ILookingBehaviour LookingBehaviour { get; }
		public ITransformProvider TransformProvider { get; set; }

		#region Constructors

		public Player(ITransformProvider transformProvider = null, IInputBehaviour inputBehaviour = null,
			IMovementBehaviour movementBehaviour = null, IDashingBehaviour dashingBehaviour = null,
			ILookingBehaviour lookingBehaviour = null)
		{
			if (transformProvider == null)
			{
				if (movementBehaviour != null) TransformProvider = movementBehaviour.TransformProvider;
				else if (dashingBehaviour != null) TransformProvider = dashingBehaviour.TransformProvider;
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
			if (dashingBehaviour != null)
			{
				DashingBehaviour = dashingBehaviour;
				DashingBehaviour.TransformProvider = TransformProvider;
			}
			else DashingBehaviour = new PlayerDashing(TransformProvider);

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
			IMovementBehaviour movementBehaviour = null, IDashingBehaviour dashingBehaviour = null,
			ILookingBehaviour lookingBehaviour = null) : this(new UnityTransformProvider(transform), inputBehaviour,
			movementBehaviour, dashingBehaviour, lookingBehaviour) {}

		#endregion

		private void Init()
		{
			InputBehaviour.Dash += DashEventHandler;

			performLookingActions = new Dictionary<DeviceType, Action<Vector2>>
			{
				{DeviceType.Mouse, LookingBehaviour.PerformLookingAtPosition},
				{DeviceType.Gamepad, LookingBehaviour.PerformLookingInDirection}
			};
		}

		private void DashEventHandler(object sender, Vector2EventArgs args) => DashingBehaviour.PerformDash(args.Value);

		public void FixedUpdate()
		{
			if (!DashingBehaviour.IsDashing) MovementBehaviour.PerformMovement(InputBehaviour.MovementDirection);

			if (performLookingActions.ContainsKey(InputBehaviour.LookDeviceType))
				performLookingActions[InputBehaviour.LookDeviceType].Invoke(InputBehaviour.LookDirection);
		}
		
		public void OnDestroy() => InputBehaviour.Dash -= DashEventHandler;
	}
}