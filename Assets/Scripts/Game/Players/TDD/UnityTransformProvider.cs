using UnityEngine;

namespace Game.Players.TDD
{
	public interface ITransformProvider
	{
		Vector3 Position { get; set; }
		Quaternion Rotation { get; set; }
		Vector3 LocalScale { get; set; }

		void Translate(Vector2 byVector, Space space = Space.Self);
		void Rotate(Vector3 axis, float angle, Space space = Space.Self);
		void Scale(Vector3 byVector);
	}

	public sealed class UnityTransformProvider : ITransformProvider
	{
		private readonly Transform transform;

		public Vector3 Position
		{
			get => transform.position;
			set => transform.position = value;
		}

		public Quaternion Rotation
		{
			get => transform.rotation;
			set => transform.rotation = value;
		}

		public Vector3 LocalScale
		{
			get => transform.localScale;
			set => transform.localScale = value;
		}

		public UnityTransformProvider(Transform transform) => this.transform = transform;

		public void Translate(Vector2 byVector, Space space = Space.Self) => transform.Translate(byVector, space);

		public void Rotate(Vector3 axis, float angle, Space space = Space.Self) => transform.Rotate(axis, angle, space);

		public void Scale(Vector3 byVector) => transform.localScale.Scale(byVector);
	}
}