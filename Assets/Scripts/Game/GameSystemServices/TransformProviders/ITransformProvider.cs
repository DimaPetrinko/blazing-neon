using UnityEngine;

namespace Game.GameSystemServices.TransformProviders
{
	public interface ITransformProvider
	{
		GameObject GameObject { get; }
		Vector3 Position { get; set; }
		Quaternion Rotation { get; set; }
		Vector3 LocalScale { get; set; }

		void Translate(Vector2 byVector, Space space = Space.Self);
		void Rotate(Vector3 axis, float angle, Space space = Space.Self);
		void Scale(Vector3 byVector);
	}
}