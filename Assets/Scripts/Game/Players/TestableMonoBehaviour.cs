using UnityEngine;

namespace Game.Players
{
	public abstract class TestableMonoBehaviour : MonoBehaviour, IComponent
	{
		public Vector3 Position => transform.position;
		public Quaternion Rotation => transform.rotation;
		public Vector3 Scale => transform.localScale;

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