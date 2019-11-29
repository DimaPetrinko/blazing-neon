using Game.TDD.GameSystemServices;
using Game.TDD.Players.Input;
using Game.TDD.Players.Looking;
using Game.TDD.Players.Movement;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using DeviceType = Game.TDD.Players.Input.DeviceType;
using Player = Game.TDD.Players.Player;
using PlayerInput = Game.TDD.Players.Input.PlayerInput;
using PlayerMovement = Game.TDD.Players.Movement.PlayerMovement;
using PlayerLooking = Game.TDD.Players.Looking.PlayerLooking;
using Random = UnityEngine.Random;

namespace Tests.TDDPlayer
{
	public static class TDDPlayer
	{
		public sealed class Initialization
		{
			private static Player player;

			public void InitializationSetup() => player = new Player();

			[Test]
			public void default_input_behaviour_is_not_null()
			{
				InitializationSetup();
				Assert.NotNull(player.InputBehaviour);
			}

			[Test]
			public void default_movement_behaviour_is_not_null()
			{
				InitializationSetup();
				Assert.NotNull(player.MovementBehaviour);
			}

			[Test]
			public void default_looking_behaviour_is_not_null()
			{
				InitializationSetup();
				Assert.NotNull(player.LookingBehaviour);
			}

			[Test]
			public void using_default_constructor_TransformProvider_is_the_same_for_player_and_his_components()
			{
				player = new Player();

				Assert.AreEqual(player.TransformProvider, player.MovementBehaviour.TransformProvider);
				Assert.AreEqual(player.TransformProvider, player.LookingBehaviour.TransformProvider);
			}

			[Test]
			public void
				using_TransformProvider_constructor_overload_TransformProvider_is_the_same_for_player_and_his_components()
			{
				player = new Player(new UnityTransformProvider(new GameObject().transform));

				Assert.AreEqual(player.TransformProvider, player.MovementBehaviour.TransformProvider);
				Assert.AreEqual(player.TransformProvider, player.LookingBehaviour.TransformProvider);
			}

			[Test]
			public void
				using_TransformProvider_constructor_overload_with_parameters_TransformProvider_is_the_same_for_player_and_his_components()
			{
				player = new Player(new UnityTransformProvider(new GameObject().transform), new PlayerInput(),
					new PlayerMovement(), new PlayerLooking());

				Assert.AreEqual(player.TransformProvider, player.MovementBehaviour.TransformProvider);
				Assert.AreEqual(player.TransformProvider, player.LookingBehaviour.TransformProvider);
			}

			[Test]
			public void
				using_Transform_constructor_overload_TransformProvider_is_the_same_for_player_and_his_components()
			{
				player = new Player(new GameObject().transform);

				Assert.AreEqual(player.TransformProvider, player.MovementBehaviour.TransformProvider);
				Assert.AreEqual(player.TransformProvider, player.LookingBehaviour.TransformProvider);
			}

			[Test]
			public void
				using_Transform_constructor_overload_with_parameters_TransformProvider_is_the_same_for_player_and_his_components()
			{
				player = new Player(new GameObject().transform, new PlayerInput(), new PlayerMovement(),
					new PlayerLooking());

				Assert.AreEqual(player.TransformProvider, player.MovementBehaviour.TransformProvider);
				Assert.AreEqual(player.TransformProvider, player.LookingBehaviour.TransformProvider);
			}

			[Test]
			public void MovementBehaviour_is_subscribed_to_Dash_event()
			{
				player = new Player(null as ITransformProvider, Substitute.For<IInputBehaviour>(),
					Substitute.For<IMovementBehaviour>());

				player.InputBehaviour.Dash += Raise.EventWith(new Vector2EventArgs(Vector2.zero));

				player.MovementBehaviour.Received().PerformDash(Arg.Any<Vector2>());
			}

