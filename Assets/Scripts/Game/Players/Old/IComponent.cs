using UnityEngine;

namespace Game.Players.Old
{
	public interface IComponent
	{
		Vector3 Position { get; }
		Quaternion Rotation { get; }
		Vector3 Scale { get; }
	}
}