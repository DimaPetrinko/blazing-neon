using UnityEngine;

namespace Game.Players
{
	public abstract class TestableMonoBehaviour : MonoBehaviour, IComponent
	{
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

		public Vector3 Scale
		{
			get => transform.localScale;
			set => transform.localScale = value;
		}

		private void Awake() => Init();
		private void OnEnable() => Enabled();
		private void OnDisable() => Disabled();
		private void OnDestroy() => Destroyed();
		private void Update() => UpdateMethod();
		private void FixedUpdate() => FixedUpdateMethod();

		public virtual void Init() {}
		public virtual void Enabled() {}
		public virtual void Disabled() {}
		public virtual void Destroyed() {}
		public virtual void FixedUpdateMethod() {}
		public virtual void UpdateMethod() {}
	}
}