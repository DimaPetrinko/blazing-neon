using Game.GameSystemServices;
using UnityEngine;

namespace Game.Players.Dashing
{
	public abstract class BaseDashingComponent : BaseComponent<IDashingBehaviour>
	{
		[Header("Base dashing component")]
		[SerializeField] protected float _distance = PlayerDashing.DEFAULT_DISTANCE;
		[SerializeField] protected float _speed = PlayerDashing.DEFAULT_SPEED;
		[SerializeField] protected float _cooldown = PlayerDashing.DEFAULT_COOLDOWN;
		[SerializeField] protected AnimationCurve _movementCurve = AnimationCurve.Linear(0, 0, 1, 1);
	}
}