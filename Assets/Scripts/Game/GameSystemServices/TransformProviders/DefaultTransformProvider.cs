using UnityEngine;

namespace Game.GameSystemServices.TransformProviders
{
	public sealed class DefaultTransformProvider : ITransformProvider
	{
		public GameObject GameObject => null;
		public Vector3 Position { get; set; }
		public Quaternion Rotation { get; set; }
		public Vector3 LocalScale { get; set; }
		public void Translate(Vector3 byVector, Space space = Space.Self) => Position += (Vector3)byVector;

		public void Rotate(Vector3 axis, float angle, Space space = Space.Self) =>
			Rotation *= Quaternion.AngleAxis(angle, axis);

		public void Scale(Vector3 byVector) => LocalScale.Scale(byVector);
	}
}