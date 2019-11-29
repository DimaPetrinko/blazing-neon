using System.Collections;
using UnityEngine;

namespace Game.TDD.GameSystemServices
{
	public interface ICoroutineRunner
	{
		void StartCoroutine(IEnumerator routine);
	}

	public sealed class UnityCoroutineRunner : ICoroutineRunner
	{
		private readonly MonoBehaviour monoBehaviour;

		public UnityCoroutineRunner(MonoBehaviour monoBehaviour) => this.monoBehaviour = monoBehaviour;

		public void StartCoroutine(IEnumerator routine) => monoBehaviour.StartCoroutine(routine);
	}

	public sealed class DefaultCoroutineRunner : ICoroutineRunner
	{
		public void StartCoroutine(IEnumerator routine)
		{
			while (routine.MoveNext()) ;
		}
	}
}