			[Test]
			public void FixedUpdate_moves_player_according_to_input()
			{
				var inputBehaviourSubstitute = Substitute.For<IInputBehaviour>();
				inputBehaviourSubstitute.MovementDirection.Returns(Vector2.right);
				var timeServiceSubstitute = Substitute.For<ITimeService>();
				timeServiceSubstitute.DeltaTime.Returns(1);
				player = new Player(null as ITransformProvider, inputBehaviourSubstitute,
					new PlayerMovement(1, 0, 0, null, timeServiceSubstitute));
				player.TransformProvider.Position = Vector2.zero;

				player.FixedUpdate();

				Assert.AreEqual(Vector3.right, player.TransformProvider.Position);
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
				player = new Player(null as ITransformProvider, inputBehaviourSubstitute, null,
					new PlayerLooking(worldToScreenProvider));
				player.TransformProvider.Rotation = Quaternion.Euler(0, 0, 0);
				var supposedRotationEuler = Quaternion.FromToRotation(Vector3.up,
					new Vector2(1, 0)).eulerAngles;

				player.FixedUpdate();

				Assert.AreEqual(supposedRotationEuler, player.TransformProvider.Rotation.eulerAngles);
			}

			[Test]
			public void FixedUpdate_rotates_player_according_to_gamepad_input()
			{
				var inputBehaviourSubstitute = Substitute.For<IInputBehaviour>();
				inputBehaviourSubstitute.LookDeviceType.Returns(DeviceType.Gamepad);
				inputBehaviourSubstitute.LookDirection.Returns(Vector2.right);
				player = new Player(null as ITransformProvider, inputBehaviourSubstitute, null, new PlayerLooking());
				player.TransformProvider.Rotation = Quaternion.Euler(0, 0, 0);
				var supposedRotationEuler = Quaternion.FromToRotation(Vector3.up,
					new Vector2(1, 0)).eulerAngles;

				player.FixedUpdate();

				Assert.AreEqual(supposedRotationEuler, player.TransformProvider.Rotation.eulerAngles);
			}

			[Test]
			public void FixedUpdate_when_LookingDeviceType_is_not_mouse_nor_gamepad_does_not_rotate_player()
			{
				var inputBehaviourSubstitute = Substitute.For<IInputBehaviour>();
				inputBehaviourSubstitute.LookDeviceType.Returns(DeviceType.Keyboard);
				inputBehaviourSubstitute.LookDirection.Returns(Vector2.right);
				player = new Player(null as ITransformProvider, inputBehaviourSubstitute, null, new PlayerLooking());
				player.TransformProvider.Rotation = Quaternion.Euler(0, 0, 0);
				var originalRotationEuler = player.TransformProvider.Rotation.eulerAngles;

				player.FixedUpdate();

				Assert.AreEqual(originalRotationEuler, player.TransformProvider.Rotation.eulerAngles);
			}
		}
		
		public sealed class TimeServiceProperty
		{
			private ITimeService timeService;

			[SetUp]
			public void TimeServiceSetup() => timeService = new UnityTimeService(); 

			[Test]
			public void default_TimeService_is_not_null() => Assert.NotNull(timeService);

			[Test]
			public void default_DeltaTime_is_greater_than_0() =>
				Assert.Greater(timeService.DeltaTime, 0);

			[Test]
			public void default_Time_is_greater_than_0() => Assert.Greater(timeService.Time, 0);
		}

		public sealed class Input
		{
			private static IInputBehaviour inputBehaviour;

			[SetUp]
			public static void InputSetup() => inputBehaviour = new PlayerInput();

			public sealed class LookDirectionProperty
			{
				[SetUp]
				public void LookDirectionSetup() => InputSetup();

				[Test]
				public void default_LookDirection_is_Vector2_zero() =>
					Assert.AreEqual(Vector2.zero, inputBehaviour.LookDirection);

				[Test]
				public void default_LookingDeviceType_is_None() =>
					Assert.AreEqual(DeviceType.None, inputBehaviour.LookDeviceType);

				[Test]
				public void SetLookDirection_for_some_DeviceType_updates_LookDirection_and_LookingDeviceType()
				{
					var originalLookDirection = inputBehaviour.LookDirection;

					inputBehaviour.SetLookDirection(new Vector2(10, 2), DeviceType.Mouse);

					Assert.AreNotEqual(originalLookDirection, inputBehaviour.LookDirection);
					Assert.AreEqual(DeviceType.Mouse, inputBehaviour.LookDeviceType);
				}

