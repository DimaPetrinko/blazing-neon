using Game.Players.Old;
using UnityEngine;

namespace Game.TDD.Players.Looking
{
	public sealed class PlayerLooking : ILookingBehaviour
	{
		IScreenToWorldPointProvider ILookingBehaviour.ScreenToWorldPointProvider { get; set; } // TODO: compatibility

		public ITransformProvider TransformProvider { get; set; }
		public IWorldToScreenProvider WorldToScreenProvider { get; set; }

		#region Constructors

		public PlayerLooking(IWorldToScreenProvider worldToScreenProvider = null,
			ITransformProvider transformProvider = null)
		{
			WorldToScreenProvider = worldToScreenProvider ?? new UnityWorldToScreenProvider(Camera.main);
			TransformProvider = transformProvider ?? new UnityTransformProvider(
				new GameObject("PlayerLooking").transform);
		}

		public PlayerLooking(Transform transform) : this(new UnityWorldToScreenProvider(Camera.main),
			new UnityTransformProvider(transform)) {}

		public PlayerLooking(ITransformProvider transformProvider) : this(new UnityWorldToScreenProvider(Camera.main),
			transformProvider) {}

		public PlayerLooking() : this(new GameObject("PlayerLooking").transform) {}

		#endregion

		public void PerformLookingInDirection(Vector2 lookDirection)
		{
			if (lookDirection == Vector2.zero) return;
			TransformProvider.Rotation = Quaternion.FromToRotation(Vector3.up, lookDirection);
		}

		public void PerformLookingAtPosition(Vector2 mousePosition)
		{
			var screenPosition = WorldToScreenProvider.Get(TransformProvider.Position);
			var lookingDirection = mousePosition - screenPosition;
			PerformLookingInDirection(lookingDirection);
		}
	}
}