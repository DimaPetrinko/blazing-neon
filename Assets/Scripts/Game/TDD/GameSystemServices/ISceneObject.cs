namespace Game.TDD.GameSystemServices
{
	public interface ISceneObject
	{
		ITransformProvider TransformProvider { get; set; }
	}
}