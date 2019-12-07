using Game.CameraSystem;
using Game.GameSystemServices;
using Game.GameSystemServices.CoroutineRunners;
using Game.GameSystemServices.TransformProviders;
using Game.Players;
using Game.Players.Dashing;
using Game.Players.Input;
using Game.Players.Looking;
using Game.Players.Movement;
using Input;
using UnityEngine;

namespace Tests.Tools.Builders
{
	public static class A
	{
		public static PlayerBuilder Player => new PlayerBuilder();
		public static PlayerInputBuilder PlayerInput => new PlayerInputBuilder();
		public static PlayerMovementBuilder PlayerMovement => new PlayerMovementBuilder();
		public static PlayerDashingBuilder PlayerDashing => new PlayerDashingBuilder();
		public static PlayerLookingBuilder PlayerLooking => new PlayerLookingBuilder();
		public static UnityTransformProviderBuilder UnityTransformProvider => new UnityTransformProviderBuilder();
		public static UnityTimeServiceBuilder UnityTimeService => new UnityTimeServiceBuilder();
		public static CameraFollowBuilder CameraFollow => new CameraFollowBuilder();
	}

	public abstract class Builder<T> where T : class
	{
		protected abstract T Build();
		public static implicit operator T(Builder<T> builder) => builder.Build();
	}

	public abstract class InterfacedBuilder<TClass, TInterface> where TClass : class, TInterface
	{
		public TInterface Interface => (TInterface)Build();
		protected abstract TClass Build();
		public static implicit operator TClass(InterfacedBuilder<TClass, TInterface> builder) => builder.Build();
	}

	public sealed class PlayerMovementBuilder : InterfacedBuilder<PlayerMovement, IMovementBehaviour>
	{
		private float _speed = -1;
		private ITimeService _timeService;
		private ITransformProvider _transformProvider;

		public PlayerMovementBuilder WithSpeed(float speed)
		{
			_speed = speed;
			return this;
		}

		public PlayerMovementBuilder With(ITimeService timeService)
		{
			_timeService = timeService;
			return this;
		}

		public PlayerMovementBuilder With(ITransformProvider transformProvider)
		{
			_transformProvider = transformProvider;
			return this;
		}

		public PlayerMovementBuilder With(Transform transform)
		{
			_transformProvider = new UnityTransformProvider(transform);
			return this;
		}

		protected override PlayerMovement Build() => new PlayerMovement(_speed, _timeService, _transformProvider);
	}

	public sealed class PlayerLookingBuilder : InterfacedBuilder<PlayerLooking, ILookingBehaviour>
	{
		public IWorldToScreenProvider _worldToScreenProvider;
		private ITransformProvider _transformProvider;

		public PlayerLookingBuilder With(IWorldToScreenProvider worldToScreenProvider)
		{
			_worldToScreenProvider = worldToScreenProvider;
			return this;
		}

		public PlayerLookingBuilder With(ITransformProvider transformProvider)
		{
			_transformProvider = transformProvider;
			return this;
		}

		public PlayerLookingBuilder With(Transform transform)
		{
			_transformProvider = new UnityTransformProvider(transform);
			return this;
		}

		protected override PlayerLooking Build() => new PlayerLooking(_worldToScreenProvider, _transformProvider);
	}

	public sealed class PlayerDashingBuilder : InterfacedBuilder<PlayerDashing, IDashingBehaviour>
	{
		private float _distance = -1;
		private float _speed = -1;
		private int _cooldown = -1;
		private AnimationCurve _dashSpeedCurve;
		private ITimeService _timeService;
		private ICoroutineRunner _coroutineRunner;
		private ITransformProvider _transformProvider;

		public PlayerDashingBuilder WithDistance(float distance)
		{
			_distance = distance;
			return this;
		}

		public PlayerDashingBuilder WithSpeed(float speed)
		{
			_speed = speed;
			return this;
		}

		public PlayerDashingBuilder WithCooldown(int cooldown)
		{
			_cooldown = cooldown;
			return this;
		}

		public PlayerDashingBuilder WithCurve(AnimationCurve dashSpeedCurve)
		{
			_dashSpeedCurve = dashSpeedCurve;
			return this;
		}

