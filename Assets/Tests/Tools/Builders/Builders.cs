using Game;
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
	
	public abstract class InterfacedBuilder<T, I> where T : class, I
	{
		public I Interface => (I)Build();
		public abstract T Build();
		public static implicit operator T(InterfacedBuilder<T, I> builder) => builder.Build();
	}

	public sealed class PlayerMovementBuilder : InterfacedBuilder<PlayerMovement, IMovementBehaviour>
	{
		private float speed = -1;
		private ITimeService timeService;
		private ITransformProvider transformProvider;

		public PlayerMovementBuilder WithSpeed(float speed)
		{
			this.speed = speed;
			return this;
		}
		
		public PlayerMovementBuilder With(ITimeService timeService)
		{
			this.timeService = timeService;
			return this;
		}
		
		public PlayerMovementBuilder With(ITransformProvider transformProvider)
		{
			this.transformProvider = transformProvider;
			return this;
		}
		
		public PlayerMovementBuilder With(Transform transform)
		{
			transformProvider = new UnityTransformProvider(transform);
			return this;
		}

		public override PlayerMovement Build() => new PlayerMovement(speed, timeService, transformProvider);
	}

	public sealed class PlayerLookingBuilder : InterfacedBuilder<PlayerLooking, ILookingBehaviour>
	{
		public IWorldToScreenProvider worldToScreenProvider;
		private ITransformProvider transformProvider;
		
		public PlayerLookingBuilder With(IWorldToScreenProvider worldToScreenProvider)
		{
			this.worldToScreenProvider = worldToScreenProvider;
			return this;
		}
		
		public PlayerLookingBuilder With(ITransformProvider transformProvider)
		{
			this.transformProvider = transformProvider;
			return this;
		}
		
		public PlayerLookingBuilder With(Transform transform)
		{
			transformProvider = new UnityTransformProvider(transform);
			return this;
		}

		public override PlayerLooking Build() => new PlayerLooking(worldToScreenProvider, transformProvider);
	}

	public sealed class PlayerDashingBuilder : InterfacedBuilder<PlayerDashing, IDashingBehaviour>
	{
		private float distance = -1;
		private float speed = -1;
		private int cooldown = -1;
		private AnimationCurve dashSpeedCurve;
		private ITimeService timeService;
		private ICoroutineRunner coroutineRunner;
		private ITransformProvider transformProvider;

		public PlayerDashingBuilder WithDistance(float distance)
		{
			this.distance = distance;
			return this;
		}
		
		public PlayerDashingBuilder WithSpeed(float speed)
		{
			this.speed = speed;
			return this;
		}

		public PlayerDashingBuilder WithCooldown(int cooldown)
		{
			this.cooldown = cooldown;
			return this;
		}
		
		public PlayerDashingBuilder WithCurve(AnimationCurve dashSpeedCurve)
		{
			this.dashSpeedCurve = dashSpeedCurve;
			return this;
		}
		
		public PlayerDashingBuilder With(ITimeService timeService)
		{
			this.timeService = timeService;
			return this;
		}
		
		public PlayerDashingBuilder With(ICoroutineRunner coroutineRunner)
		{
			this.coroutineRunner = coroutineRunner;
			return this;
		}
		
		public PlayerDashingBuilder With(ITransformProvider transformProvider)
		{
			this.transformProvider = transformProvider;
			return this;
		}
		
		public PlayerDashingBuilder With(Transform transform)
		{
			transformProvider = new UnityTransformProvider(transform);
			return this;
		}

		public override PlayerDashing Build() => new PlayerDashing(distance, speed, cooldown, dashSpeedCurve,
			timeService, coroutineRunner, transformProvider);
	}

	public sealed class PlayerInputBuilder : InterfacedBuilder<PlayerInput, IInputBehaviour>
	{
		private InputMaster inputMaster;
		
		public PlayerInputBuilder With(InputMaster inputMaster)
		{
			this.inputMaster = inputMaster;
			return this;
		}

		public override PlayerInput Build() => new PlayerInput(inputMaster);
	}

	public sealed class UnityTransformProviderBuilder : InterfacedBuilder<UnityTransformProvider, ITransformProvider>
	{
		private Transform transform;

		public UnityTransformProviderBuilder With(Transform transform)
		{
			this.transform = transform;
			return this;
		}

		public override UnityTransformProvider Build() =>
			new UnityTransformProvider(transform ? transform : new GameObject().transform);
	}
	
	public sealed class UnityTimeServiceBuilder : InterfacedBuilder<UnityTimeService, ITimeService>
	{
		public override UnityTimeService Build() => new UnityTimeService();
	}

	public sealed class PlayerBuilder : Builder<Player>
	{
		private IInputBehaviour inputBehaviour;
		private IMovementBehaviour movementBehaviour;
		private IDashingBehaviour dashingBehaviour;
		private ILookingBehaviour lookingBehaviour;
		private ITransformProvider transformProvider;

		public PlayerBuilder With(IInputBehaviour inputBehaviour)
		{
			this.inputBehaviour = inputBehaviour;
			return this;
		}
		
		public PlayerBuilder With(IMovementBehaviour movementBehaviour)
		{
			this.movementBehaviour = movementBehaviour;
			return this;
		}
		
		public PlayerBuilder With(IDashingBehaviour dashingBehaviour)
		{
			this.dashingBehaviour = dashingBehaviour;
			return this;
		}
		
		public PlayerBuilder With(ILookingBehaviour lookingBehaviour)
		{
			this.lookingBehaviour = lookingBehaviour;
			return this;
		}
		
		public PlayerBuilder With(ITransformProvider transformProvider)
		{
			this.transformProvider = transformProvider;
			return this;
		}
		
		public PlayerBuilder With(Transform transform)
		{
			transformProvider = new UnityTransformProvider(transform);
			return this;
		}

		protected override Player Build() => new Player(transformProvider, inputBehaviour, movementBehaviour,
			dashingBehaviour, lookingBehaviour);
	}

	public sealed class CameraFollowBuilder : Builder<CameraFollow>
	{
		private ITransformProvider target;
		private ITransformProvider transformProvider;

		public CameraFollowBuilder WithTarget(ITransformProvider target)
		{
			this.target = target;
			return this;
		}

		public CameraFollowBuilder With(ITransformProvider transformProvider)
		{
			this.transformProvider = transformProvider;
			return this;
		}

		public CameraFollowBuilder With(Transform transform)
		{
			transformProvider = A.UnityTransformProvider.With(transform).Interface;
			return this;
		}

		protected override CameraFollow Build() => new CameraFollow(target, transformProvider);
	}
}