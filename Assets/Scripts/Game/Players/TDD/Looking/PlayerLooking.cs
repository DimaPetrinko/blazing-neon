using UnityEngine;

namespace Game.Players.TDD.Looking
{
	public sealed class PlayerLooking : ILookingBehaviour, IComponent
	{
		public ITransformProvider TransformProvider { get; }
		public ITimeService TimeService { get; }
		public IScreenToWorldPointProvider ScreenToWorldPointProvider { get; set; }

		#region Constructors

		public PlayerLooking(ITimeService timeService = null, ITransformProvider transformProvider = null)
		{
			TimeService = timeService ?? new UnityTimeService();
			TransformProvider = transformProvider ?? new UnityTransformProvider(new GameObject().transform);
		}

		public PlayerLooking(Transform transform)
		{
			TimeService = new UnityTimeService();
			TransformProvider = new UnityTransformProvider(transform);
		}

		public PlayerLooking(ITimeService timeService) : this(timeService,
			new UnityTransformProvider(new GameObject().transform)) {}

		public PlayerLooking(ITransformProvider transformProvider) : this(new UnityTimeService(), transformProvider) {}

		public PlayerLooking() : this(new GameObject().transform) {}

		#endregion

		public void PerformLookingWithGamepad(Vector2 lookDirection)
		{
			if (lookDirection == Vector2.zero) return;
			TransformProvider.Rotation = Quaternion.FromToRotation(Vector3.up, lookDirection);
		}

		public void PerformLookingWithMouse(Vector2 lookDirection) {}
	}
}