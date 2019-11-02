using System;
using Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
	public sealed class PlayerInput : MonoBehaviour
	{
		public event Action Dash;

		[SerializeField] private float dashSpeedThreshold;

		private InputMaster controls;

		public Vector2 MovementDirection { get; private set; }

		private void Awake()
		{
			controls = new InputMaster();
			controls.Player.Movement.performed += MovementPerformed;
			controls.Player.Dash.performed += DashPerformed;
		}

		private void OnEnable() => controls.Enable();
		private void OnDisable() => controls.Disable();

		private void OnDestroy() => Dash = null;

		private void MovementPerformed(InputAction.CallbackContext context) =>
			MovementDirection = context.ReadValue<Vector2>();

		private void DashPerformed(InputAction.CallbackContext context)
		{
			if (MovementDirection.sqrMagnitude > dashSpeedThreshold) Dash?.Invoke();
		}
	}
}