using UnityEngine;

namespace Game.Players.TDD
{
	public abstract class BaseBehaviourComponent<T> : MonoBehaviour
	{
		public abstract T Behaviour { get; }
		private void Awake() => CreateBehaviour();
		protected abstract T CreateBehaviour();
	}
}