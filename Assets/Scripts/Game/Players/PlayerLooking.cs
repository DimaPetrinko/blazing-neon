using UnityEngine;

namespace Game.Players
{
	public interface ILookingBehaviour
	{
		IScreenToWorldPointProvider ScreenToWorldPointProvider { get; set; }
		void PerformLooking(Vector2 lookDirection);
	}

	[RequireComponent(typeof(Rigidbody2D))]
	public sealed class PlayerLooking : TestableMonoBehaviour, ILookingBehaviour
	{
		public IScreenToWorldPointProvider ScreenToWorldPointProvider { get; set; }

		public override void Init() => ScreenToWorldPointProvider = new ScreenToWorldPointProvider(Camera.main);

		public void PerformLooking(Vector2 lookDirection)
		{
			if (lookDirection == Vector2.zero) return;
			float angle;
			// if this is mouse input
			if (lookDirection.x > 1 || lookDirection.y > 1 || lookDirection.x < -1 || lookDirection.y < -1)
			{
				var position = transform.position;
				var worldPoint = ScreenToWorldPointProvider.Get(lookDirection, position);
				angle = Vector3.SignedAngle(Vector3.up, worldPoint - position, Vector3.forward);
			}
			else angle = Vector3.SignedAngle(Vector3.up, lookDirection, Vector3.forward);

			transform.rotation = Quaternion.Euler(0, 0, angle);
		}
	}
}