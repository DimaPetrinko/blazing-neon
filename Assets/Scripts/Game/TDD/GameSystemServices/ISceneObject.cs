using Game.TDD.GameSystemServices.TransformProviders;

namespace Game.TDD.GameSystemServices
{
	public interface ISceneObject
	{
		ITransformProvider TransformProvider { get; set; }
	}
}