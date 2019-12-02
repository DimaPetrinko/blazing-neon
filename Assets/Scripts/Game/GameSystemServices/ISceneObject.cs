using Game.GameSystemServices.TransformProviders;

namespace Game.GameSystemServices
{
	public interface ISceneObject
	{
		ITransformProvider TransformProvider { get; set; }
	}
}