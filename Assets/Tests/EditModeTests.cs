using System;
using System.Collections;
using Game.Players;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
	public sealed class EditModeTests
	{
		[Test]
		public void PlayerInputBehaviourAcquiredCorrectly()
		{
			var gameObject = new GameObject();
			var player = gameObject.AddComponent<Player>();
			gameObject.AddComponent<PlayerInput>();
			gameObject.AddComponent<PlayerMovement>();
			gameObject.AddComponent<PlayerLooking>();
			
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