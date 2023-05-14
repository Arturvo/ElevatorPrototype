using UnityEngine;

namespace ZadanieRekrutacyjne.Core.InputSystem
{
    public class InputManager : MonoBehaviour, IInputManager
    {
        public PlayerInputActions PlayerInputActions {  get; private set; }

        private void Awake()
        {
            PlayerInputActions = new PlayerInputActions();
            PlayerInputActions.Game.Enable();
        }
    }
}