				[Test]
				public void SetLookDirection_for_Keyboard_does_nothing()
				{
					var originalLookDirection = inputBehaviour.LookDirection;
					var originalDeviceType = inputBehaviour.LookDeviceType;

					inputBehaviour.SetLookDirection(new Vector2(10, 2), DeviceType.Keyboard);

					Assert.AreEqual(originalLookDirection, inputBehaviour.LookDirection);
					Assert.AreEqual(originalDeviceType, inputBehaviour.LookDeviceType);
				}

				[Test]
				public void SetLookDirection_normalizes_input()
				{
					var newLookDirection = new Vector2(10, 2);

					inputBehaviour.SetLookDirection(newLookDirection, DeviceType.Mouse);

					Assert.AreEqual(newLookDirection.normalized, inputBehaviour.LookDirection);
				}
			}
			
			public sealed class MovementDirectionProperty
			{
				[SetUp]
				public void  MovementDirectionSetup() => InputSetup();

				[Test]
				public void default_MovementDirection_is_Vector2_zero() =>
					Assert.AreEqual(Vector2.zero, inputBehaviour.MovementDirection);

				[Test]
				public void SetMovementDirection_updates_MovementDirection()
				{
					var originalMovementDirection = inputBehaviour.MovementDirection;

					inputBehaviour.SetMovementDirection(new Vector2(10, 2));

					Assert.AreNotEqual(originalMovementDirection, inputBehaviour.MovementDirection);
				}

				[Test]
				public void SetMovementDirection_normalizes_input()
				{
					var newMovementDirection = new Vector2(10, 2);

					inputBehaviour.SetMovementDirection(newMovementDirection);

					Assert.AreEqual(newMovementDirection.normalized, inputBehaviour.MovementDirection);
				}
			}
		}

		public sealed class Movement
		{
			private static IMovementBehaviour movementBehaviour;

			[SetUp]
			public static void MovementSetup() => movementBehaviour = new PlayerMovement();

			public sealed class PerformMovementMethod
			{
				[Test]
				public void default_Speed_is_greater_than_0()
				{
					MovementSetup();

					Assert.Greater(movementBehaviour.Speed, 0);
				}

				[Test]
				public void default_Speed_has_the_value_of_DEFAULT_SPEED_constant()
				{
					MovementSetup();

					Assert.AreEqual(PlayerMovement.DEFAULT_SPEED, movementBehaviour.Speed);
				}

				[Test]
				public void Vector2_zero_does_nothing()
				{
					MovementSetup();
					movementBehaviour.TransformProvider.Position = new Vector2(2, 5);

					movementBehaviour.PerformMovement(Vector2.zero);

					Assert.AreEqual(new Vector3(2, 5), movementBehaviour.TransformProvider.Position);
				}

				[Test]
				public void Vector2_right_with_Speed_1_and_DeltaTime_1_moves_1_unit_to_the_right()
				{
					var timeServiceSubstitute = Substitute.For<ITimeService>();
					timeServiceSubstitute.DeltaTime.Returns(1);
					movementBehaviour = new PlayerMovement(1, 0, 0, null, timeServiceSubstitute);
					movementBehaviour.TransformProvider.Position = Vector2.zero;

					movementBehaviour.PerformMovement(Vector2.right);

					Assert.AreEqual(new Vector3(1, 0), movementBehaviour.TransformProvider.Position);
				}

				[Test]
				public void Vector2_up_with_Speed_1_and_DeltaTime_1_moves_1_unit_vertically()
				{
					var timeServiceSubstitute = Substitute.For<ITimeService>();
					timeServiceSubstitute.DeltaTime.Returns(1);
					movementBehaviour = new PlayerMovement(1, 0, 0, null, timeServiceSubstitute);
					movementBehaviour.TransformProvider.Position = Vector2.zero;

					movementBehaviour.PerformMovement(Vector2.up);

					Assert.AreEqual(new Vector3(0, 1), movementBehaviour.TransformProvider.Position);
				}