		public PlayerDashingBuilder With(ITimeService timeService)
		{
			_timeService = timeService;
			return this;
		}

		public PlayerDashingBuilder With(ICoroutineRunner coroutineRunner)
		{
			_coroutineRunner = coroutineRunner;
			return this;
		}

		public PlayerDashingBuilder With(ITransformProvider transformProvider)
		{
			_transformProvider = transformProvider;
			return this;
		}

		public PlayerDashingBuilder With(Transform transform)
		{
			_transformProvider = new UnityTransformProvider(transform);
			return this;
		}

		protected override PlayerDashing Build() => new PlayerDashing(_distance, _speed, _cooldown, _dashSpeedCurve,
			_timeService, _coroutineRunner, _transformProvider);
	}

	public sealed class PlayerInputBuilder : InterfacedBuilder<PlayerInput, IInputBehaviour>
	{
		private InputMaster _inputMaster;

		public PlayerInputBuilder With(InputMaster inputMaster)
		{
			_inputMaster = inputMaster;
			return this;
		}

		protected override PlayerInput Build() => new PlayerInput(_inputMaster);
	}

	public sealed class UnityTransformProviderBuilder : InterfacedBuilder<UnityTransformProvider, ITransformProvider>
	{
		private Transform _transform;

		public UnityTransformProviderBuilder With(Transform transform)
		{
			_transform = transform;
			return this;
		}

		protected override UnityTransformProvider Build() =>
			new UnityTransformProvider(_transform ? _transform : new GameObject().transform);
	}

	public sealed class UnityTimeServiceBuilder : InterfacedBuilder<UnityTimeService, ITimeService>
	{
		protected override UnityTimeService Build() => new UnityTimeService();
	}

	public sealed class PlayerBuilder : Builder<Player>
	{
		private IInputBehaviour _inputBehaviour;
		private IMovementBehaviour _movementBehaviour;
		private IDashingBehaviour _dashingBehaviour;
		private ILookingBehaviour _lookingBehaviour;
		private ITransformProvider _transformProvider;

		public PlayerBuilder With(IInputBehaviour inputBehaviour)
		{
			_inputBehaviour = inputBehaviour;
			return this;
		}

		public PlayerBuilder With(IMovementBehaviour movementBehaviour)
		{
			_movementBehaviour = movementBehaviour;
			return this;
		}

		public PlayerBuilder With(IDashingBehaviour dashingBehaviour)
		{
			_dashingBehaviour = dashingBehaviour;
			return this;
		}

		public PlayerBuilder With(ILookingBehaviour lookingBehaviour)
		{
			_lookingBehaviour = lookingBehaviour;
			return this;
		}

		public PlayerBuilder With(ITransformProvider transformProvider)
		{
			_transformProvider = transformProvider;
			return this;
		}

		public PlayerBuilder With(Transform transform)
		{
			_transformProvider = new UnityTransformProvider(transform);
			return this;
		}

		protected override Player Build() => new Player(_transformProvider, _inputBehaviour, _movementBehaviour,
			_dashingBehaviour, _lookingBehaviour);
	}

	public sealed class CameraFollowBuilder : InterfacedBuilder<CameraFollow, ICameraFollow>
	{
		private FloatReference _smoothing;
		private ITransformProvider _target;
		private ITransformProvider _transformProvider;

		public CameraFollowBuilder WithSmoothing(FloatReference smoothing)
		{
			_smoothing = smoothing;
			return this;
		}

		public CameraFollowBuilder WithSmoothing(float smoothing)
		{
			_smoothing = new FloatReference(smoothing);
			return this;
		}

		public CameraFollowBuilder WithTarget(ISceneObject target)
		{
			_target = target.TransformProvider;
			return this;
		}

		public CameraFollowBuilder WithTarget(ITransformProvider target)
		{
			_target = target;
			return this;
		}

		public CameraFollowBuilder With(ITransformProvider transformProvider)
		{
			_transformProvider = transformProvider;
			return this;
		}

		public CameraFollowBuilder With(Transform transform)
		{
			_transformProvider = A.UnityTransformProvider.With(transform).Interface;
			return this;
		}

		protected override CameraFollow Build() => new CameraFollow(_smoothing, _transformProvider, _target);
	}
}