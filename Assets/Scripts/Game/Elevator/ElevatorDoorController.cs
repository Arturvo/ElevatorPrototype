using System;
using UnityEngine;
using ZadanieRekrutacyjne.Core.AudioSystem;
using Zenject;

namespace ZadanieRekrutacyjne.Game.Elevator
{
    public class ElevatorDoorController : MonoBehaviour
    {
        [SerializeField] private PhotoCell photoCell;

        [Inject] private readonly IAudioManager audioManager;

        private Animator doorAnimator;
        private bool waitingForDoorsToClose;
        private AudioSource currentDoorSoundClip;
        private event Action onDoorsClosedCallback;

        private const string elevatorBingAndMoveSound = "ElevatorDoorBingAndMove";
        private const string elevatorMoveSound = "ElevatorDoorMove";

        private readonly int openDoorAnimationHash =  Animator.StringToHash("ElevatorDoorOpen");
        private readonly int openDoorTriggerHash = Animator.StringToHash("OpenDoor");
        private readonly int closeDoorTriggerHash = Animator.StringToHash("CloseDoor");

        private void Awake()
        {
            doorAnimator = GetComponent<Animator>();
            photoCell.DoorBlocked += OnDoorBlocked;
        }

        public void OpenDoor()
        {
            waitingForDoorsToClose = false;
            if (doorAnimator.enabled)
            {
                if (doorAnimator.GetCurrentAnimatorStateInfo(0).IsName("ElevatorDoorClose"))
                {
                    doorAnimator.SetTrigger(openDoorTriggerHash);
                    PlayElevatorDoorSound(elevatorBingAndMoveSound);
                }
            }
            else
            {
                doorAnimator.enabled = true;
                PlayElevatorDoorSound(elevatorBingAndMoveSound);
            }
        }

        public void CloseDoor(Action onDoorsClosedCallback)
        {
            if (!photoCell.IsDoorBlocked && doorAnimator.GetCurrentAnimatorStateInfo(0).IsName("ElevatorDoorOpen") && doorAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                waitingForDoorsToClose = true;
                this.onDoorsClosedCallback = onDoorsClosedCallback;
                PlayElevatorDoorSound(elevatorMoveSound);
                doorAnimator.SetTrigger(closeDoorTriggerHash);
            }
        }

        private void OnDoorBlocked()
        {
            waitingForDoorsToClose = false;
            if (doorAnimator.GetCurrentAnimatorStateInfo(0).IsName("ElevatorDoorClose"))
            {
                float animationProgress = doorAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                if (animationProgress < 1)
                {
                    PlayElevatorDoorSound(elevatorMoveSound);
                    doorAnimator.Play(openDoorAnimationHash, 0, 1 - animationProgress);
                }
            }
        }

        private void PlayElevatorDoorSound(string sound)
        {
            currentDoorSoundClip?.Stop();
            currentDoorSoundClip = audioManager.PlayAndGetSource(sound);
        }

        // Animation event invoked at the start of "ElevatorDoorOpen" animation (which in turn invokes at the end of "ElevatorDoorClose")
        private void OnDoorClosedEvent()
        {
            if (waitingForDoorsToClose)
            {
                onDoorsClosedCallback?.Invoke();
            }
        }
    }
}