				[Test]
				public void Vector2_right_with_Speed_5_and_DeltaTime_1_moves_1_unit_to_the_right()
				{
					var timeServiceSubstitute = Substitute.For<ITimeService>();
					timeServiceSubstitute.DeltaTime.Returns(1);
					movementBehaviour = new PlayerMovement(5, 0, 0, null, timeServiceSubstitute);
					movementBehaviour.TransformProvider.Position = Vector2.zero;

					movementBehaviour.PerformMovement(Vector2.right);

					Assert.AreEqual(new Vector3(5, 0), movementBehaviour.TransformProvider.Position);
				}

				[Test]
				public void Vector2_up_with_Speed_5_and_DeltaTime_1_moves_1_unit_vertically()
				{
					var timeServiceSubstitute = Substitute.For<ITimeService>();
					timeServiceSubstitute.DeltaTime.Returns(1);
					movementBehaviour = new PlayerMovement(5, 0, 0, null, timeServiceSubstitute);
					movementBehaviour.TransformProvider.Position = Vector2.zero;

					movementBehaviour.PerformMovement(Vector2.up);

					Assert.AreEqual(new Vector3(0, 5), movementBehaviour.TransformProvider.Position);
				}

				[Test]
				public void
					random_Vector2_with_only_x_coordinate_with_Speed_1_and_DeltaTime_1_still_moves_1_unit_in_the_correct_direction()
				{
					var timeServiceSubstitute = Substitute.For<ITimeService>();
					timeServiceSubstitute.DeltaTime.Returns(1);
					movementBehaviour = new PlayerMovement(1, 0, 0, null, timeServiceSubstitute);
					movementBehaviour.TransformProvider.Position = Vector2.zero;
					var movementDirection = new Vector2(Random.Range(0, 10000f), 0);

					movementBehaviour.PerformMovement(movementDirection);

					Assert.AreEqual(new Vector3(1, 0), movementBehaviour.TransformProvider.Position);
				}

				[Test]
				public void
					random_Vector2_with_only_y_coordinate_with_Speed_1_and_DeltaTime_1_still_moves_1_unit_in_the_correct_direction()
				{
					var timeServiceSubstitute = Substitute.For<ITimeService>();
					timeServiceSubstitute.DeltaTime.Returns(1);
					movementBehaviour = new PlayerMovement(1, 0, 0, null, timeServiceSubstitute);
					movementBehaviour.TransformProvider.Position = Vector2.zero;
					var movementDirection = new Vector2(0, Random.Range(0, 10000f));

					movementBehaviour.PerformMovement(movementDirection);

					Assert.AreEqual(new Vector3(0, 1), movementBehaviour.TransformProvider.Position);
				}

				[Test]
				public void Vector2_right_with_Speed_1_and_DeltaTime_05_moves_05_unit_to_the_right()
				{
					var timeServiceSubstitute = Substitute.For<ITimeService>();
					timeServiceSubstitute.DeltaTime.Returns(0.5f);
					movementBehaviour = new PlayerMovement(1, 0, 0, null, timeServiceSubstitute);
					movementBehaviour.TransformProvider.Position = Vector2.zero;

					movementBehaviour.PerformMovement(Vector2.right);

					Assert.AreEqual(new Vector3(0.5f, 0), movementBehaviour.TransformProvider.Position);
				}

				[Test]
				public void Vector2_up_with_Speed_1_and_DeltaTime_05_moves_05_unit_vertically()
				{
					var timeServiceSubstitute = Substitute.For<ITimeService>();
					timeServiceSubstitute.DeltaTime.Returns(0.5f);
					movementBehaviour = new PlayerMovement(1, 0, 0, null, timeServiceSubstitute);
					movementBehaviour.TransformProvider.Position = Vector2.zero;

					movementBehaviour.PerformMovement(Vector2.up);

					Assert.AreEqual(new Vector3(0, 0.5f), movementBehaviour.TransformProvider.Position);
				}

				[Test]
				public void Vector3_with_only_z_coordinate_does_nothing()
				{
					var position = movementBehaviour.TransformProvider.Position;

					movementBehaviour.PerformMovement(new Vector3(0, 0, 20));

					Assert.AreEqual(position, movementBehaviour.TransformProvider.Position);
				}

