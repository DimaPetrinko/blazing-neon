using UnityEngine;

namespace Game.Players
{
	public interface IScreenToWorldPointProvider
	{
		Vector3 Get(Vector2 lookDirection, Vector3 position);
	}

	public sealed class ScreenToWorldPointProvider : IScreenToWorldPointProvider
	{
		private readonly Camera cam;

		public ScreenToWorldPointProvider(Camera cam) => this.cam = cam;

		public Vector3 Get(Vector2 lookDirection, Vector3 position) =>
			cam.ScreenToWorldPoint(new Vector3(lookDirection.x, lookDirection.y,
				position.z - cam.transform.position.z));
	}
}