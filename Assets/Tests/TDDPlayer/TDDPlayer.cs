using System;
using Game.GameSystemServices;
using Game.GameSystemServices.CoroutineRunners;
using Game.GameSystemServices.TransformProviders;
using Game.Players;
using Game.Players.Dashing;
using Game.Players.Input;
using Game.Players.Looking;
using Game.Players.Movement;
using NSubstitute;
using NUnit.Framework;
using Tests.Tools.Builders;
using UnityEngine;
using DeviceType = Game.Players.Input.DeviceType;
using Random = UnityEngine.Random;

namespace Tests.TDDPlayer
{
	public static class TDDPlayer
	{
		public sealed class Initialization
		{
			private static Player _player;

			[Test]
			public void default_input_behaviour_is_not_null()
			{
				_player = A.Player;
				Assert.NotNull(_player.InputBehaviour);
			}

			[Test]
			public void default_movement_behaviour_is_not_null()
			{
				_player = A.Player;
				Assert.NotNull(_player.MovementBehaviour);
			}

			[Test]
			public void default_looking_behaviour_is_not_null()
			{
				_player = A.Player;
				Assert.NotNull(_player.LookingBehaviour);
			}

			[Test]
			public void using_default_constructor_TransformProvider_is_the_same_for_player_and_his_components()
			{
				_player = A.Player;

				Assert.AreEqual(_player.TransformProvider, _player.MovementBehaviour.TransformProvider);
				Assert.AreEqual(_player.TransformProvider, _player.LookingBehaviour.TransformProvider);
			}

			[Test]
			public void using_TransformProvider_constructor_overload_TransformProvider_is_the_same_for_player_and_his_components()
			{
				_player = A.Player.With(A.UnityTransformProvider.Interface);

				Assert.AreEqual(_player.TransformProvider, _player.MovementBehaviour.TransformProvider);
				Assert.AreEqual(_player.TransformProvider, _player.LookingBehaviour.TransformProvider);
			}

			[Test]
			public void using_TransformProvider_constructor_overload_with_parameters_TransformProvider_is_the_same_for_player_and_his_components()
			{
				_player = A.Player.With(A.UnityTransformProvider.Interface).With(A.PlayerInput.Interface)
					.With(A.PlayerMovement.Interface).With(A.PlayerLooking.Interface);

				Assert.AreEqual(_player.TransformProvider, _player.MovementBehaviour.TransformProvider);
				Assert.AreEqual(_player.TransformProvider, _player.LookingBehaviour.TransformProvider);
			}

			[Test]
			public void using_Transform_constructor_overload_TransformProvider_is_the_same_for_player_and_his_components()
			{
				_player = A.Player.With(A.UnityTransformProvider.Interface);

				Assert.AreEqual(_player.TransformProvider, _player.MovementBehaviour.TransformProvider);
				Assert.AreEqual(_player.TransformProvider, _player.LookingBehaviour.TransformProvider);
			}

			[Test]
			public void using_Transform_constructor_overload_with_parameters_TransformProvider_is_the_same_for_player_and_his_components()
			{
				_player = A.Player.With(A.UnityTransformProvider.Interface).With(A.PlayerInput.Interface)
					.With(A.PlayerMovement.Interface).With(A.PlayerLooking.Interface);

				Assert.AreEqual(_player.TransformProvider, _player.MovementBehaviour.TransformProvider);
				Assert.AreEqual(_player.TransformProvider, _player.LookingBehaviour.TransformProvider);
			}

			[Test]
			public void MovementBehaviour_is_subscribed_to_Dash_event()
			{
				var inputBehaviourSubstitute = Substitute.For<IInputBehaviour>();
				var dashBehaviourSubstitute = Substitute.For<IDashingBehaviour>();
				_player = A.Player.With(inputBehaviourSubstitute).With(dashBehaviourSubstitute);

				_player.InputBehaviour.Dash += Raise.EventWith(new Vector2EventArgs(Vector2.zero));

				_player.DashingBehaviour.Received().PerformDash(Arg.Any<Vector2>());
			}

			[Test]
			public void FixedUpdate_moves_player_according_to_input()
			{
				var inputBehaviourSubstitute = Substitute.For<IInputBehaviour>();
				inputBehaviourSubstitute.MovementDirection.Returns(Vector2.right);
				var timeServiceSubstitute = Substitute.For<ITimeService>();
				timeServiceSubstitute.DeltaTime.Returns(1);
				_player = A.Player.With(inputBehaviourSubstitute)
					.With(A.PlayerMovement.WithSpeed(1).With(timeServiceSubstitute).Interface);
				_player.TransformProvider.Position = Vector2.zero;

				_player.FixedUpdate();

				Assert.AreEqual(Vector3.right, _player.TransformProvider.Position);
			}