				[Test]
				public void
					non_zero_Vector2_followed_by_Vector2_zero_both_with_Speed_1_and_DeltaTime_1_moves_in_a_non_zero_Vector2_direction_according_to_speed()
				{
					var timeServiceSubstitute = Substitute.For<ITimeService>();
					timeServiceSubstitute.DeltaTime.Returns(1);
					movementBehaviour = new PlayerMovement(1, 0, 0, null, timeServiceSubstitute);
					movementBehaviour.TransformProvider.Position = Vector2.zero;
					var movementDirection = new Vector3(Random.Range(0, 10000f), Random.Range(0, 10000f));

					movementBehaviour.PerformMovement(movementDirection);
					movementBehaviour.PerformMovement(Vector2.zero);

					Assert.AreEqual(movementDirection.normalized, movementBehaviour.TransformProvider.Position);
				}

				[Test]
				public void when_rotated_Vector_right_moves_to_the_right()
				{
					var timeServiceSubstitute = Substitute.For<ITimeService>();
					timeServiceSubstitute.DeltaTime.Returns(1);
					movementBehaviour = new PlayerMovement(timeServiceSubstitute);
					movementBehaviour.TransformProvider.Rotation = Quaternion.identity;
					var supposedPosition = movementBehaviour.TransformProvider.Position +
						movementBehaviour.Speed * new Vector3(1, 0);

					movementBehaviour.TransformProvider.Rotate(Vector3.forward, 90);
					movementBehaviour.PerformMovement(Vector2.right);

					Assert.AreEqual(supposedPosition, movementBehaviour.TransformProvider.Position);
				}
			}

			public sealed class DashMethod
			{
				[SetUp]
				public void DashSetup() => MovementSetup();

				[Test]
				public void default_DashDistance_is_greater_than_zero() =>
					Assert.Greater(movementBehaviour.DashDistance, 0);

				[Test]
				public void default_DashDistance_has_the_value_of_DEFAULT_DASH_SPEED_constant() =>
					Assert.AreEqual(PlayerMovement.DEFAULT_DASH_DISTANCE, movementBehaviour.DashDistance);

				[Test]
				public void using_constructor_with_invalid_data_default_dash_parameters_have_valid_values()
				{
					movementBehaviour = new PlayerMovement(-1, -1, -1);
					
					Assert.AreEqual(PlayerMovement.DEFAULT_DASH_DISTANCE, movementBehaviour.DashDistance);
					Assert.AreEqual(PlayerMovement.DEFAULT_DASH_DURATION, movementBehaviour.DashDuration);
					Assert.NotNull(movementBehaviour.DashSpeedCurve);
				}

				[Test]
				public void Vector2_zero_does_nothing()
				{
					movementBehaviour.TransformProvider.Position = new Vector2(5, 2);

					movementBehaviour.PerformDash(Vector2.zero);

					Assert.AreEqual(new Vector3(5, 2), movementBehaviour.TransformProvider.Position);
				}

				[Test]
				public void Vector2_right_with_DashDistance_3_DashDuration_1_default_DashSpeedCurve_and_DeltaTime_1_moves_3_to_the_right()
				{
					var timeServiceSubstitute = Substitute.For<ITimeService>();
					timeServiceSubstitute.DeltaTime.Returns(1f);
					movementBehaviour = new PlayerMovement(0, 3, 1, null, timeServiceSubstitute);
					movementBehaviour.TransformProvider.Position = Vector3.zero;
					var supposedPosition = new Vector3(3, 0);
					
					// perform dash. couple of times?
					movementBehaviour.PerformDash(new Vector2(3, 0));
					movementBehaviour.PerformMovement(Vector2.one);
					
					//check for things
					Assert.AreEqual(supposedPosition, movementBehaviour.TransformProvider.Position);
				}

