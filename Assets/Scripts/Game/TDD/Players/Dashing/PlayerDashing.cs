using System.Collections;
using Game.TDD.GameSystemServices;
using UnityEngine;

namespace Game.TDD.Players.Dashing
{
	public interface IDashingBehaviour
	{
		float Distance { get; }
		float Speed { get; }
		float Cooldown { get; }
		bool IsDashing { get; }
		AnimationCurve MovementCurve { get; }
		ITransformProvider TransformProvider { get; set; }
		ICoroutineRunner CoroutineRunner { get; set; }
		void PerformDash(Vector2 movementDirection);
	}

	public sealed class PlayerDashing : IDashingBehaviour
	{
		public const float DEFAULT_DISTANCE = 1;
		public const float DEFAULT_SPEED = 1;
		public const float DEFAULT_COOLDOWN = 0;

		private float Duration => Distance / Speed;
		private bool canDash = true;

		public float Distance { get; }
		public float Speed { get; }
		public float Cooldown { get; }
		public bool IsDashing { get; private set; }
		public AnimationCurve MovementCurve { get; }
		public ITransformProvider TransformProvider { get; set; }
		public ICoroutineRunner CoroutineRunner { get; set; }
		private ITimeService TimeService { get; }

		#region Constructors

		public PlayerDashing(float distance, float speed, float cooldown, AnimationCurve movementCurve = null,
			ITimeService timeService = null, ICoroutineRunner coroutineRunner = null,
			ITransformProvider transformProvider = null)
		{
			Distance = distance > 0 ? distance : DEFAULT_DISTANCE;
			Speed = speed > 0 ? speed : DEFAULT_SPEED;
			Cooldown = cooldown >= 0 ? cooldown : DEFAULT_COOLDOWN;
			MovementCurve = movementCurve ?? AnimationCurve.Linear(0, 0, 1, 1);
			TimeService = timeService ?? new UnityTimeService();
			CoroutineRunner = coroutineRunner ?? new DefaultCoroutineRunner();
			TransformProvider = transformProvider ??
				new UnityTransformProvider(new GameObject("PlayerMovement").transform);
		}

		public PlayerDashing(float distance, float speed, float cooldown, AnimationCurve movementCurve,
			ICoroutineRunner coroutineRunner, Transform transform) : this(distance, speed, cooldown,
			movementCurve, null, coroutineRunner, new UnityTransformProvider(transform)) {}

		public PlayerDashing(ITimeService timeService) : this(DEFAULT_DISTANCE, DEFAULT_SPEED, DEFAULT_COOLDOWN, null,
			timeService) {}

		public PlayerDashing(ITransformProvider transformProvider) : this(DEFAULT_DISTANCE, DEFAULT_SPEED,
			DEFAULT_COOLDOWN, null, null, null, transformProvider) {}

		public PlayerDashing() : this(DEFAULT_DISTANCE, DEFAULT_SPEED, DEFAULT_COOLDOWN) {}

		#endregion

		public void PerformDash(Vector2 movementDirection) => CoroutineRunner.StartCoroutine(Dash(movementDirection));

		private IEnumerator Dash(Vector2 movementDirection)
		{
			if (!canDash) yield break;
			canDash = false;
			IsDashing = true;
			movementDirection.Normalize();
			var startingDashDirection = movementDirection;
			var dashTime = 0f;
			var previousMovementProgress = 0f;
			while (dashTime < Duration)
			{
				dashTime += TimeService.DeltaTime;
				var dashMovementProgress = MovementCurve.Evaluate(dashTime / Duration) * Distance;
				var dashMovementVector = (dashMovementProgress - previousMovementProgress) * startingDashDirection;
				previousMovementProgress = dashMovementProgress;

				PerformDashMovement(dashMovementVector);
				yield return null;
			}

			IsDashing = false;
			if (Cooldown > 0) yield return new WaitForSeconds(Cooldown);
			canDash = true;
		}

		private void PerformDashMovement(Vector2 movementDirection)
		{
			if (movementDirection == Vector2.zero) return;
			TransformProvider.Translate(movementDirection, Space.World);
		}
	}
}