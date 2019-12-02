using UnityEngine;

namespace Game.Players.Looking
{
	public interface IWorldToScreenProvider
	{
		Vector2 Get(Vector2 worldPosition);
	}

	public sealed class UnityWorldToScreenProvider : IWorldToScreenProvider
	{
		private readonly Camera camera;

		public UnityWorldToScreenProvider(Camera camera) => this.camera = camera;

		public Vector2 Get(Vector2 worldPosition) => camera.WorldToScreenPoint(worldPosition);
	}
}