			[Test]
			public void FixedUpdate_rotates_player_according_to_mouse_input()
			{
				var inputBehaviourSubstitute = Substitute.For<IInputBehaviour>();
				inputBehaviourSubstitute.LookDeviceType.Returns(DeviceType.Mouse);
				inputBehaviourSubstitute.LookDirection.Returns(new Vector2(Screen.width, Screen.height / 2f));
				var worldToScreenProvider = Substitute.For<IWorldToScreenProvider>();
				worldToScreenProvider.Get(Arg.Any<Vector2>())
					.Returns(new Vector2(Screen.width / 2f, Screen.height / 2f));
				_player = A.Player.With(inputBehaviourSubstitute)
					.With(A.PlayerLooking.With(worldToScreenProvider).Interface);
				_player.TransformProvider.Rotation = Quaternion.Euler(0, 0, 0);
				var supposedRotationEuler = Quaternion.FromToRotation(Vector3.up, new Vector2(1, 0)).eulerAngles;

				_player.FixedUpdate();

				Assert.AreEqual(supposedRotationEuler, _player.TransformProvider.Rotation.eulerAngles);
			}

			[Test]
			public void FixedUpdate_rotates_player_according_to_gamepad_input()
			{
				var inputBehaviourSubstitute = Substitute.For<IInputBehaviour>();
				inputBehaviourSubstitute.LookDeviceType.Returns(DeviceType.Gamepad);
				inputBehaviourSubstitute.LookDirection.Returns(Vector2.right);
				_player = A.Player.With(inputBehaviourSubstitute).With(A.PlayerLooking.Interface);
				_player.TransformProvider.Rotation = Quaternion.Euler(0, 0, 0);
				var supposedRotationEuler = Quaternion.FromToRotation(Vector3.up, new Vector2(1, 0)).eulerAngles;

				_player.FixedUpdate();

				Assert.AreEqual(supposedRotationEuler, _player.TransformProvider.Rotation.eulerAngles);
			}

			[Test]
			public void FixedUpdate_when_LookingDeviceType_is_not_mouse_nor_gamepad_does_not_rotate_player()
			{
				var inputBehaviourSubstitute = Substitute.For<IInputBehaviour>();
				inputBehaviourSubstitute.LookDeviceType.Returns(DeviceType.Keyboard);
				inputBehaviourSubstitute.LookDirection.Returns(Vector2.right);
				_player = A.Player.With(inputBehaviourSubstitute).With(A.PlayerLooking.Interface);
				_player.TransformProvider.Rotation = Quaternion.Euler(0, 0, 0);
				var originalRotationEuler = _player.TransformProvider.Rotation.eulerAngles;

				_player.FixedUpdate();

				Assert.AreEqual(originalRotationEuler, _player.TransformProvider.Rotation.eulerAngles);
			}

			[Test]
			public void while_not_dashing_can_move()
			{
				var dashingBehaviourSubstitute = Substitute.For<IDashingBehaviour>();
				dashingBehaviourSubstitute.IsDashing.Returns(false);
				var timeServiceSubstitute = Substitute.For<ITimeService>();
				timeServiceSubstitute.DeltaTime.Returns(1);
				var inputBehaviourSubstitute = Substitute.For<IInputBehaviour>();
				inputBehaviourSubstitute.MovementDirection.Returns(Vector2.right);
				_player = A.Player.With(A.PlayerMovement.With(timeServiceSubstitute).Interface)
					.With(dashingBehaviourSubstitute).With(inputBehaviourSubstitute);
				var originalPosition = new Vector3(5, 2);
				_player.TransformProvider.Position = originalPosition;

				_player.FixedUpdate();

				Assert.AreNotEqual(originalPosition, _player.TransformProvider.Position);
			}

			[Test]
			public void while_dashing_cannot_move()
			{
				var dashingBehaviourSubstitute = Substitute.For<IDashingBehaviour>();
				dashingBehaviourSubstitute.IsDashing.Returns(true);
				var timeServiceSubstitute = Substitute.For<ITimeService>();
				timeServiceSubstitute.DeltaTime.Returns(1);
				var inputBehaviourSubstitute = Substitute.For<IInputBehaviour>();
				inputBehaviourSubstitute.MovementDirection.Returns(Vector2.right);
				_player = A.Player.With(A.PlayerMovement.With(timeServiceSubstitute).Interface)
					.With(dashingBehaviourSubstitute).With(inputBehaviourSubstitute);
				var originalPosition = new Vector3(5, 2);
				_player.TransformProvider.Position = originalPosition;

				_player.FixedUpdate();

				Assert.AreEqual(originalPosition, _player.TransformProvider.Position);
			}
		}

