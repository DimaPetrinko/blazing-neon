using Game.Players.Old;
using UnityEngine;

namespace Game.TDD.Players.Movement
{
	public abstract class BaseMovementComponent : BaseComponent<IMovementBehaviour>
	{
		[Header("Base movement component")]
		[SerializeField] protected float speed;
		[SerializeField] protected float dashSpeed;
	}
}