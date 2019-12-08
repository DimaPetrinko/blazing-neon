using Game.GameSystemServices;
using Game.GameSystemServices.DataReferences;
using Game.GameSystemServices.TransformProviders;
using UnityEngine;

namespace Game.CameraSystem
{
	public interface ICameraFollow
	{
		FloatReference Smoothing { get; }
		TransformProviderReference Target { get; }
		ITransformProvider TransformProvider { get; set; }
		void Update();
		void SetTarget(ITransformProvider target);
		void ClearTarget();
	}

	public sealed class CameraFollow : ICameraFollow
	{
		public ITimeService TimeService { get; }
		public FloatReference Smoothing { get; }
		public TransformProviderReference Target { get; }
		public ITransformProvider TransformProvider { get; set; }

		#region Constructors

		public CameraFollow(FloatReference smoothing, ITransformProvider transformProvider = null,
			TransformProviderReference target = null, ITimeService timeService = null)
		{
			Smoothing = smoothing;
			Target = target;
			TransformProvider =
				transformProvider ?? new UnityTransformProvider(new GameObject("PlayerMovement").transform);
			TimeService = timeService ?? new UnityTimeService();
		}

		// public CameraFollow(FloatReference smoothing, ITransformProvider transformProvider = null,
		// 	ISceneObject target = null, ITimeService timeService = null) : this(smoothing, transformProvider,
		// 	target.TransformProvider, timeService) {}

		#endregion

		public void Update()
		{
			if (Target.Value == null) return;
			TransformProvider.Position = Smoothing.Value > 0 ? GetSmoothedPosition() : GetMatchPosition();
		}

		public void SetTarget(ITransformProvider target) => Target.Value = target;
		public void ClearTarget() => Target.Value = null;

		private Vector3 GetSmoothedPosition()
		{
			var targetPosition = new Vector3(Target.Value.Position.x, Target.Value.Position.y,
				TransformProvider.Position.z);
			return Vector3.Lerp(TransformProvider.Position, targetPosition, Smoothing.Value * TimeService.DeltaTime);
		}

		private Vector3 GetMatchPosition() => new Vector3(Target.Value.Position.x, Target.Value.Position.y,
			TransformProvider.Position.z);

		public void SetTarget(ISceneObject target) => Target.Value = target.TransformProvider;
	}
}