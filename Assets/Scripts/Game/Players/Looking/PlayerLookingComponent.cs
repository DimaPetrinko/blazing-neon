using Game.GameSystemServices;
using UnityEngine;

namespace Game.Players.Looking
{
	public sealed class PlayerLookingComponent : BaseComponent<ILookingBehaviour>
	{
		[SerializeField] private Camera cam;

		protected override ILookingBehaviour CreateInstance() =>
			new PlayerLooking(transform, new UnityWorldToScreenProvider(cam));
	}
}