		public sealed class TimeServiceProperty
		{
			private ITimeService _timeService;

			[SetUp]
			public void TimeServiceSetup() => _timeService = A.UnityTimeService.Interface;

			[Test]
			public void default_TimeService_is_not_null() => Assert.NotNull(_timeService);

			[Test]
			public void default_DeltaTime_is_greater_than_0() => Assert.Greater(_timeService.DeltaTime, 0);

			[Test]
			public void default_Time_is_greater_than_0() => Assert.Greater(_timeService.Time, 0);
		}

		public sealed class Input
		{
			private static IInputBehaviour _inputBehaviour;

			[SetUp]
			public static void InputSetup() => _inputBehaviour = A.PlayerInput.Interface;

			public sealed class LookDirectionProperty
			{
				[SetUp]
				public void LookDirectionSetup() => InputSetup();

				[Test]
				public void default_LookDirection_is_Vector2_zero() =>
					Assert.AreEqual(Vector2.zero, _inputBehaviour.LookDirection);

				[Test]
				public void default_LookingDeviceType_is_None() =>
					Assert.AreEqual(DeviceType.None, _inputBehaviour.LookDeviceType);

				[Test]
				public void SetLookDirection_for_some_DeviceType_updates_LookDirection_and_LookingDeviceType()
				{
					var originalLookDirection = _inputBehaviour.LookDirection;

					_inputBehaviour.SetLookDirection(new Vector2(10, 2), DeviceType.Mouse);

					Assert.AreNotEqual(originalLookDirection, _inputBehaviour.LookDirection);
					Assert.AreEqual(DeviceType.Mouse, _inputBehaviour.LookDeviceType);
				}

				[Test]
				public void SetLookDirection_for_Keyboard_does_nothing()
				{
					var originalLookDirection = _inputBehaviour.LookDirection;
					var originalDeviceType = _inputBehaviour.LookDeviceType;

					_inputBehaviour.SetLookDirection(new Vector2(10, 2), DeviceType.Keyboard);

					Assert.AreEqual(originalLookDirection, _inputBehaviour.LookDirection);
					Assert.AreEqual(originalDeviceType, _inputBehaviour.LookDeviceType);
				}

				[Test]
				public void SetLookDirection_normalizes_input()
				{
					var newLookDirection = new Vector2(10, 2);

					_inputBehaviour.SetLookDirection(newLookDirection, DeviceType.Mouse);

					Assert.AreEqual(newLookDirection.normalized, _inputBehaviour.LookDirection);
				}
			}

			public sealed class MovementDirectionProperty
			{
				[SetUp]
				public void MovementDirectionSetup() => InputSetup();

				[Test]
				public void default_MovementDirection_is_Vector2_zero() =>
					Assert.AreEqual(Vector2.zero, _inputBehaviour.MovementDirection);

				[Test]
				public void SetMovementDirection_updates_MovementDirection()
				{
					var originalMovementDirection = _inputBehaviour.MovementDirection;

					_inputBehaviour.SetMovementDirection(new Vector2(10, 2));

					Assert.AreNotEqual(originalMovementDirection, _inputBehaviour.MovementDirection);
				}

				[Test]
				public void SetMovementDirection_normalizes_input()
				{
					var newMovementDirection = new Vector2(10, 2);

					_inputBehaviour.SetMovementDirection(newMovementDirection);

					Assert.AreEqual(newMovementDirection.normalized, _inputBehaviour.MovementDirection);
				}
			}
		}

		public sealed class Movement
		{
			private IMovementBehaviour _movementBehaviour;

			[Test]
			public void default_Speed_is_greater_than_0()
			{
				_movementBehaviour = A.PlayerMovement.Interface;

				Assert.Greater(_movementBehaviour.Speed, 0);
			}

			[Test]
			public void default_Speed_has_the_value_of_DEFAULT_SPEED_constant()
			{
				_movementBehaviour = A.PlayerMovement.Interface;

				Assert.AreEqual(PlayerMovement.DEFAULT_SPEED, _movementBehaviour.Speed);
			}

			[Test]
			public void Vector2_zero_does_nothing()
			{
				_movementBehaviour = A.PlayerMovement.Interface;
				_movementBehaviour.TransformProvider.Position = new Vector2(2, 5);

				_movementBehaviour.PerformMovement(Vector2.zero);

				Assert.AreEqual(new Vector3(2, 5), _movementBehaviour.TransformProvider.Position);
			}

