using UnityEngine;

namespace Game.GameSystemServices.TransformProviders
{
	public sealed class UnityTransformProvider : ITransformProvider
	{
		private readonly Transform _transform;

		public GameObject GameObject => _transform.gameObject;

		public Vector3 Position
		{
			get => _transform.position;
			set => _transform.position = value;
		}

		public Quaternion Rotation
		{
			get => _transform.rotation;
			set => _transform.rotation = value;
		}

		public Vector3 LocalScale
		{
			get => _transform.localScale;
			set => _transform.localScale = value;
		}

		public UnityTransformProvider(Transform transform) => _transform = transform;

		public void Translate(Vector3 byVector, Space space = Space.Self) => _transform.Translate(byVector, space);

		public void Rotate(Vector3 axis, float angle, Space space = Space.Self) =>
			_transform.Rotate(axis, angle, space);

		public void Scale(Vector3 byVector) => _transform.localScale.Scale(byVector);
	}
}