using System;
using System.Collections;
using Game.Players;
using NSubstitute;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace Tests
{
	public sealed class PlayerTests
	{ 
		private Player InstantiatePlayer()
		{
			var prefab = AssetDatabase.LoadAssetAtPath<Player>("Assets/InternalAssets/Prefabs/Player.prefab");
			var player = Object.Instantiate(prefab);
			return player;
		}

		[Test]
		public void PlayerSpawned()
		{
			var player = InstantiatePlayer();
			Assert.NotNull(player);
		}

		[Test]
		public void PlayerInputBehaviourAcquiredCorrectly()
		{
			var player = InstantiatePlayer();
			player.Init();

			Assert.NotNull(player.InputBehaviour);
			Assert.NotNull(player.MovementBehaviour);
			Assert.NotNull(player.LookingBehaviour);
		}
		
		[Test]
		public void DashEventWasCalled()
		{
			var inputBehaviour = Substitute.For<IInputBehaviour>();
			var wasCalled = false;
			inputBehaviour.Dash += _ => wasCalled = true;
			inputBehaviour.Dash += Raise.Event<Action<Vector2>>(Arg.Any<Vector2>());

			Assert.True(wasCalled);
		}

		[Test]
		public void DashEventIsReceivedByPlayerMovement()
		{
			var inputBehaviour = Substitute.For<IInputBehaviour>();
			var movementBehaviour = Substitute.For<IMovementBehaviour>();

			inputBehaviour.Dash += movementBehaviour.PerformDash;
			inputBehaviour.Dash += Raise.Event<Action<Vector2>>(Arg.Any<Vector2>());

			movementBehaviour.Received().PerformDash(Arg.Any<Vector2>());
		}

		[Test]
		public void PlayerMovedInAccordanceToSpeed()
		{
			var player = InstantiatePlayer();
			var movementBehaviour = player.GetComponent<PlayerMovement>();
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

		// A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
		// `yield return null;` to skip a frame.
		[UnityTest]
		public IEnumerator EditModeTestsWithEnumeratorPasses()
		{
			// Use the Assert class to test conditions.
			// Use yield to skip a frame.
			yield return null;
		}
	}
}