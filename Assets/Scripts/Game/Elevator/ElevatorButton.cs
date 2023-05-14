using System;
using ZadanieRekrutacyjne.Core.AudioSystem;
using ZadanieRekrutacyjne.Game.Player.Interaction;
using Zenject;

namespace ZadanieRekrutacyjne.Game.Elevator
{
    public class ElevatorButton : Interactable
    {
        public event Action ButtonPressed;

        [Inject] private readonly IAudioManager audioManager;

        private const string buttonPressSound = "ButtonPressSound";

        protected override void OnInteraction()
        {
            audioManager.Play(buttonPressSound);
            ButtonPressed?.Invoke();
        }
    }
}
