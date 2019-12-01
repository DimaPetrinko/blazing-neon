using System.Collections;

namespace Game.TDD.GameSystemServices.CoroutineRunners
{
	public interface ICoroutineRunner
	{
		void StartCoroutine(IEnumerator routine);
	}
}