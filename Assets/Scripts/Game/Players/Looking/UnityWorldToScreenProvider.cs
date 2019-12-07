using UnityEngine;

namespace Game.Players.Looking
{
	public interface IWorldToScreenProvider
	{
		Vector2 Get(Vector2 worldPosition);
	}

	public sealed class UnityWorldToScreenProvider : IWorldToScreenProvider
	{
		private readonly Camera _camera;
		public UnityWorldToScreenProvider(Camera camera) => _camera = camera;
		public Vector2 Get(Vector2 worldPosition) => _camera.WorldToScreenPoint(worldPosition);
	}
}