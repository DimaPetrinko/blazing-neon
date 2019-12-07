using Game.GameSystemServices;
using Game.GameSystemServices.TransformProviders;
using UnityEngine;

namespace Game.Players.Movement
{
	public interface IMovementBehaviour
	{
		float Speed { get; }
		ITransformProvider TransformProvider { get; set; }

		void PerformMovement(Vector2 movementDirection);
	}

	public sealed class PlayerMovement : IMovementBehaviour
	{
		public const float DEFAULT_SPEED = 1;

		public float Speed { get; }
		public ITransformProvider TransformProvider { get; set; }
		private ITimeService TimeService { get; }

		#region Constructors

		public PlayerMovement(float speed, ITimeService timeService = null, ITransformProvider transformProvider = null)
		{
			Speed = speed > 0 ? speed : DEFAULT_SPEED;
			TimeService = timeService ?? new UnityTimeService();
			TransformProvider =
				transformProvider ?? new UnityTransformProvider(new GameObject("PlayerMovement").transform);
		}

		public PlayerMovement(float speed, Transform transform) : this(speed, null,
			new UnityTransformProvider(transform)) {}

		public PlayerMovement(ITimeService timeService) : this(DEFAULT_SPEED, timeService) {}

		public PlayerMovement(ITransformProvider transformProvider) : this(DEFAULT_SPEED, null, transformProvider) {}

		public PlayerMovement() : this(DEFAULT_SPEED) {}

		#endregion

		public void PerformMovement(Vector2 movementDirection)
		{
			if (movementDirection == Vector2.zero) return;
			var movementVector = Speed * TimeService.DeltaTime * movementDirection.normalized;
			TransformProvider.Translate(movementVector, Space.World);
		}
	}
}