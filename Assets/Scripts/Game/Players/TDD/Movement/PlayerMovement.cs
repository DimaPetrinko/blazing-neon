using UnityEngine;

namespace Game.Players.TDD.Movement
{
	public sealed class PlayerMovement : IMovementBehaviour
	{
		public const float DEFAULT_SPEED = 1;
		public const float DEFAULT_DASH_SPEED = 1;

		public float Speed { get; private set; }
		public float DashSpeed { get; private set; }


		Vector3 IComponent.Position => TransformProvider.Position;
		Quaternion IComponent.Rotation => TransformProvider.Rotation;
		Vector3 IComponent.Scale => TransformProvider.LocalScale;
		
		public ITransformProvider TransformProvider { get; }
		public ITimeService TimeService { get; }

		#region Constructors

		public PlayerMovement(float speed, float dashSpeed, ITimeService timeService = null,
			ITransformProvider transformProvider = null)
		{
			Speed = speed;
			DashSpeed = dashSpeed;
			TimeService = timeService ?? new UnityTimeService();
			TransformProvider = transformProvider ?? new UnityTransformProvider(new GameObject().transform);
		}

		public PlayerMovement(float speed, float dashSpeed, Transform transform)
		{
			Speed = speed;
			DashSpeed = dashSpeed;
			TimeService = new UnityTimeService();
			TransformProvider = new UnityTransformProvider(transform);
		}

		public PlayerMovement(ITimeService timeService) : this(DEFAULT_SPEED, DEFAULT_DASH_SPEED, timeService,
			new UnityTransformProvider(new GameObject().transform)) {}

		public PlayerMovement(ITransformProvider transformProvider) : this(DEFAULT_SPEED, DEFAULT_DASH_SPEED,
			new UnityTimeService(), transformProvider) {}

		public PlayerMovement() : this(DEFAULT_SPEED, DEFAULT_DASH_SPEED, new UnityTimeService(),
			new UnityTransformProvider(new GameObject().transform)) {}

		#endregion

		public void PerformMovement(Vector2 movementDirection)
		{
			movementDirection.Normalize();
			var movementVector = Speed * TimeService.DeltaTime * movementDirection;
			TransformProvider.Translate(movementVector, Space.World);
		}

		public void PerformDash(Vector2 dashDirection) {}
	}
}