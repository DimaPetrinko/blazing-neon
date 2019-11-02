using UnityEngine;

namespace Game
{
	[RequireComponent(typeof(PlayerInput))]
	[RequireComponent(typeof(PlayerMovement))]
	public sealed class Player : MonoBehaviour
	{
		private PlayerInput playerInput;
		private PlayerMovement playerMovement;

		private void Awake()
		{
			playerInput = GetComponent<PlayerInput>();
			playerMovement = GetComponent<PlayerMovement>();
			playerInput.Dash += () => playerMovement.PerformDash(playerInput.MovementDirection);
		}

		private void FixedUpdate() => playerMovement.PerformMovement(playerInput.MovementDirection);
	}
}