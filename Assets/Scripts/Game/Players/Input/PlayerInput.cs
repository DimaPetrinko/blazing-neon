using System;
using Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Players.Input
{
	public interface IInputBehaviour
	{
		event EventHandler<Vector2EventArgs> Dash;
		Vector2 MovementDirection { get; }
		Vector2 LookDirection { get; }
		DeviceType LookDeviceType { get; }
		void SetLookDirection(Vector2 newLookDirection, DeviceType deviceType);
		void SetMovementDirection(Vector2 newMovementDirection);
		void OnEnable();
		void OnDisable();
	}

	public sealed class PlayerInput : IInputBehaviour
	{
		private readonly InputMaster _controls;

		public event EventHandler<Vector2EventArgs> Dash;
		public Vector2 MovementDirection { get; private set; }
		public Vector2 LookDirection { get; private set; }
		public DeviceType LookDeviceType { get; private set; }

		public PlayerInput(InputMaster inputMaster = null)
		{
			_controls = inputMaster ?? new InputMaster();
			Init();
		}

		private void Init()
		{
			_controls.Player.Movement.performed += MovementPerformed;
			_controls.Player.Dash.performed += DashPerformed;
			_controls.Player.Looking.performed += LookingPerformed;
		}

		public void OnEnable() => _controls.Enable();

		public void OnDisable() => _controls.Disable();

		private void MovementPerformed(InputAction.CallbackContext context) =>
			MovementDirection = context.ReadValue<Vector2>();

		private void LookingPerformed(InputAction.CallbackContext context)
		{
			LookDirection = context.ReadValue<Vector2>();
			LookDeviceType = GetDeviceType(context);
		}

		private void DashPerformed(InputAction.CallbackContext context) =>
			Dash?.Invoke(this, new Vector2EventArgs(MovementDirection));

		public static DeviceType GetDeviceType(InputAction.CallbackContext context)
		{
			switch (context.control.device)
			{
				case Mouse _: return DeviceType.Mouse;
				case Gamepad _: return DeviceType.Gamepad;
				case Keyboard _: return DeviceType.Keyboard;
				default: return DeviceType.None;
			}
		}

		public void SetLookDirection(Vector2 newLookDirection, DeviceType deviceType)
		{
			if (deviceType == DeviceType.Keyboard) return;
			LookDirection = newLookDirection.normalized;
			LookDeviceType = deviceType;
		}

		public void SetMovementDirection(Vector2 newMovementDirection) =>
			MovementDirection = newMovementDirection.normalized;
	}

	public sealed class Vector2EventArgs : EventArgs
	{
		public Vector2 Value { get; }

		public Vector2EventArgs(Vector2 value) => Value = value;
	}
}