using System;
using Game.Players.Old;
using UnityEngine;

namespace Game.TDD.Players.Input
{
	public sealed class PlayerInput : IInputBehaviour
	{
		public event EventHandler<Vector2EventArgs> Dash;
		public Vector2 MovementDirection { get; }
		public Vector2 LookDirection { get; }
		public DeviceType LookingDeviceType { get; }
	}
}