using Game.GameSystemServices;
using UnityEngine;

namespace Game.Players.Dashing
{
	public abstract class BaseDashingComponent : BaseComponent<IDashingBehaviour>
	{
		[Header("Base dashing component")]
		[SerializeField] protected float distance = PlayerDashing.DEFAULT_DISTANCE;
		[SerializeField] protected float speed = PlayerDashing.DEFAULT_SPEED;
		[SerializeField] protected float cooldown = PlayerDashing.DEFAULT_COOLDOWN;
		[SerializeField] protected AnimationCurve movementCurve = AnimationCurve.Linear(0, 0, 1, 1);
	}
}