				[Test]
				public void Vector2_right_with_DashDistance_3_DashDuration_1_bell_curve_DashSpeedCurve_and_DeltaTime_0_25_moves_3_to_the_right()
				{
					var timeServiceSubstitute = Substitute.For<ITimeService>();
					timeServiceSubstitute.DeltaTime.Returns(0.25f);
					var animationCurve = new AnimationCurve(new Keyframe(0, 0),
						new Keyframe(0.25f, 0.75f), new Keyframe(1, 1));
					movementBehaviour = new PlayerMovement(0, 3, 1, animationCurve, timeServiceSubstitute);
					movementBehaviour.TransformProvider.Position = Vector3.zero;
					var supposedPosition = new Vector3(3, 0);
					
					// perform dash. couple of times?
					movementBehaviour.PerformDash(new Vector2(3, 0));
					movementBehaviour.PerformMovement(Vector2.one);
					
					//check for things
					Assert.AreEqual(supposedPosition, movementBehaviour.TransformProvider.Position);
				}
			}
		}

		public sealed class Looking
		{
			private static ILookingBehaviour lookingBehaviour;

			[SetUp]
			public static void LookingSetup() => lookingBehaviour = new PlayerLooking();

			public sealed class PerformLookingAtScreenPositionMethod
			{
				[Test]
				public void Vector2_with_the_same_screen_position_does_nothing()
				{
					var worldToScreenProvider = Substitute.For<IWorldToScreenProvider>();
					worldToScreenProvider.Get(Arg.Any<Vector2>())
						.Returns(new Vector2(200, 500));
					lookingBehaviour = new PlayerLooking(worldToScreenProvider);
					lookingBehaviour.TransformProvider.Rotation = Quaternion.Euler(0, 0, 50);
					var originalRotationEuler = lookingBehaviour.TransformProvider.Rotation.eulerAngles;

					lookingBehaviour.PerformLookingAtPosition(new Vector2(200, 500));

					Assert.AreEqual(originalRotationEuler, lookingBehaviour.TransformProvider.Rotation.eulerAngles);
				}

				[Test]
				public void Vector2_zero_when_positioned_in_the_center_of_the_screen_rotates_correctly()
				{
					var worldToScreenProvider = Substitute.For<IWorldToScreenProvider>();
					worldToScreenProvider.Get(Arg.Any<Vector2>())
						.Returns(new Vector2(Screen.width / 2f, Screen.height / 2f));
					lookingBehaviour = new PlayerLooking(worldToScreenProvider);
					var supposedRotationEuler = Quaternion.FromToRotation(Vector3.up,
						-new Vector2(Screen.width, Screen.height).normalized).eulerAngles;

					lookingBehaviour.PerformLookingAtPosition(Vector2.zero);

					Assert.AreEqual(supposedRotationEuler, lookingBehaviour.TransformProvider.Rotation.eulerAngles);
				}

				[Test]
				public void
					Vector2_with_screen_height_and_width_as_coordinates_when_positioned_in_the_center_of_the_screen_rotates_correctly()
				{
					var worldToScreenProvider = Substitute.For<IWorldToScreenProvider>();
					worldToScreenProvider.Get(Arg.Any<Vector2>())
						.Returns(new Vector2(Screen.width / 2f, Screen.height / 2f));
					lookingBehaviour = new PlayerLooking(worldToScreenProvider);
					var supposedRotationEuler = Quaternion.FromToRotation(Vector3.up,
						new Vector2(Screen.width, Screen.height).normalized).eulerAngles;

					lookingBehaviour.PerformLookingAtPosition(new Vector2(Screen.width, Screen.height));

					Assert.AreEqual(supposedRotationEuler, lookingBehaviour.TransformProvider.Rotation.eulerAngles);
				}

				[Test]
				public void
					Vector2_with_negative_values_when_positioned_in_bottom_left_screen_corner_rotates_correctly()
				{
					var worldToScreenProvider = Substitute.For<IWorldToScreenProvider>();
					worldToScreenProvider.Get(Arg.Any<Vector2>())
						.Returns(new Vector2(0, 0));
					lookingBehaviour = new PlayerLooking(worldToScreenProvider);
					var supposedRotationEuler = Quaternion.FromToRotation(Vector3.up,
						new Vector2(-200, -200).normalized).eulerAngles;

					lookingBehaviour.PerformLookingAtPosition(new Vector2(-200, -200));

					Assert.AreEqual(supposedRotationEuler, lookingBehaviour.TransformProvider.Rotation.eulerAngles);
				}

