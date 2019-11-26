using UnityEngine;

namespace Game.Players
{
	public interface ILookingBehaviour
	{
		IScreenToWorldPointProvider ScreenToWorldPointProvider { get; set; }
		void PerformLookingWithGamepad(Vector2 lookDirection);
		void PerformLookingWithMouse(Vector2 lookDirection);
	}

	[RequireComponent(typeof(Rigidbody2D))]
	public sealed class PlayerLooking : TestableMonoBehaviour, ILookingBehaviour
	{
		public IScreenToWorldPointProvider ScreenToWorldPointProvider { get; set; }

		public override void Init() => ScreenToWorldPointProvider = new ScreenToWorldPointProvider(Camera.main);

		public void PerformLookingWithGamepad(Vector2 lookDirection)
		{
			if (lookDirection == Vector2.zero) return;
			Vector2 finalLookDirection;
			// if this is mouse input
			if (lookDirection.x > 1 || lookDirection.y > 1 || lookDirection.x < -1 || lookDirection.y < -1)
			{
				var worldPoint = ScreenToWorldPointProvider.Get(lookDirection, Position);
				finalLookDirection = worldPoint - Position;
			}
			else finalLookDirection = lookDirection;

			transform.rotation = Quaternion.FromToRotation(Vector3.up, finalLookDirection);
		}
		
		public void PerformLookingWithMouse(Vector2 lookDirection)
		{
			if (lookDirection == Vector2.zero) return;
			Vector2 finalLookDirection;
			// if this is mouse input
			if (lookDirection.x > 1 || lookDirection.y > 1 || lookDirection.x < -1 || lookDirection.y < -1)
			{
				var worldPoint = ScreenToWorldPointProvider.Get(lookDirection, Position);
				finalLookDirection = worldPoint - Position;
			}
			else finalLookDirection = lookDirection;

			transform.rotation = Quaternion.FromToRotation(Vector3.up, finalLookDirection);
		}
	}
}