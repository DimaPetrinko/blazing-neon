using Game.GameSystemServices;
using UnityEngine;

namespace Game.Players.Movement
{
	public abstract class BaseMovementComponent : BaseComponent<IMovementBehaviour>
	{
		[Header("Base movement component")]
		[SerializeField] protected float speed = PlayerMovement.DEFAULT_SPEED;}
}