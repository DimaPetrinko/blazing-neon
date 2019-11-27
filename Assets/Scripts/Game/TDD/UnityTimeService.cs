namespace Game.TDD
{
	public interface ITimeService
	{
		float Time { get; }
		float DeltaTime { get; }
		float FixedDeltaTime { get; }
	}

	public sealed class UnityTimeService : ITimeService
	{
		public float Time => UnityEngine.Time.time;
		public float DeltaTime => UnityEngine.Time.deltaTime > 0 ? UnityEngine.Time.deltaTime : 0.016666666666f;

		public float FixedDeltaTime =>
			UnityEngine.Time.fixedDeltaTime > 0 ? UnityEngine.Time.fixedDeltaTime : 0.016666666666f;
	}
}