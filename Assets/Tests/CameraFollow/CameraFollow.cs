using Game.GameSystemServices.TransformProviders;
using NUnit.Framework;
using Tests.Tools.Builders;
using UnityEngine;

namespace Tests.CameraFollow
{
	public sealed class CameraFollow
	{
		[Test]
		public void CameraFollow_exists()
		{
			var cameraFollow = A.CameraFollow;

			Assert.NotNull(cameraFollow);
		}

		[Test]
		public void default_Target_is_null()
		{
			Game.CameraFollow cameraFollow = A.CameraFollow;
			
			Assert.IsNull(cameraFollow.Target);
		}

		[Test]
		public void with_Target_null_does_nothing()
		{
			Game.CameraFollow cameraFollow = A.CameraFollow;
			var target = new DefaultTransformProvider();
			var originalPosition = cameraFollow.TransformProvider.Position;

			target.Translate(Vector2.right);
			cameraFollow.LateUpdate();

			Assert.AreEqual(originalPosition, cameraFollow.TransformProvider.Position);
		}

		[Test]
		public void with_Target_not_null_matches_position()
		{
			var target = new DefaultTransformProvider();
			Game.CameraFollow cameraFollow = A.CameraFollow.WithTarget(target);
			var originalPosition = cameraFollow.TransformProvider.Position;

			target.Translate(Vector2.right);
			cameraFollow.LateUpdate();

			Assert.AreNotEqual(originalPosition, cameraFollow.TransformProvider.Position);
			Assert.AreEqual(Vector3.right, cameraFollow.TransformProvider.Position);
		}
	}
}