using System.Collections;

namespace Game.GameSystemServices.CoroutineRunners
{
	public interface ICoroutineRunner
	{
		void StartCoroutine(IEnumerator routine);
	}
}