			[Test]
			public void Vector2_right_with_Speed_1_and_DeltaTime_1_moves_1_unit_to_the_right()
			{
				var timeServiceSubstitute = Substitute.For<ITimeService>();
				timeServiceSubstitute.DeltaTime.Returns(1);
				_movementBehaviour = A.PlayerMovement.WithSpeed(1).With(timeServiceSubstitute).Interface;
				_movementBehaviour.TransformProvider.Position = Vector2.zero;

				_movementBehaviour.PerformMovement(Vector2.right);

				Assert.AreEqual(new Vector3(1, 0), _movementBehaviour.TransformProvider.Position);
			}

			[Test]
			public void Vector2_up_with_Speed_1_and_DeltaTime_1_moves_1_unit_vertically()
			{
				var timeServiceSubstitute = Substitute.For<ITimeService>();
				timeServiceSubstitute.DeltaTime.Returns(1);
				_movementBehaviour = A.PlayerMovement.WithSpeed(1).With(timeServiceSubstitute).Interface;
				_movementBehaviour.TransformProvider.Position = Vector2.zero;

				_movementBehaviour.PerformMovement(Vector2.up);

				Assert.AreEqual(new Vector3(0, 1), _movementBehaviour.TransformProvider.Position);
			}

			[Test]
			public void Vector2_right_with_Speed_5_and_DeltaTime_1_moves_1_unit_to_the_right()
			{
				var timeServiceSubstitute = Substitute.For<ITimeService>();
				timeServiceSubstitute.DeltaTime.Returns(1);
				_movementBehaviour = A.PlayerMovement.WithSpeed(5).With(timeServiceSubstitute).Interface;
				_movementBehaviour.TransformProvider.Position = Vector2.zero;

				_movementBehaviour.PerformMovement(Vector2.right);

				Assert.AreEqual(new Vector3(5, 0), _movementBehaviour.TransformProvider.Position);
			}

			[Test]
			public void Vector2_up_with_Speed_5_and_DeltaTime_1_moves_1_unit_vertically()
			{
				var timeServiceSubstitute = Substitute.For<ITimeService>();
				timeServiceSubstitute.DeltaTime.Returns(1);
				_movementBehaviour = A.PlayerMovement.WithSpeed(5).With(timeServiceSubstitute).Interface;
				_movementBehaviour.TransformProvider.Position = Vector2.zero;

				_movementBehaviour.PerformMovement(Vector2.up);

				Assert.AreEqual(new Vector3(0, 5), _movementBehaviour.TransformProvider.Position);
			}

			[Test]
			public void random_Vector2_with_only_x_coordinate_with_Speed_1_and_DeltaTime_1_still_moves_1_unit_in_the_correct_direction()
			{
				var timeServiceSubstitute = Substitute.For<ITimeService>();
				timeServiceSubstitute.DeltaTime.Returns(1);
				_movementBehaviour = A.PlayerMovement.WithSpeed(1).With(timeServiceSubstitute).Interface;
				_movementBehaviour.TransformProvider.Position = Vector2.zero;
				var movementDirection = new Vector2(Random.Range(0, 10000f), 0);

				_movementBehaviour.PerformMovement(movementDirection);

				Assert.AreEqual(new Vector3(1, 0), _movementBehaviour.TransformProvider.Position);
			}

			[Test]
			public void random_Vector2_with_only_y_coordinate_with_Speed_1_and_DeltaTime_1_still_moves_1_unit_in_the_correct_direction()
			{
				var timeServiceSubstitute = Substitute.For<ITimeService>();
				timeServiceSubstitute.DeltaTime.Returns(1);
				_movementBehaviour = A.PlayerMovement.WithSpeed(1).With(timeServiceSubstitute).Interface;
				_movementBehaviour.TransformProvider.Position = Vector2.zero;
				var movementDirection = new Vector2(0, Random.Range(0, 10000f));

				_movementBehaviour.PerformMovement(movementDirection);

				Assert.AreEqual(new Vector3(0, 1), _movementBehaviour.TransformProvider.Position);
			}

			[Test]
			public void Vector2_right_with_Speed_1_and_DeltaTime_05_moves_05_unit_to_the_right()
			{
				var timeServiceSubstitute = Substitute.For<ITimeService>();
				timeServiceSubstitute.DeltaTime.Returns(0.5f);
				_movementBehaviour = A.PlayerMovement.WithSpeed(1).With(timeServiceSubstitute).Interface;
				_movementBehaviour.TransformProvider.Position = Vector2.zero;

				_movementBehaviour.PerformMovement(Vector2.right);

				Assert.AreEqual(new Vector3(0.5f, 0), _movementBehaviour.TransformProvider.Position);
			}

