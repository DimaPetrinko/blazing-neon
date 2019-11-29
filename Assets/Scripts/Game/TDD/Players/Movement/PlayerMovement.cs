using System.Collections;
using Game.TDD.GameSystemServices;
using UnityEngine;

namespace Game.TDD.Players.Movement
{
	public interface IMovementBehaviour
	{
		float Speed { get; }
		float DashDistance { get; }
		float DashDuration { get; }
		AnimationCurve DashSpeedCurve { get; }
		ITransformProvider TransformProvider { get; set; }
		ICoroutineRunner CoroutineRunner { get; set; }
		
		void PerformMovement(Vector2 movementDirection);
		void PerformDash(Vector2 movementDirection);
	}

	public sealed class PlayerMovement : IMovementBehaviour
	{
		public const float DEFAULT_SPEED = 1;
		public const float DEFAULT_DASH_DISTANCE = 1;
		public const float DEFAULT_DASH_DURATION = 1;
		
		private Vector2 startingDashDirection;
		private Vector2 dashMovementVector;

		public float Speed { get; }
		public float DashDistance { get; }
		public float DashDuration { get; }
		public AnimationCurve DashSpeedCurve { get; }

		public ITransformProvider TransformProvider { get; set; }
		public ICoroutineRunner CoroutineRunner { get; set; }
		private ITimeService TimeService { get; }

		#region Constructors

		public PlayerMovement(float speed, float dashDistance, float dashDuration, AnimationCurve dashSpeedCurve = null,
			ITimeService timeService = null, ICoroutineRunner coroutineRunner = null,
			ITransformProvider transformProvider = null)
		{
			Speed = speed > 0 ? speed : DEFAULT_SPEED;
			DashDistance = dashDistance > 0 ? dashDistance : DEFAULT_DASH_DISTANCE;
			DashDuration = dashDuration > 0 ? dashDuration : DEFAULT_DASH_DURATION;
			DashSpeedCurve = dashSpeedCurve ?? AnimationCurve.Linear(0, 0, 1, 1);
			TimeService = timeService ?? new UnityTimeService();
			CoroutineRunner = coroutineRunner ?? new DefaultCoroutineRunner();
			TransformProvider = transformProvider ??
				new UnityTransformProvider(new GameObject("PlayerMovement").transform);
		}

		public PlayerMovement(float speed, float dashDistance, float dashDuration, AnimationCurve dashSpeedCurve,
			ICoroutineRunner coroutineRunner, Transform transform) : this(speed, dashDistance, dashDuration,
			dashSpeedCurve, null, coroutineRunner, new UnityTransformProvider(transform)) {}

		public PlayerMovement(ITimeService timeService) : this(DEFAULT_SPEED, DEFAULT_DASH_DISTANCE,
			DEFAULT_DASH_DURATION, null, timeService) {}

		public PlayerMovement(ITransformProvider transformProvider) : this(DEFAULT_SPEED, DEFAULT_DASH_DISTANCE,
			DEFAULT_DASH_DURATION, null, null, null, transformProvider) {}

		public PlayerMovement() : this(DEFAULT_SPEED, DEFAULT_DASH_DISTANCE, DEFAULT_DASH_DURATION) {}

		#endregion

		public void PerformMovement(Vector2 movementDirection)
		{
			movementDirection.Normalize();
			Vector2 movementVector;
			if (dashMovementVector == Vector2.zero) movementVector = Speed * TimeService.DeltaTime * movementDirection;
			else
			{
				movementVector = dashMovementVector;
				ResetDash();
			}

			TransformProvider.Translate(movementVector, Space.World);
		}

		public void PerformDash(Vector2 movementDirection) => CoroutineRunner.StartCoroutine(Dash(movementDirection));

		private IEnumerator Dash(Vector2 movementDirection)
		{
//			if (!canDash || isDashing) yield break;
			movementDirection.Normalize();
//			canDash = false;
			startingDashDirection = movementDirection;
			var dashTime = 0f;
			var previousMovementProgress = 0f;
			while (dashTime < DashDuration)
			{
				dashTime += TimeService.DeltaTime;
				var dashMovementProgress = DashSpeedCurve.Evaluate(dashTime / DashDuration) * DashDistance;
				dashMovementVector += (dashMovementProgress - previousMovementProgress) * startingDashDirection;
				previousMovementProgress = dashMovementProgress;
				yield return null;
			}

			startingDashDirection = Vector2.zero;
//			yield return new WaitForSeconds(postDashCooldown);
//			canDash = true;
		}

		private void ResetDash() => dashMovementVector = Vector2.zero;
	}
}