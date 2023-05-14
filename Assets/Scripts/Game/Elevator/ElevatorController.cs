using System.Collections;
using TMPro;
using UnityEngine;
using ZadanieRekrutacyjne.Core.AudioSystem;
using ZadanieRekrutacyjne.Core.SceneLoader;
using Zenject;

namespace ZadanieRekrutacyjne.Game.Elevator
{
    public class ElevatorController : MonoBehaviour
    {
        [SerializeField] private float driveDurationPerFloor = 3f;
        [SerializeField] private ElevatorDoorController elevatorDoorController;
        [SerializeField] private TextMeshPro floorDisplayNumber;
        [SerializeField] private ElevatorButton outsideButton;
        [Tooltip("Order of inside buttons matters. Corresponding floors are resolved by button index")]
        [SerializeField] private ElevatorButton[] insideButtons;

        [Inject] private readonly ISceneLoader sceneLoader;
        [Inject] private readonly IAudioManager audioManager;

        private AudioSource elevatorTravellingSoundSource;
        private AudioSource elevatorMusicSource;
        private int targetDisplayFloor;
        private int currentDisplayFloor;

        private const string elevatorFloorBipSound = "ElevatorFloorBip";
        private const string elevatorTravellingSound = "ElevatorTraveling";
        private const string elevatorMusic = "ElevatorMusic";

        private void Awake()
        {
            floorDisplayNumber.text = sceneLoader.StartingFloorIndex.ToString();
            outsideButton.ButtonPressed += OnOuterButtonPressed;
            for (int i = 0; i < insideButtons.Length; i++)
            {
                int index = i;
                insideButtons[i].ButtonPressed += () => OnInsideButtonPressed(index);
            }
        }

        private void Start()
        {
            insideButtons[sceneLoader.StartingFloorIndex].DisableInteraction();
        }

        private void OnOuterButtonPressed()
        {
            elevatorDoorController.OpenDoor();
        }

        private void OnInsideButtonPressed(int buttonIndex)
        {
            if (sceneLoader.CurrentFloorIndex != buttonIndex)
            {
                elevatorDoorController.CloseDoor(() => SwitchFloors(buttonIndex));
            }
        }

        private void SwitchFloors(int newFloorIndex)
        {
            DisableAllButtons();
            float floorDifference = Mathf.Abs(newFloorIndex - sceneLoader.CurrentFloorIndex);
            float travelTime = floorDifference * driveDurationPerFloor;
            targetDisplayFloor = newFloorIndex;
            currentDisplayFloor = sceneLoader.CurrentFloorIndex;

            elevatorTravellingSoundSource = audioManager.PlayAndGetSource(elevatorTravellingSound, loop: true);
            elevatorMusicSource = audioManager.PlayAndGetSource(elevatorMusic, loop: true);

            StartDisplayNumberAnimation();
            sceneLoader.LoadFloorAsync(newFloorIndex, OnSwitchFloorsCompleted, travelTime);
        }

        private void OnSwitchFloorsCompleted()
        {
            elevatorTravellingSoundSource.Stop();
            elevatorMusicSource.Stop();

            floorDisplayNumber.text = sceneLoader.CurrentFloorIndex.ToString();
            elevatorDoorController.OpenDoor();
            EnableAllButtonsExceptOne(sceneLoader.CurrentFloorIndex);
        }

        private void DisableAllButtons()
        {
            for (int i = 0; i < insideButtons.Length; i++)
            {
                insideButtons[i].DisableInteraction();
            }
        }

        private void EnableAllButtonsExceptOne(int buttonIndex)
        {
            for (int i = 0; i < insideButtons.Length; i++)
            {
                if (buttonIndex != i)
                {
                    insideButtons[i].EnableInteraction();
                }
            }
        }

        private IEnumerator IncrementFloorDisplayNumber()
        {
            yield return new WaitForSeconds(driveDurationPerFloor);

            currentDisplayFloor = targetDisplayFloor > currentDisplayFloor ? currentDisplayFloor + 1 : currentDisplayFloor - 1;
            floorDisplayNumber.text = currentDisplayFloor.ToString();
            audioManager.Play(elevatorFloorBipSound);
            StartDisplayNumberAnimation();
        }

        private void StartDisplayNumberAnimation()
        {
            if (Mathf.RoundToInt(Mathf.Abs(currentDisplayFloor - targetDisplayFloor)) > 1)
            {
                StartCoroutine(IncrementFloorDisplayNumber());
            }
        }
    }
}
