using UnityEngine;
using ZadanieRekrutacyjne.Core.InputSystem;
using Zenject;

namespace ZadanieRekrutacyjne.Game.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float movementSpeed;
        [SerializeField] private Transform orientation;
        [SerializeField] private Rigidbody playerRigidbody;

        [Inject] private readonly IInputManager inputManager;

        private Vector2 input;
        private Vector3 moveDirection;

        private void FixedUpdate()
        {
            GetInput();
            MovePlayer();
        }

        private void GetInput()
        {
            input = inputManager.PlayerInputActions.Game.MovePlayer.ReadValue<Vector2>();
        }

        private void MovePlayer()
        {
            moveDirection = orientation.forward * input.y + orientation.right * input.x;
            playerRigidbody.AddForce(moveDirection.normalized * movementSpeed, ForceMode.Force);

            Vector3 currentVelocity = new Vector3(playerRigidbody.velocity.x, 0, playerRigidbody.velocity.z);

            if (currentVelocity.magnitude > movementSpeed)
            {
                Vector3 limitedVelocity = currentVelocity.normalized * movementSpeed;
                playerRigidbody.velocity = new Vector3(limitedVelocity.x, playerRigidbody.velocity.y, limitedVelocity.z);
            }
        }
    }
}

