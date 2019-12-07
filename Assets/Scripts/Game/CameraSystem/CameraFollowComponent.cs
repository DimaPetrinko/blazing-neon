using Game.GameSystemServices;
using Game.GameSystemServices.TransformProviders;
using UnityEngine;

namespace Game.CameraSystem
{
	public sealed class CameraFollowComponent : BaseComponent<CameraFollow>
	{
		[Header("Camera follow component")]
		[SerializeField] private BaseComponent _target;
		[SerializeField] private FloatReference _smoothing;

		protected override CameraFollow CreateInstance() => new CameraFollow(_smoothing,
			new UnityTransformProvider(transform), _target?.BaseInstance as ISceneObject);

		private void FixedUpdate() => Instance.Update();
	}
}