			[Test]
			public void Vector2_up_with_Speed_1_and_DeltaTime_05_moves_05_unit_vertically()
			{
				var timeServiceSubstitute = Substitute.For<ITimeService>();
				timeServiceSubstitute.DeltaTime.Returns(0.5f);
				_movementBehaviour = A.PlayerMovement.WithSpeed(1).With(timeServiceSubstitute).Interface;
				_movementBehaviour.TransformProvider.Position = Vector2.zero;

				_movementBehaviour.PerformMovement(Vector2.up);

				Assert.AreEqual(new Vector3(0, 0.5f), _movementBehaviour.TransformProvider.Position);
			}

			[Test]
			public void Vector3_with_only_z_coordinate_does_nothing()
			{
				_movementBehaviour = A.PlayerMovement.Interface;
				var position = _movementBehaviour.TransformProvider.Position;

				_movementBehaviour.PerformMovement(new Vector3(0, 0, 20));

				Assert.AreEqual(position, _movementBehaviour.TransformProvider.Position);
			}

			[Test]
			public void non_zero_Vector2_followed_by_Vector2_zero_both_with_Speed_1_and_DeltaTime_1_moves_in_a_non_zero_Vector2_direction_according_to_speed()
			{
				var timeServiceSubstitute = Substitute.For<ITimeService>();
				timeServiceSubstitute.DeltaTime.Returns(1);
				_movementBehaviour = A.PlayerMovement.WithSpeed(1).With(timeServiceSubstitute).Interface;
				_movementBehaviour.TransformProvider.Position = Vector2.zero;
				var movementDirection = new Vector3(Random.Range(0, 10000f), Random.Range(0, 10000f));

				_movementBehaviour.PerformMovement(movementDirection);
				_movementBehaviour.PerformMovement(Vector2.zero);

				Assert.AreEqual(movementDirection.normalized, _movementBehaviour.TransformProvider.Position);
			}

			[Test]
			public void Vector_right_transform_when_rotated_moves_to_the_right()
			{
				var timeServiceSubstitute = Substitute.For<ITimeService>();
				timeServiceSubstitute.DeltaTime.Returns(1);
				_movementBehaviour = A.PlayerMovement.WithSpeed(1).With(timeServiceSubstitute).Interface;
				_movementBehaviour.TransformProvider.Rotation = Quaternion.identity;
				var supposedPosition = _movementBehaviour.TransformProvider.Position
					+ _movementBehaviour.Speed * new Vector3(1, 0);

				_movementBehaviour.TransformProvider.Rotate(Vector3.forward, 90);
				_movementBehaviour.PerformMovement(Vector2.right);

				Assert.AreEqual(supposedPosition, _movementBehaviour.TransformProvider.Position);
			}
		}

		public sealed class Dashing
		{
			private IDashingBehaviour _dashingBehaviour;

			[Test]
			public void default_DashDistance_is_greater_than_zero()
			{
				_dashingBehaviour = A.PlayerDashing.Interface;
				Assert.Greater(_dashingBehaviour.Distance, 0);
			}

			[Test]
			public void default_DashDistance_has_the_value_of_DEFAULT_DASH_SPEED_constant()
			{
				_dashingBehaviour = A.PlayerDashing.Interface;
				Assert.AreEqual(PlayerDashing.DEFAULT_DISTANCE, _dashingBehaviour.Distance);
			}

			[Test]
			public void using_constructor_with_invalid_data_default_dash_parameters_have_valid_values()
			{
				_dashingBehaviour = A.PlayerDashing.WithDistance(-1).WithSpeed(-1).Interface;

				Assert.AreEqual(PlayerDashing.DEFAULT_DISTANCE, _dashingBehaviour.Distance);
				Assert.AreEqual(PlayerDashing.DEFAULT_SPEED, _dashingBehaviour.Speed);
				Assert.NotNull(_dashingBehaviour.MovementCurve);
			}

			[Test]
			public void Vector2_zero_does_nothing()
			{
				_dashingBehaviour = A.PlayerDashing.Interface;
				_dashingBehaviour.TransformProvider.Position = new Vector2(5, 2);

				_dashingBehaviour.PerformDash(Vector2.zero);

				Assert.AreEqual(new Vector3(5, 2), _dashingBehaviour.TransformProvider.Position);
			}

			[Test]
			public void Vector2_right_with_Distance_3_Speed_5_default_DashSpeedCurve_and_DeltaTime_1_moves_3_to_the_right()
			{
				var timeServiceSubstitute = Substitute.For<ITimeService>();
				timeServiceSubstitute.DeltaTime.Returns(1f);
				_dashingBehaviour = A.PlayerDashing.WithDistance(3).WithSpeed(2).With(timeServiceSubstitute).Interface;
				_dashingBehaviour.TransformProvider.Position = Vector3.zero;
				var supposedPosition = new Vector3(3, 0);

				_dashingBehaviour.PerformDash(Vector2.right);

				Assert.AreEqual(supposedPosition, _dashingBehaviour.TransformProvider.Position);
			}

