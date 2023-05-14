using System;
using UnityEngine;

namespace ZadanieRekrutacyjne.Game.Elevator
{
    public class PhotoCell : MonoBehaviour
    {
        public bool IsDoorBlocked {  get; private set; }

        public event Action DoorBlocked;

        private void OnTriggerEnter(Collider other)
        {
            IsDoorBlocked = true;
            DoorBlocked?.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            IsDoorBlocked = false;
        }
    }
}
