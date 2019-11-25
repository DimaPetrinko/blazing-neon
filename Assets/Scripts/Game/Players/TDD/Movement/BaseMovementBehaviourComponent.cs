using UnityEngine;

namespace Game.Players.TDD.Movement
{
	public abstract class BaseMovementBehaviourComponent : BaseBehaviourComponent<IMovementBehaviour>
	{
		[SerializeField] protected float speed;
		[SerializeField] protected float dashSpeed;
		private IMovementBehaviour movementBehaviour;

		public override IMovementBehaviour Behaviour => movementBehaviour ?? (movementBehaviour = CreateBehaviour());
	}
}