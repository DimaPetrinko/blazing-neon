using Game.TDD.GameSystemServices;
using UnityEngine;

namespace Game.TDD.Players.Movement
{
	public abstract class BaseMovementComponent : BaseComponent<IMovementBehaviour>
	{
		[Header("Base movement component")]
		[SerializeField] protected float speed = PlayerMovement.DEFAULT_SPEED;
		[SerializeField] protected float dashDistance = PlayerMovement.DEFAULT_DASH_DISTANCE;
		[SerializeField] protected float dashDuration = PlayerMovement.DEFAULT_DASH_DURATION;
		[SerializeField] protected AnimationCurve dashSpeedCurve = AnimationCurve.Linear(0, 0, 1, 1);
	}
}