using UnityEngine;

namespace Game.TDD.GameSystemServices
{
	public abstract class BaseComponent<T> : MonoBehaviour where T : class
	{
		[Header("Base component")]
		[SerializeField] private bool createInstanceOnAwake = true;

		private T instance;

		public T Instance => instance ?? (instance = CreateInstance());

		private void Awake()
		{
			if (createInstanceOnAwake) CreateInstance();
		}

		protected abstract T CreateInstance();
	}
}