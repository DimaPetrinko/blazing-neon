using Game.GameSystemServices;
using Game.GameSystemServices.TransformProviders;
using UnityEngine;

namespace Game.CameraSystem
{
	public interface ICameraFollow
	{
		float Smoothing { get; }
		ITimeService TimeService { get; }
		ITransformProvider Target { get; }
		ITransformProvider TransformProvider { get; set; }
		void Update();
		void SetTarget(ITransformProvider target);
		void ClearTarget();
	}

	public sealed class CameraFollow : ICameraFollow
	{
		public float Smoothing { get; set; }
		public ITimeService TimeService { get; }
		public ITransformProvider Target { get; private set; }
		public ITransformProvider TransformProvider { get; set; }

		#region Constructors

		public CameraFollow(float smoothing, ITransformProvider transformProvider = null,
			ITransformProvider target = null, ITimeService timeService = null)
		{
			Smoothing = smoothing;
			Target = target;
			TransformProvider = transformProvider ??
				new UnityTransformProvider(new GameObject("PlayerMovement").transform);
			TimeService = timeService ?? new UnityTimeService();
		}

		public CameraFollow(float smoothing, ITransformProvider transformProvider = null, ISceneObject target = null,
			ITimeService timeService = null) : this(smoothing, transformProvider, target.TransformProvider,
			timeService) {}

		#endregion

		public void Update()
		{
			if (Target == null) return;
			TransformProvider.Position = Smoothing > 0 ? GetSmoothedPosition() : GetMatchPosition();
		}

		private Vector3 GetSmoothedPosition()
		{
			var targetPosition = new Vector3(Target.Position.x, Target.Position.y, TransformProvider.Position.z);
			return Vector3.Lerp(TransformProvider.Position, targetPosition, Smoothing * TimeService.DeltaTime);
		}

		private Vector3 GetMatchPosition() => new Vector3(Target.Position.x, Target.Position.y, TransformProvider.Position.z);

		public void SetTarget(ISceneObject target) => Target = target.TransformProvider;
		public void SetTarget(ITransformProvider target) => Target = target;
		public void ClearTarget() => Target = null;
	}
}