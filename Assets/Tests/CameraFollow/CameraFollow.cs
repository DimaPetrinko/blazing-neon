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
			var cameraFollow = A.CameraFollow.Interface;

			Assert.NotNull(cameraFollow);
		}

		[Test]
		public void default_Target_is_null()
		{
			var cameraFollow = A.CameraFollow.Interface;

			Assert.IsNull(cameraFollow.Target);
		}

		[Test]
		public void with_Target_null_does_nothing()
		{
			var cameraFollow = A.CameraFollow.Interface;
			var target = new DefaultTransformProvider();
			var originalPosition = cameraFollow.TransformProvider.Position;

			target.Translate(Vector2.right);
			cameraFollow.Update();

			Assert.AreEqual(originalPosition, cameraFollow.TransformProvider.Position);
		}

		[Test]
		public void with_Target_not_null_and_smoothing_0_matches_position()
		{
			var target = new DefaultTransformProvider();
			var cameraFollow = A.CameraFollow.WithTarget(target).WithSmoothing(0).Interface;
			var originalPosition = cameraFollow.TransformProvider.Position;

			target.Translate(Vector2.right);
			cameraFollow.Update();

			Assert.AreNotEqual(originalPosition, cameraFollow.TransformProvider.Position);
			Assert.AreEqual(Vector3.right, cameraFollow.TransformProvider.Position);
		}

		[Test]
		public void with_Target_not_null_and_smoothing_not_0_does_not_match_position_but_still_moves_in_correct_direction()
		{
			var target = new DefaultTransformProvider();
			var cameraFollow = A.CameraFollow.WithTarget(target).WithSmoothing(0.5f).Interface;
			var originalPosition = cameraFollow.TransformProvider.Position;
			var originalTargetPosition = target.Position;

			var targetPosition = Vector3.right;
			var targetDirection = (targetPosition - originalTargetPosition).normalized; 
			target.Translate(targetPosition);
			cameraFollow.Update();
			var followDirection = (cameraFollow.TransformProvider.Position - originalPosition).normalized;

			Assert.AreNotEqual(originalPosition, cameraFollow.TransformProvider.Position);
			Assert.AreNotEqual(targetPosition, cameraFollow.TransformProvider.Position);
			Assert.AreEqual(targetDirection, followDirection);
		}

		[Test]
		public void moving_does_not_affect_position_z()
		{
			var target = new DefaultTransformProvider();
			var cameraFollow = A.CameraFollow.WithTarget(target).Interface;
			var originalZ = cameraFollow.TransformProvider.Position.z;

			target.Translate(new Vector3(0, 0, 5));
			cameraFollow.Update();

			Assert.AreEqual(originalZ, cameraFollow.TransformProvider.Position.z, float.Epsilon);
		}

		[Test]
		public void SetTarget_causes_to_start_moving_next_update()
		{
			var cameraFollow = A.CameraFollow.Interface;
			cameraFollow.TransformProvider.Position = Vector3.zero;
			ITransformProvider target = new DefaultTransformProvider();
			target.Position = new Vector3(5, 8);
			var originalPosition = cameraFollow.TransformProvider.Position;

			cameraFollow.SetTarget(target);
			cameraFollow.Update();

			Assert.AreNotEqual(originalPosition, cameraFollow.TransformProvider.Position);
		}

		[Test]
		public void ClearTarget_causes_to_stop_moving_next_update()
		{
			
			ITransformProvider target = new DefaultTransformProvider();
			target.Position = new Vector3(5, 8);
			var cameraFollow = A.CameraFollow.WithTarget(target).Interface;
			cameraFollow.Update();

			var originalPosition = cameraFollow.TransformProvider.Position;
			cameraFollow.ClearTarget();
			target.Translate(Vector2.one);

			Assert.AreEqual(originalPosition, cameraFollow.TransformProvider.Position);
		}
	}
}