using UnityEngine;

namespace Game.Players
{
	public interface IComponent
	{
		Vector3 Position { get; }
		Quaternion Rotation { get; }
		Vector3 Scale { get; }
	}
}