using Game.TDD;
using Game.TDD.Players.Looking;
using UnityEngine;

namespace Game.Players.Old
{
	public interface ILookingBehaviour : ISceneObject
	{
		IWorldToScreenProvider WorldToScreenProvider { get; set; }
		IScreenToWorldPointProvider ScreenToWorldPointProvider { get; set; }
		void PerformLookingInDirection(Vector2 lookDirection);
		void PerformLookingAtPosition(Vector2 mousePosition);
	}
	
	[RequireComponent(typeof(Rigidbody2D))]
	public sealed class PlayerLooking : TestableMonoBehaviour, ILookingBehaviour
	{
		ITransformProvider ISceneObject.TransformProvider { get; set; }
		IWorldToScreenProvider ILookingBehaviour.WorldToScreenProvider { get; set; }
		public IScreenToWorldPointProvider ScreenToWorldPointProvider { get; set; }

		public override void Init() => ScreenToWorldPointProvider = new ScreenToWorldPointProvider(Camera.main);

		public void PerformLookingAtPosition(Vector2 mousePosition)
		{
			if (mousePosition == Vector2.zero) return;
			Vector2 finalLookDirection;
			// if this is mouse input
			if (mousePosition.x > 1 || mousePosition.y > 1 || mousePosition.x < -1 || mousePosition.y < -1)
			{
				var worldPoint = ScreenToWorldPointProvider.Get(mousePosition, Position);
				finalLookDirection = worldPoint - Position;
			}
			else finalLookDirection = mousePosition;

			transform.rotation = Quaternion.FromToRotation(Vector3.up, finalLookDirection);
		}
		
		public void PerformLookingInDirection(Vector2 lookDirection)
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