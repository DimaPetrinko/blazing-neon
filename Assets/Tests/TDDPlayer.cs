using Game.Players.TDD;
using Game.Players.TDD.Movement;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
	public static class TDDPlayer
	{
		public static class Input {}

		public static class Movement
		{
			private static PlayerMovement movementBehaviour;

			[SetUp]
			public static void MovementSetup() => movementBehaviour = new PlayerMovement();

			public sealed class TimeServiceProperty
			{
				[SetUp]
				public void TimeServiceSetup() => MovementSetup();

				[Test]
				public void default_TimeService_is_not_null() => Assert.NotNull(movementBehaviour.TimeService);

				[Test]
				public void default_DeltaTime_is_greater_than_0() =>
					Assert.Greater(movementBehaviour.TimeService.DeltaTime, 0);

				[Test]
				public void default_Time_is_greater_than_0()
				{
					Assert.Greater(movementBehaviour.TimeService.Time, 0);
				}
			}

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
					movementBehaviour = new PlayerMovement(1, 0, timeServiceSubstitute);
					movementBehaviour.TransformProvider.Position = Vector2.zero;

					movementBehaviour.PerformMovement(Vector2.right);

					Assert.AreEqual(new Vector3(1, 0), movementBehaviour.TransformProvider.Position);
				}

				[Test]
				public void Vector2_up_with_Speed_1_and_DeltaTime_1_moves_1_unit_vertically()
				{
					var timeServiceSubstitute = Substitute.For<ITimeService>();
					timeServiceSubstitute.DeltaTime.Returns(1);
					movementBehaviour = new PlayerMovement(1, 0, timeServiceSubstitute);
					movementBehaviour.TransformProvider.Position = Vector2.zero;

					movementBehaviour.PerformMovement(Vector2.up);

					Assert.AreEqual(new Vector3(0, 1), movementBehaviour.TransformProvider.Position);
				}

				[Test]
				public void Vector2_right_with_Speed_5_and_DeltaTime_1_moves_1_unit_to_the_right()
				{
					var timeServiceSubstitute = Substitute.For<ITimeService>();
					timeServiceSubstitute.DeltaTime.Returns(1);
					movementBehaviour = new PlayerMovement(5, 0, timeServiceSubstitute);
					movementBehaviour.TransformProvider.Position = Vector2.zero;

					movementBehaviour.PerformMovement(Vector2.right);

					Assert.AreEqual(new Vector3(5, 0), movementBehaviour.TransformProvider.Position);
				}

				[Test]
				public void Vector2_up_with_Speed_5_and_DeltaTime_1_moves_1_unit_vertically()
				{
					var timeServiceSubstitute = Substitute.For<ITimeService>();
					timeServiceSubstitute.DeltaTime.Returns(1);
					movementBehaviour = new PlayerMovement(5, 0, timeServiceSubstitute);
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
					movementBehaviour = new PlayerMovement(1, 0, timeServiceSubstitute);
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
					movementBehaviour = new PlayerMovement(1, 0, timeServiceSubstitute);
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
					movementBehaviour = new PlayerMovement(1, 0, timeServiceSubstitute);
					movementBehaviour.TransformProvider.Position = Vector2.zero;

					movementBehaviour.PerformMovement(Vector2.right);

					Assert.AreEqual(new Vector3(0.5f, 0), movementBehaviour.TransformProvider.Position);
				}

				[Test]
				public void Vector2_up_with_Speed_1_and_DeltaTime_05_moves_05_unit_vertically()
				{
					var timeServiceSubstitute = Substitute.For<ITimeService>();
					timeServiceSubstitute.DeltaTime.Returns(0.5f);
					movementBehaviour = new PlayerMovement(1, 0, timeServiceSubstitute);
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
					movementBehaviour = new PlayerMovement(1, 0, timeServiceSubstitute);
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
						movementBehaviour.TimeService.DeltaTime * movementBehaviour.Speed * new Vector3(1, 0);

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
				public void default_DashSpeed_is_greater_than_zero() => Assert.Greater(movementBehaviour.DashSpeed, 0);

				[Test]
				public void default_DashSpeed_has_the_value_of_DEFAULT_DASH_SPEED_constant() =>
					Assert.AreEqual(PlayerMovement.DEFAULT_DASH_SPEED, movementBehaviour.DashSpeed);

				[Test]
				public void Vector2_zero_does_nothing()
				{
					movementBehaviour.TransformProvider.Position = new Vector2(5, 2);

					movementBehaviour.PerformDash(Vector2.zero);

					Assert.AreEqual(new Vector3(5, 2), movementBehaviour.TransformProvider.Position);
				}
			}
		}

		public static class Looking {}
	}
}