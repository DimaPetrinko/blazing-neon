using System.Collections;
using Game.TDD.GameSystemServices;
using UnityEngine;

namespace Game.TDD.Players.Dashing
{
	public interface IDashingBehaviour
	{
		float DashDistance { get; }
		float DashDuration { get; }
		AnimationCurve DashMovementCurve { get; }
		ITransformProvider TransformProvider { get; set; }
		ICoroutineRunner CoroutineRunner { get; set; }
		void PerformDash(Vector2 movementDirection);
	}

	public sealed class PlayerDashing : IDashingBehaviour
	{
		public const float DEFAULT_DASH_DISTANCE = 1;
		public const float DEFAULT_DASH_DURATION = 1;

		public float DashDistance { get; }
		public float DashDuration { get; }
		public AnimationCurve DashMovementCurve { get; }
		public ITransformProvider TransformProvider { get; set; }
		public ICoroutineRunner CoroutineRunner { get; set; }
		private ITimeService TimeService { get; }

		#region Constructors

		public PlayerDashing(float dashDistance, float dashDuration, AnimationCurve dashMovementCurve = null,
			ITimeService timeService = null, ICoroutineRunner coroutineRunner = null,
			ITransformProvider transformProvider = null)
		{
			DashDistance = dashDistance > 0 ? dashDistance : DEFAULT_DASH_DISTANCE;
			DashDuration = dashDuration > 0 ? dashDuration : DEFAULT_DASH_DURATION;
			DashMovementCurve = dashMovementCurve ?? AnimationCurve.Linear(0, 0, 1, 1);
			TimeService = timeService ?? new UnityTimeService();
			CoroutineRunner = coroutineRunner ?? new DefaultCoroutineRunner();
			TransformProvider = transformProvider ??
				new UnityTransformProvider(new GameObject("PlayerMovement").transform);
		}

		public PlayerDashing(float dashDistance, float dashDuration, AnimationCurve dashMovementCurve,
			ICoroutineRunner coroutineRunner, Transform transform) : this(dashDistance, dashDuration, dashMovementCurve,
			null, coroutineRunner, new UnityTransformProvider(transform)) {}

		public PlayerDashing(ITimeService timeService) : this(DEFAULT_DASH_DISTANCE, DEFAULT_DASH_DURATION, null,
			timeService) {}

		public PlayerDashing(ITransformProvider transformProvider) : this(DEFAULT_DASH_DISTANCE, DEFAULT_DASH_DURATION,
			null, null, null, transformProvider) {}

		public PlayerDashing() : this(DEFAULT_DASH_DISTANCE, DEFAULT_DASH_DURATION) {}

		#endregion

		public void PerformDash(Vector2 movementDirection) => CoroutineRunner.StartCoroutine(Dash(movementDirection));

		private IEnumerator Dash(Vector2 movementDirection)
		{
//			if (!canDash || isDashing) yield break;
			movementDirection.Normalize();
//			canDash = false;
			var startingDashDirection = movementDirection;
			var dashTime = 0f;
			var previousMovementProgress = 0f;
			while (dashTime < DashDuration)
			{
				dashTime += TimeService.DeltaTime;
				var dashMovementProgress = DashMovementCurve.Evaluate(dashTime / DashDuration) * DashDistance;
				var dashMovementVector = (dashMovementProgress - previousMovementProgress) * startingDashDirection;
				previousMovementProgress = dashMovementProgress;

				PerformDashMovement(dashMovementVector);
				yield return null;
			}

//			yield return new WaitForSeconds(postDashCooldown);
//			canDash = true;
		}

		private void PerformDashMovement(Vector2 movementDirection)
		{
			if (movementDirection == Vector2.zero) return;
			TransformProvider.Translate(movementDirection, Space.World);
		}
	}
}