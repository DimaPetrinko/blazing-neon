using Game.GameSystemServices;
using Game.GameSystemServices.TransformProviders;
using UnityEngine;

namespace Game.CameraSystem
{
	public sealed class CameraFollowComponent : BaseComponent<CameraFollow>
	{
		[Header("Camera follow component")]
		[SerializeField] private BaseComponent target;
		[SerializeField] private float smoothing;

		protected override CameraFollow CreateInstance() => new CameraFollow(smoothing,
			new UnityTransformProvider(transform), target?.BaseInstance as ISceneObject);

		private void FixedUpdate() => Instance.Update();
	}
}