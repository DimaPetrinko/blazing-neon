using UnityEngine;

namespace Game.GameSystemServices
{
	public abstract class BaseComponent<T> : BaseComponent where T : class
	{
		[Header("Base component")] [SerializeField]
		private bool createInstanceOnAwake = true;

		private T instance;

		public T Instance => instance ?? (instance = CreateInstance());

		public override object BaseInstance => Instance;

		private void Awake()
		{
			if (createInstanceOnAwake) instance = CreateInstance();
		}

		protected abstract T CreateInstance();
	}

	public abstract class BaseComponent : MonoBehaviour
	{
		public abstract object BaseInstance { get; }
	}
}