				[Test]
				public void sequential_Vector2s_when_positioned_in_the_center_of_the_screen_rotates_correctly()
				{
					var worldToScreenProvider = Substitute.For<IWorldToScreenProvider>();
					var screenPosition = new Vector2(Screen.width / 2f, Screen.height / 2f);
					worldToScreenProvider.Get(Arg.Any<Vector2>())
						.Returns(screenPosition);
					lookingBehaviour = new PlayerLooking(worldToScreenProvider);

					for (var y = 0; y < 1000; y++)
					{
						for (var x = 0; x < 1000; x++)
						{
							var mousePosition = new Vector2(x, y);
							var supposedRotationEuler = Quaternion.FromToRotation(Vector3.up,
								mousePosition - screenPosition).eulerAngles;
							lookingBehaviour.PerformLookingAtPosition(mousePosition);
							Assert.AreEqual(supposedRotationEuler,
								lookingBehaviour.TransformProvider.Rotation.eulerAngles);
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
					lookingBehaviour.TransformProvider.Rotation = Quaternion.Euler(0, 0, 45);
					var originalRotationEuler = lookingBehaviour.TransformProvider.Rotation.eulerAngles;

					lookingBehaviour.PerformLookingInDirection(Vector2.zero);

					var finalRotationEuler = lookingBehaviour.TransformProvider.Rotation.eulerAngles;
					Assert.AreEqual(originalRotationEuler, finalRotationEuler);
				}

				[Test]
				public void Vector2_right_with_initial_rotation_0_rotates_to_negative_90()
				{
					lookingBehaviour.TransformProvider.Rotation = Quaternion.Euler(0, 0, 0);
					var supposedRotationEuler = Quaternion.Euler(0, 0, -90).eulerAngles;

					lookingBehaviour.PerformLookingInDirection(Vector2.right);

					Assert.AreEqual(supposedRotationEuler, lookingBehaviour.TransformProvider.Rotation.eulerAngles);
				}

				[Test]
				public void Vector2_up_with_initial_rotation_negative_135_rotates_to_negative_0()
				{
					lookingBehaviour.TransformProvider.Rotation = Quaternion.Euler(0, 0, -135);
					var supposedRotationEuler = Quaternion.Euler(0, 0, 0).eulerAngles;

					lookingBehaviour.PerformLookingInDirection(Vector2.up);

					Assert.AreEqual(supposedRotationEuler, lookingBehaviour.TransformProvider.Rotation.eulerAngles);
				}

				[Test]
				public void not_normalized_Vector2_to_the_right_with_initial_rotation_0_rotates_to_negative_90()
				{
					lookingBehaviour.TransformProvider.Rotation = Quaternion.Euler(0, 0, 0);
					var supposedRotationEuler = Quaternion.Euler(0, 0, -90).eulerAngles;

					lookingBehaviour.PerformLookingInDirection(new Vector2(20, 0));

					Assert.AreEqual(supposedRotationEuler, lookingBehaviour.TransformProvider.Rotation.eulerAngles);
				}

				[Test]
				public void not_normalized_Vector2_to_the_up_with_initial_rotation_90_rotates_to_0()
				{
					lookingBehaviour.TransformProvider.Rotation = Quaternion.Euler(0, 0, 90);
					var supposedRotationEuler = Quaternion.Euler(0, 0, 0).eulerAngles;

					lookingBehaviour.PerformLookingInDirection(new Vector2(0, 20));

					Assert.AreEqual(supposedRotationEuler, lookingBehaviour.TransformProvider.Rotation.eulerAngles);
				}

				[Test]
				public void Vector3_with_only_z_does_nothing()
				{
					lookingBehaviour.TransformProvider.Rotation = Quaternion.Euler(0, 0, 22);
					var originalRotationEuler = lookingBehaviour.TransformProvider.Rotation.eulerAngles;

					lookingBehaviour.PerformLookingInDirection(new Vector3(0, 0, 20));

					Assert.AreEqual(originalRotationEuler, lookingBehaviour.TransformProvider.Rotation.eulerAngles);
				}
			}
		}
	}
}