			[Test]
			public void Vector2_right_with_Distance_3_Speed_5_bell_curve_DashSpeedCurve_and_DeltaTime_0_25_moves_3_to_the_right()
			{
				var timeServiceSubstitute = Substitute.For<ITimeService>();
				timeServiceSubstitute.DeltaTime.Returns(0.25f);
				var animationCurve = new AnimationCurve(new Keyframe(0, 0),
					new Keyframe(0.25f, 0.75f), new Keyframe(1, 1));
				_dashingBehaviour = A.PlayerDashing.WithDistance(3).WithSpeed(5).WithCurve(animationCurve)
					.With(timeServiceSubstitute).Interface;
				_dashingBehaviour.TransformProvider.Position = Vector3.zero;
				var supposedPosition = new Vector3(3, 0);

				_dashingBehaviour.PerformDash(Vector2.right);

				Assert.AreEqual(supposedPosition, _dashingBehaviour.TransformProvider.Position);
			}

			[Test]
			public void Vector2_right_with_Distance_3_Speed_5_default_DashSpeedCurve_and_DeltaTime_0_01666_moves_3_to_the_right()
			{
				var timeServiceSubstitute = Substitute.For<ITimeService>();
				timeServiceSubstitute.DeltaTime.Returns(1 / 60f);
				_dashingBehaviour = A.PlayerDashing.WithDistance(3).WithSpeed(5).With(timeServiceSubstitute).Interface;
				_dashingBehaviour.TransformProvider.Position = Vector3.zero;
				var supposedPosition = new Vector3(3, 0);

				_dashingBehaviour.PerformDash(Vector2.right);

				const float tolerance = 0.001f;
				var actualPosition = _dashingBehaviour.TransformProvider.Position;
				var approximatelyEqual = Math.Abs(supposedPosition.x - actualPosition.x) < tolerance
					&& Math.Abs(supposedPosition.y - actualPosition.y) < tolerance
					&& Math.Abs(supposedPosition.z - actualPosition.z) < tolerance;
				Assert.IsTrue(approximatelyEqual, $"expected: {supposedPosition}, actual: {actualPosition}");
			}

			[Test]
			public void Vector2_right_with_Distance_3_Speed_15_bell_curve_DashSpeedCurve_and_DeltaTime_0_01666_moves_3_to_the_right()
			{
				var timeServiceSubstitute = Substitute.For<ITimeService>();
				timeServiceSubstitute.DeltaTime.Returns(1 / 60f);
				var animationCurve = new AnimationCurve(new Keyframe(0, 0),
					new Keyframe(0.25f, 0.75f), new Keyframe(1, 1));
				_dashingBehaviour = A.PlayerDashing.WithDistance(3).WithSpeed(15).WithCurve(animationCurve)
					.With(timeServiceSubstitute).Interface;
				_dashingBehaviour.TransformProvider.Position = Vector3.zero;
				var supposedPosition = new Vector3(3, 0);

				_dashingBehaviour.PerformDash(Vector2.right);

				const float tolerance = 0.001f;
				var actualPosition = _dashingBehaviour.TransformProvider.Position;
				var approximatelyEqual = Math.Abs(supposedPosition.x - actualPosition.x) < tolerance
					&& Math.Abs(supposedPosition.y - actualPosition.y) < tolerance
					&& Math.Abs(supposedPosition.z - actualPosition.z) < tolerance;
				Assert.IsTrue(approximatelyEqual, $"expected: {supposedPosition}, actual: {actualPosition}");
			}

