namespace Game.Players.TDD
{
	public interface IComponent
	{
		ITransformProvider TransformProvider { get; }
		ITimeService TimeService { get; }
	}
}