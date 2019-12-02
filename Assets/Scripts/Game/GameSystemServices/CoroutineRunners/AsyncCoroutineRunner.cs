using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using ThreadPriority = System.Threading.ThreadPriority;

namespace Game.GameSystemServices.CoroutineRunners
{
	public sealed class AsyncCoroutineRunner : ICoroutineRunner
	{
		public void StartCoroutine(IEnumerator routine)
		{
			var originalPriority = Thread.CurrentThread.Priority;
			Thread.CurrentThread.Priority = ThreadPriority.Lowest;
			Task.Run(() => RunRoutine(routine));
			Thread.Sleep(100);
			Thread.CurrentThread.Priority = originalPriority;
		}

		private void RunRoutine(IEnumerator routine)
		{
			Thread.CurrentThread.Priority = ThreadPriority.Highest;
			while (routine.MoveNext())
			{
				if (routine.Current is WaitForSeconds) Thread.Sleep(500);
			}
		}
	}
}