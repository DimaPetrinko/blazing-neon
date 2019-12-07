using UnityEngine;

namespace Game.GameSystemServices
{
	public abstract class BaseComponent<T> : BaseComponent where T : class
	{
		[Header("Base component")]
		[SerializeField] private bool _createInstanceOnAwake = true;

		private T _instance;

		public T Instance => _instance ?? (_instance = CreateInstance());

		public override object BaseInstance => Instance;

		private void Awake()
		{
			if (_createInstanceOnAwake) _instance = CreateInstance();
		}

		protected abstract T CreateInstance();
	}

	public abstract class BaseComponent : MonoBehaviour
	{
		public abstract object BaseInstance { get; }
	}
}