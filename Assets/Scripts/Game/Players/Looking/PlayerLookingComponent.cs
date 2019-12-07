using Game.GameSystemServices;
using UnityEngine;

namespace Game.Players.Looking
{
	public sealed class PlayerLookingComponent : BaseComponent<ILookingBehaviour>
	{
		[Header("Player looking component")]
		[SerializeField] private Camera _cam;

		protected override ILookingBehaviour CreateInstance() =>
			new PlayerLooking(transform, new UnityWorldToScreenProvider(_cam));
	}
}