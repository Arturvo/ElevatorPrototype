using UnityEngine;
using UnityEngine.InputSystem;

namespace ZadanieRekrutacyjne.Game.Player
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] private Transform playerOrientation;
        [SerializeField] private Transform cameraPosition;
        [SerializeField] private Vector2 sensitivity;

        private Vector2 currentRotation;

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            currentRotation = new Vector2(playerOrientation.rotation.eulerAngles.x, playerOrientation.rotation.eulerAngles.y);
        }

        private void Update()
        {
            MoveCamera();
            RotateCamera();
        }

        private void MoveCamera()
        {
            transform.position = cameraPosition.position;
        }

        private void RotateCamera()
        {
            if (IsMouseOverGameWindow())
            {
                float mouseX = Mouse.current.delta.x.ReadValue() * sensitivity.x;
                float mouseY = Mouse.current.delta.y.ReadValue() * sensitivity.y;

                currentRotation.y += mouseX;
                currentRotation.x -= mouseY;
                currentRotation.x = Mathf.Clamp(currentRotation.x, -90f, 90f);

                transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, 0);
                playerOrientation.rotation = Quaternion.Euler(0, currentRotation.y, 0);
            }
        }

        private bool IsMouseOverGameWindow()
        {
            Vector2 mousePosition = new Vector2(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue());
            return !(0 > mousePosition.x || 0 > mousePosition.y || Screen.width < mousePosition.x || Screen.height < mousePosition.y);
        }
    }
}