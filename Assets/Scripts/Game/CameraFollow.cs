using Game.GameSystemServices.TransformProviders;
using UnityEngine;

namespace Game
{
	public sealed class CameraFollow
	{
		public ITransformProvider Target { get; }
		public ITransformProvider TransformProvider { get; set; }

		public CameraFollow(ITransformProvider target, ITransformProvider transformProvider = null)
		{
			Target = target;
			TransformProvider = transformProvider ??
				new UnityTransformProvider(new GameObject("PlayerMovement").transform);
		}

		public void LateUpdate()
		{
			if (Target == null) return;
			TransformProvider.Position = Target.Position;
		}
	}
}