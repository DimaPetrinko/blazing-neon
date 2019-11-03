using UnityEngine;

namespace Game.Players
{
	[RequireComponent(typeof(Rigidbody2D))]
	public sealed class PlayerLooking : MonoBehaviour
	{
		private Rigidbody2D rb;
		private Camera cam;

		private void Awake()
		{
			rb = GetComponent<Rigidbody2D>();
			cam = Camera.main;
		}

		public void PerformLooking(Vector2 lookDirection)
		{
			if (lookDirection == Vector2.zero) return;
			float angle;
			// TODO: if this is mouse input
			if (lookDirection.x > 1 || lookDirection.y > 1 || lookDirection.x < -1 || lookDirection.y < -1)
			{
				var position = transform.position;
				var worldPoint = cam.ScreenToWorldPoint(new Vector3(lookDirection.x, lookDirection.y,
					position.z - cam.transform.position.z));
				angle = Vector3.SignedAngle(Vector3.up, worldPoint - position, Vector3.forward);
			}
			else angle = Vector3.SignedAngle(Vector3.up, lookDirection, Vector3.forward);

			rb.MoveRotation(angle);
		}
	}
}