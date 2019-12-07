using System.Collections;
using UnityEngine;

namespace Game.GameSystemServices.CoroutineRunners
{
	public sealed class UnityCoroutineRunner : ICoroutineRunner
	{
		private readonly MonoBehaviour _monoBehaviour;

		public UnityCoroutineRunner(MonoBehaviour monoBehaviour) => _monoBehaviour = monoBehaviour;

		public void StartCoroutine(IEnumerator routine) => _monoBehaviour.StartCoroutine(routine);
	}
}