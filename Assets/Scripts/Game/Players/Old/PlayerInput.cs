using System;
using Input;
using UnityEngine;
using UnityEngine.InputSystem;
using DeviceType = Game.TDD.Players.Input.DeviceType;

namespace Game.Players.Old
{
	public interface IInputBehaviour
	{
		event EventHandler<Vector2EventArgs> Dash;
		Vector2 MovementDirection { get; }
		Vector2 LookDirection { get; }
		DeviceType LookingDeviceType { get; }
	}

	public sealed class Vector2EventArgs : EventArgs
	{
		public Vector2 Value { get; }

		public Vector2EventArgs(Vector2 value) => Value = value;
	}

	public sealed class PlayerInput : TestableMonoBehaviour, IInputBehaviour
	{
		public event EventHandler<Vector2EventArgs> Dash;

		[SerializeField] private float dashSpeedThreshold;

		private InputMaster controls;

		public Vector2 MovementDirection { get; private set; }
		public Vector2 LookDirection { get; private set; }
		public DeviceType LookingDeviceType { get; private set; }

		public override void Init()
		{
			controls = new InputMaster();
			controls.Player.Movement.performed += MovementPerformed;
			controls.Player.Dash.performed += DashPerformed;
			controls.Player.Looking.performed += LookingPerformed;
		}

		public override void Enabled() => controls.Enable();
		public override void Disabled() => controls.Disable();

		public override void Destroyed() => Dash = null;

		private void MovementPerformed(InputAction.CallbackContext context) =>
			MovementDirection = context.ReadValue<Vector2>();

		private void LookingPerformed(InputAction.CallbackContext context)
		{
			LookDirection = context.ReadValue<Vector2>();
			LookingDeviceType = GetDeviceType(context);
		}

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

		private void DashPerformed(InputAction.CallbackContext context)
		{
			if (MovementDirection.sqrMagnitude > dashSpeedThreshold) // TODO move to movement behaviour
				Dash?.Invoke(this, new Vector2EventArgs(MovementDirection));
		}
	}
}