using UnityEngine;

namespace Game.Players
{
	[RequireComponent(typeof(PlayerInput))]
	[RequireComponent(typeof(PlayerMovement))]
	[RequireComponent(typeof(PlayerLooking))]
	public sealed class Player : MonoBehaviour
	{
		private PlayerInput playerInput;
		private PlayerMovement playerMovement;
		private PlayerLooking playerLooking;

		private void Awake()
		{
			playerInput = GetComponent<PlayerInput>();
			playerMovement = GetComponent<PlayerMovement>();
			playerLooking = GetComponent<PlayerLooking>();
			playerInput.Dash += () => playerMovement.PerformDash(playerInput.MovementDirection);
		}

		private void FixedUpdate()
		{
			playerMovement.PerformMovement(playerInput.MovementDirection);
			playerLooking.PerformLooking(playerInput.LookDirection);
		}
	}
}