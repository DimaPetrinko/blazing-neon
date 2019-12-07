using Game.GameSystemServices;
using Game.GameSystemServices.TransformProviders;
using UnityEngine;

namespace Game.CameraSystem
{
	public interface ICameraFollow
	{
		FloatReference Smoothing { get; }
		ITransformProvider Target { get; }
		ITransformProvider TransformProvider { get; set; }
		void Update();
		void SetTarget(ITransformProvider target);
		void ClearTarget();
	}

	public sealed class CameraFollow : ICameraFollow
	{
		public FloatReference Smoothing { get; }
		public ITimeService TimeService { get; }
		public ITransformProvider Target { get; private set; }
		public ITransformProvider TransformProvider { get; set; }

		#region Constructors

		public CameraFollow(FloatReference smoothing, ITransformProvider transformProvider = null,
			ITransformProvider target = null, ITimeService timeService = null)
		{
			Smoothing = smoothing;
			Target = target;
			TransformProvider = transformProvider ??
				new UnityTransformProvider(new GameObject("PlayerMovement").transform);
			TimeService = timeService ?? new UnityTimeService();
		}

		public CameraFollow(FloatReference smoothing, ITransformProvider transformProvider = null, ISceneObject target = null,
			ITimeService timeService = null) : this(smoothing, transformProvider, target.TransformProvider,
			timeService) {}

		#endregion

		public void Update()
		{
			if (Target == null) return;
			Debug.Log(Target.GameObject.name);
			TransformProvider.Position = Smoothing.Value > 0 ? GetSmoothedPosition() : GetMatchPosition();
		}

		private Vector3 GetSmoothedPosition()
		{
			var targetPosition = new Vector3(Target.Position.x, Target.Position.y, TransformProvider.Position.z);
			return Vector3.Lerp(TransformProvider.Position, targetPosition, Smoothing.Value * TimeService.DeltaTime);
		}

		private Vector3 GetMatchPosition() => new Vector3(Target.Position.x, Target.Position.y, TransformProvider.Position.z);

		public void SetTarget(ISceneObject target) => Target = target.TransformProvider;
		public void SetTarget(ITransformProvider target) => Target = target;
		public void ClearTarget() => Target = null;
	}
}