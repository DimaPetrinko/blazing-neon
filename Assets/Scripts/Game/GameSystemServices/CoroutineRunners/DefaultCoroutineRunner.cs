using System.Collections;

namespace Game.GameSystemServices.CoroutineRunners
{
	public sealed class DefaultCoroutineRunner : ICoroutineRunner
	{
		public void StartCoroutine(IEnumerator routine)
		{
			while (routine.MoveNext()) ;
		}
	}
}