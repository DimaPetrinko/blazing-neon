using Game.TDD.GameSystemServices;
using UnityEngine;

namespace Game.TDD.Players.Movement
{
	public abstract class BaseMovementComponent : BaseComponent<IMovementBehaviour>
	{
		[Header("Base movement component")]
		[SerializeField] protected float speed = PlayerMovement.DEFAULT_SPEED;}
}