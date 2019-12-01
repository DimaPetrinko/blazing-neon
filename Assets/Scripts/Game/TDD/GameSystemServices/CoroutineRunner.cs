using System.Collections;
using System.Threading;
using UnityEditor.VersionControl;
using UnityEngine;
using Task = System.Threading.Tasks.Task;
using ThreadPriority = System.Threading.ThreadPriority;

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
		private readonly bool immediate;

		public DefaultCoroutineRunner(bool immediate = true)
		{
			this.immediate = immediate;
		}

		public async void StartCoroutine(IEnumerator routine)
		{
			void Routine()
			{
				Thread.CurrentThread.Priority = ThreadPriority.Highest;
				while (routine.MoveNext())
				{
					if (immediate) continue;
					if (routine.Current is WaitForSeconds) Thread.Sleep(1000);
//					Thread.Sleep(17);
				}
			}

			if (immediate) Routine();
			await Task.Run(Routine);
		}
	}
}