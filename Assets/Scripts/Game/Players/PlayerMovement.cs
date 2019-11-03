using System.Collections;
using UnityEngine;

namespace Game.Players
{
	[RequireComponent(typeof(Rigidbody2D))]
	public sealed class PlayerMovement : MonoBehaviour
	{
		[SerializeField] private float speed = 1f;
		[SerializeField] private float maxDashSpeed = 1f;
		[SerializeField] private float postDashCooldown;
		[SerializeField] private AnimationCurve dashSpeedCurve;

		private Rigidbody2D rb;
		private Vector2 dashDirection;
		private float currentDashSpeed;
		private bool isDashing;
		private bool canDash = true;

		// TODO: stack of movement modes?

		private void Awake() => rb = GetComponent<Rigidbody2D>();

		public void PerformDash(Vector2 movementDirection) => StartCoroutine(Dash(movementDirection));

		private IEnumerator Dash(Vector2 movementDirection)
		{
			if (!canDash || isDashing) yield break;
			canDash = false;
			isDashing = true;
			dashDirection = movementDirection;
			var dashTime = 0f;
			var maxDashTime = dashSpeedCurve.keys[dashSpeedCurve.keys.Length - 1].time;
			while (dashTime < maxDashTime)
			{
				currentDashSpeed = dashSpeedCurve.Evaluate(dashTime) * maxDashSpeed + 1;
				dashTime += Time.deltaTime;
				yield return null;
			}

			dashDirection = Vector2.zero;
			isDashing = false;
			yield return new WaitForSeconds(postDashCooldown);
			canDash = true;
		}

		public void PerformMovement(Vector2 movementDirection)
		{
			Vector2 finalMovementDirection;
			if (!isDashing) finalMovementDirection = speed * Time.deltaTime * movementDirection;
			else finalMovementDirection = Time.deltaTime * currentDashSpeed * dashDirection;

			rb.MovePosition(rb.position + finalMovementDirection);
		}
	}
}