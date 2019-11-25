using System.Collections;
using UnityEngine;

namespace Game.Players
{
	public interface IMovementBehaviour : IComponent
	{
		float Speed { get; }
		void PerformDash(Vector2 movementDirection);
		void PerformMovement(Vector2 movementDirection);
	}

	[RequireComponent(typeof(Rigidbody2D))]
	public sealed class PlayerMovement : TestableMonoBehaviour, IMovementBehaviour
	{
		[SerializeField] private float speed = 1f;
		[SerializeField] private float maxDashSpeed = 1f;
		[SerializeField] private float postDashCooldown;
		[SerializeField] private AnimationCurve dashSpeedCurve;

		private Vector2 dashDirection;
		private float currentDashSpeed;
		private bool isDashing;
		private bool canDash = true;

		public float Speed => speed;

		public void PerformDash(Vector2 movementDirection) => StartCoroutine(Dash(movementDirection));

		private IEnumerator Dash(Vector2 movementDirection)
		{
			if (!canDash || isDashing) yield break;
			movementDirection.Normalize();
			canDash = false;
			isDashing = true;
			dashDirection = movementDirection;
			var dashTime = 0f;
			var maxDashTime = dashSpeedCurve.keys[dashSpeedCurve.keys.Length - 1].time;
			while (dashTime < maxDashTime)
			{
				currentDashSpeed = dashSpeedCurve.Evaluate(dashTime) * maxDashSpeed + 1;
				dashTime += DeltaTime;
				yield return null;
			}

			dashDirection = Vector2.zero;
			isDashing = false;
			yield return new WaitForSeconds(postDashCooldown);
			canDash = true;
		}

		public void PerformMovement(Vector2 movementDirection)
		{
			movementDirection.Normalize();
			Vector2 movementVector;
			if (!isDashing) movementVector = speed * DeltaTime * movementDirection;
			else movementVector = DeltaTime * currentDashSpeed * dashDirection;

			transform.Translate(movementVector, Space.World);
		}
	}
}