			[Test]
			public void PerformDash_with_non_zero_cooldown_causes_all_subsequent_PerformDashes_to_do_nothing()
			{
				var timeServiceSubstitute = Substitute.For<ITimeService>();
				timeServiceSubstitute.DeltaTime.Returns(1 / 60f);
				var transformProviderSubstitute = new DefaultTransformProvider();
				_dashingBehaviour = A.PlayerDashing.WithDistance(3).WithSpeed(1).WithCooldown(1)
					.With(transformProviderSubstitute).With(timeServiceSubstitute).With(new AsyncCoroutineRunner())
					.Interface;
				_dashingBehaviour.TransformProvider.Position = Vector3.zero;
				var originalPosition = _dashingBehaviour.TransformProvider.Position;

				_dashingBehaviour.PerformDash(Vector2.right);
				var supposedPosition = _dashingBehaviour.TransformProvider.Position;
				_dashingBehaviour.PerformDash(Vector2.right);

				const float tolerance = 0.001f;
				var actualPosition = _dashingBehaviour.TransformProvider.Position;
				var actualPositionIsEqualToOriginal = Math.Abs(originalPosition.x - actualPosition.x) < tolerance
					&& Math.Abs(originalPosition.y - actualPosition.y) < tolerance
					&& Math.Abs(originalPosition.z - actualPosition.z) < tolerance;
				Assert.IsFalse(actualPositionIsEqualToOriginal,
					$"expected: {supposedPosition}, actual: {actualPosition}");
				var actualPositionIsEqualToSupposed = Math.Abs(supposedPosition.x - actualPosition.x) < tolerance
					&& Math.Abs(supposedPosition.y - actualPosition.y) < tolerance
					&& Math.Abs(supposedPosition.z - actualPosition.z) < tolerance;
				Assert.IsTrue(actualPositionIsEqualToSupposed,
					$"expected: {supposedPosition}, actual: {actualPosition}");
			}
		}

		public sealed class Looking
		{
			private static ILookingBehaviour _lookingBehaviour;

			[SetUp]
			public static void LookingSetup() => _lookingBehaviour = A.PlayerLooking.Interface;

			public sealed class PerformLookingAtScreenPositionMethod
			{
				[Test]
				public void Vector2_with_the_same_screen_position_does_nothing()
				{
					var worldToScreenProviderSubstitute = Substitute.For<IWorldToScreenProvider>();
					worldToScreenProviderSubstitute.Get(Arg.Any<Vector2>()).Returns(new Vector2(200, 500));
					_lookingBehaviour = A.PlayerLooking.With(worldToScreenProviderSubstitute).Interface;
					_lookingBehaviour.TransformProvider.Rotation = Quaternion.Euler(0, 0, 50);
					var originalRotationEuler = _lookingBehaviour.TransformProvider.Rotation.eulerAngles;

					_lookingBehaviour.PerformLookingAtPosition(new Vector2(200, 500));

					Assert.AreEqual(originalRotationEuler, _lookingBehaviour.TransformProvider.Rotation.eulerAngles);
				}

				[Test]
				public void Vector2_zero_when_positioned_in_the_center_of_the_screen_rotates_correctly()
				{
					var worldToScreenProviderSubstitute = Substitute.For<IWorldToScreenProvider>();
					worldToScreenProviderSubstitute.Get(Arg.Any<Vector2>())
						.Returns(new Vector2(Screen.width / 2f, Screen.height / 2f));
					_lookingBehaviour = A.PlayerLooking.With(worldToScreenProviderSubstitute).Interface;
					var supposedRotationEuler = Quaternion.FromToRotation(Vector3.up,
						-new Vector2(Screen.width, Screen.height).normalized).eulerAngles;

					_lookingBehaviour.PerformLookingAtPosition(Vector2.zero);

					Assert.AreEqual(supposedRotationEuler, _lookingBehaviour.TransformProvider.Rotation.eulerAngles);
				}

				[Test]
				public void Vector2_with_screen_height_and_width_as_coordinates_when_positioned_in_the_center_of_the_screen_rotates_correctly()
				{
					var worldToScreenProviderSubstitute = Substitute.For<IWorldToScreenProvider>();
					worldToScreenProviderSubstitute.Get(Arg.Any<Vector2>())
						.Returns(new Vector2(Screen.width / 2f, Screen.height / 2f));
					_lookingBehaviour = A.PlayerLooking.With(worldToScreenProviderSubstitute).Interface;
					var supposedRotationEuler = Quaternion.FromToRotation(Vector3.up,
						new Vector2(Screen.width, Screen.height).normalized).eulerAngles;

					_lookingBehaviour.PerformLookingAtPosition(new Vector2(Screen.width, Screen.height));

					Assert.AreEqual(supposedRotationEuler, _lookingBehaviour.TransformProvider.Rotation.eulerAngles);
				}

				[Test]
				public void Vector2_with_negative_values_when_positioned_in_bottom_left_screen_corner_rotates_correctly()
				{
					var worldToScreenProviderSubstitute = Substitute.For<IWorldToScreenProvider>();
					worldToScreenProviderSubstitute.Get(Arg.Any<Vector2>()).Returns(new Vector2(0, 0));
					_lookingBehaviour = A.PlayerLooking.With(worldToScreenProviderSubstitute).Interface;
					var supposedRotationEuler = Quaternion.FromToRotation(Vector3.up,
						new Vector2(-200, -200).normalized).eulerAngles;

					_lookingBehaviour.PerformLookingAtPosition(new Vector2(-200, -200));

					Assert.AreEqual(supposedRotationEuler, _lookingBehaviour.TransformProvider.Rotation.eulerAngles);
				}

