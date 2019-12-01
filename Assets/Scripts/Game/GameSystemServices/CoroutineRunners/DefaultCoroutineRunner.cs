using System.Collections;

namespace Game.TDD.GameSystemServices.CoroutineRunners
{
	public sealed class DefaultCoroutineRunner : ICoroutineRunner
	{
		public void StartCoroutine(IEnumerator routine)
		{
			while (routine.MoveNext()) ;
		}
	}
}