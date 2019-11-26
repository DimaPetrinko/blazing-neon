using Game.Players;
using NSubstitute;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tests
{
	public sealed class Player
	{
		private static Game.Players.Player player;

		private static Game.Players.Player InstantiatePlayer()
		{
			var prefab =
				AssetDatabase.LoadAssetAtPath<Game.Players.Player>("Assets/InternalAssets/Prefabs/Player.prefab");
			var newPlayer = Object.Instantiate(prefab);
			return newPlayer;
		}

		[SetUp]
		public static void PlayerSetup()
		{
			player = InstantiatePlayer();
		}

		[Test]
		public void player_spawned()
		{
			Assert.NotNull(player);
		}

		[Test]
		public void player_components_have_been_assigned_correctly_during_init_call()
		{
			player.Init();

			Assert.NotNull(player.InputBehaviour);
			Assert.NotNull(player.MovementBehaviour);
			Assert.NotNull(player.LookingBehaviour);
		}

		public sealed class MovementBehaviour
		{
			private PlayerMovement movementBehaviour;

			[SetUp]
			public void MovementBehaviourSetup()
			{
				PlayerSetup();
				movementBehaviour = player.GetComponent<PlayerMovement>();
			}

			[Test]
			public void unit_vector_moves_player_according_to_speed()
			{
				var currentPosition = movementBehaviour.Position;
				var movementVector = Vector2.right;
				var supposedMovementDelta = movementBehaviour.Speed * Time.deltaTime * movementVector.normalized;

				player.Init();
				movementBehaviour.Init();
				movementBehaviour.PerformMovement(movementVector);

				var newPosition = movementBehaviour.Position;
				Vector2 movementDelta = newPosition - currentPosition;

				Assert.AreNotEqual(Vector2.zero, movementDelta);
				Assert.AreEqual(supposedMovementDelta, movementDelta);
			}

			[Test]
			public void non_unit_vector_moves_player_according_to_speed()
			{
				var currentPosition = movementBehaviour.Position;
				var movementVector = new Vector2(2, 5);
				var supposedMovementDelta = movementBehaviour.Speed * Time.deltaTime * movementVector.normalized;

				movementBehaviour.Init();
				movementBehaviour.PerformMovement(movementVector);

				var newPosition = movementBehaviour.Position;
				Vector2 movementDelta = newPosition - currentPosition;

				Assert.AreNotEqual(Vector2.zero, movementDelta);
				Assert.AreEqual(supposedMovementDelta, movementDelta);
			}
		}

		public sealed class LookingBehaviour
		{
			private PlayerLooking lookBehaviour;

			[SetUp]
			public void LookingBehaviourSetup()
			{
				PlayerSetup();
				lookBehaviour = player.GetComponent<PlayerLooking>();
			}

			[Test]
			public void diagonal_mouse_position_rotates_player_in_45_degrees_to_the_right()
			{
				var screenToWorldPointProviderSubstitute = Substitute.For<IScreenToWorldPointProvider>();
				var lookDirection = new Vector2(45, 45);
				screenToWorldPointProviderSubstitute.Get(lookDirection, player.Position)
					.Returns((Vector3)lookDirection.normalized);
				lookBehaviour.ScreenToWorldPointProvider = screenToWorldPointProviderSubstitute;
				player.Position = Vector3.zero;
				player.Rotation = Quaternion.identity;

				lookBehaviour.PerformLookingWithMouse(lookDirection);

				if (player.Rotation.eulerAngles.z > 0) Assert.AreEqual(315, player.Rotation.eulerAngles.z);
				else Assert.AreEqual(-45, player.Rotation.eulerAngles.z);
			}

			[Test]
			public void mouse_position_to_the_right_rotates_player_to_the_right()
			{
				var screenToWorldPointProviderSubstitute = Substitute.For<IScreenToWorldPointProvider>();
				player.Position = Vector3.zero;
				player.Rotation = Quaternion.identity;
				var lookDirection = new Vector2(500, 0);
				screenToWorldPointProviderSubstitute.Get(lookDirection, player.Position)
					.Returns((Vector3)lookDirection.normalized);
				lookBehaviour.ScreenToWorldPointProvider = screenToWorldPointProviderSubstitute;

				lookBehaviour.PerformLookingWithMouse(lookDirection);

				if (player.Rotation.eulerAngles.z > 0) Assert.AreEqual(270, player.Rotation.eulerAngles.z);
				else Assert.AreEqual(-90, player.Rotation.eulerAngles.z);
			}

			[Test]
			public void controller_stick_to_the_right_rotates_player_to_the_right()
			{
				var lookDirection = Vector2.right;
				player.Rotation = Quaternion.identity;

				lookBehaviour.PerformLookingWithGamepad(lookDirection);

				if (player.Rotation.eulerAngles.z > 0) Assert.AreEqual(270, player.Rotation.eulerAngles.z);
				else Assert.AreEqual(-90, player.Rotation.eulerAngles.z);
			}

			[Test]
			public void controller_stick_to_the_diagonal_rotates_player_diagonally()
			{
				var lookDirection = new Vector2(1, 1).normalized;
				player.Rotation = Quaternion.identity;

				lookBehaviour.PerformLookingWithGamepad(lookDirection);

				if (player.Rotation.eulerAngles.z > 0) Assert.AreEqual(315, player.Rotation.eulerAngles.z);
				else Assert.AreEqual(-45, player.Rotation.eulerAngles.z);
			}

			[Test]
			public void zero_controller_stick_direction_does_not_change_rotation()
			{
				var playerRotationAngle = Random.Range(0f, 359f);
				player.Rotation = Quaternion.Euler(0, 0, playerRotationAngle);
				playerRotationAngle = player.Rotation.eulerAngles.z;

				lookBehaviour.PerformLookingWithGamepad(Vector2.zero);

				Assert.AreEqual(playerRotationAngle, player.Rotation.eulerAngles.z);
			}
		}
	}
}