				[Test]
				public void sequential_Vector2s_when_positioned_in_the_center_of_the_screen_rotates_correctly()
				{
					var worldToScreenProviderSubstitute = Substitute.For<IWorldToScreenProvider>();
					var screenPosition = new Vector2(Screen.width / 2f, Screen.height / 2f);
					worldToScreenProviderSubstitute.Get(Arg.Any<Vector2>()).Returns(screenPosition);
					_lookingBehaviour = A.PlayerLooking.With(worldToScreenProviderSubstitute).Interface;

					const int step = 2;
					for (var y = 0; y < Screen.height; y += step)
					{
						for (var x = 0; x < Screen.width; x += step)
						{
							var mousePosition = new Vector2(x, y);
							var supposedRotationEuler = Quaternion.FromToRotation(Vector3.up,
								mousePosition - screenPosition).eulerAngles;
							_lookingBehaviour.PerformLookingAtPosition(mousePosition);
							Assert.AreEqual(supposedRotationEuler,
								_lookingBehaviour.TransformProvider.Rotation.eulerAngles,
								$"For position {mousePosition}");
						}
					}
				}
			}

			public sealed class PerformLookingInDirectionMethod
			{
				[SetUp]
				public void PerformLookingInDirectionSetup() => LookingSetup();

				[Test]
				public void Vector2_zero_does_nothing()
				{
					_lookingBehaviour.TransformProvider.Rotation = Quaternion.Euler(0, 0, 45);
					var originalRotationEuler = _lookingBehaviour.TransformProvider.Rotation.eulerAngles;

					_lookingBehaviour.PerformLookingInDirection(Vector2.zero);

					var finalRotationEuler = _lookingBehaviour.TransformProvider.Rotation.eulerAngles;
					Assert.AreEqual(originalRotationEuler, finalRotationEuler);
				}

				[Test]
				public void Vector2_right_with_initial_rotation_0_rotates_to_negative_90()
				{
					_lookingBehaviour.TransformProvider.Rotation = Quaternion.Euler(0, 0, 0);
					var supposedRotationEuler = Quaternion.Euler(0, 0, -90).eulerAngles;

					_lookingBehaviour.PerformLookingInDirection(Vector2.right);

					Assert.AreEqual(supposedRotationEuler, _lookingBehaviour.TransformProvider.Rotation.eulerAngles);
				}

				[Test]
				public void Vector2_up_with_initial_rotation_negative_135_rotates_to_negative_0()
				{
					_lookingBehaviour.TransformProvider.Rotation = Quaternion.Euler(0, 0, -135);
					var supposedRotationEuler = Quaternion.Euler(0, 0, 0).eulerAngles;

					_lookingBehaviour.PerformLookingInDirection(Vector2.up);

					Assert.AreEqual(supposedRotationEuler, _lookingBehaviour.TransformProvider.Rotation.eulerAngles);
				}

				[Test]
				public void not_normalized_Vector2_to_the_right_with_initial_rotation_0_rotates_to_negative_90()
				{
					_lookingBehaviour.TransformProvider.Rotation = Quaternion.Euler(0, 0, 0);
					var supposedRotationEuler = Quaternion.Euler(0, 0, -90).eulerAngles;

					_lookingBehaviour.PerformLookingInDirection(new Vector2(20, 0));

					Assert.AreEqual(supposedRotationEuler, _lookingBehaviour.TransformProvider.Rotation.eulerAngles);
				}

				[Test]
				public void not_normalized_Vector2_to_the_up_with_initial_rotation_90_rotates_to_0()
				{
					_lookingBehaviour.TransformProvider.Rotation = Quaternion.Euler(0, 0, 90);
					var supposedRotationEuler = Quaternion.Euler(0, 0, 0).eulerAngles;

					_lookingBehaviour.PerformLookingInDirection(new Vector2(0, 20));

					Assert.AreEqual(supposedRotationEuler, _lookingBehaviour.TransformProvider.Rotation.eulerAngles);
				}

				[Test]
				public void Vector3_with_only_z_does_nothing()
				{
					_lookingBehaviour.TransformProvider.Rotation = Quaternion.Euler(0, 0, 22);
					var originalRotationEuler = _lookingBehaviour.TransformProvider.Rotation.eulerAngles;

					_lookingBehaviour.PerformLookingInDirection(new Vector3(0, 0, 20));

					Assert.AreEqual(originalRotationEuler, _lookingBehaviour.TransformProvider.Rotation.eulerAngles);
				}
			}
		}
	}
}