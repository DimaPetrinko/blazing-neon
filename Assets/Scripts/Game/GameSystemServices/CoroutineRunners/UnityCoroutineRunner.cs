using System.Collections;
using UnityEngine;

namespace Game.GameSystemServices.CoroutineRunners
{
	public sealed class UnityCoroutineRunner : ICoroutineRunner
	{
		private readonly MonoBehaviour monoBehaviour;

		public UnityCoroutineRunner(MonoBehaviour monoBehaviour) => this.monoBehaviour = monoBehaviour;

		public void StartCoroutine(IEnumerator routine) => monoBehaviour.StartCoroutine(routine);
	}
}