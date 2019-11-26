using UnityEngine;

namespace Game.Players.TDD
{
	public abstract class BaseComponent<T> : MonoBehaviour
	{
		public abstract T Behaviour { get; }
		private void Awake() => CreateBehaviour();
		protected abstract T CreateBehaviour();
	}
}