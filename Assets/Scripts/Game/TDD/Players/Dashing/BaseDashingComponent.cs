using Game.TDD.GameSystemServices;
using UnityEngine;

namespace Game.TDD.Players.Dashing
{
	public abstract class BaseDashingComponent : BaseComponent<IDashingBehaviour>
	{
		[Header("Base dashing component")]
		[SerializeField] protected float dashDistance = PlayerDashing.DEFAULT_DASH_DISTANCE;

		[SerializeField] protected float dashDuration = PlayerDashing.DEFAULT_DASH_DURATION;
		[SerializeField] protected AnimationCurve dashSpeedCurve = AnimationCurve.Linear(0, 0, 1, 1);
	}
}