using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using ZadanieRekrutacyjne.Core.InputSystem;
using Zenject;

namespace ZadanieRekrutacyjne.Game.Player.Interaction
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private Camera playerCamera;
        [SerializeField] private float interactionRange = 10f;

        [Inject] private readonly IInputManager inputManager;

        private Interactable currentlySelected;
        private int interactableLayerMask;

        private void Awake()
        {
            FindInteractableLayerMask();
        }

        private void OnEnable()
        {
            inputManager.PlayerInputActions.Game.Interact.performed += OnInteract;
        }

        private void Update()
        {
            FindAndSelectInteractable();
        }

        private void OnDisable()
        {
            inputManager.PlayerInputActions.Game.Interact.performed -= OnInteract;
        }

        private void FindInteractableLayerMask()
        {
            int interactableLayer = LayerMask.NameToLayer(Interactable.interactableLayerName);
            Assert.IsTrue(interactableLayer >= 0, "Interactable layer name is incorrectly defined");
            interactableLayerMask = 1 << interactableLayer;
        }

        private void FindAndSelectInteractable()
        {
            bool hitInteractableObject = Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out var hitData, interactionRange, interactableLayerMask);

            if (hitInteractableObject)
            {
                bool hitAlreadySelectedObject = currentlySelected != null && !ReferenceEquals(currentlySelected.gameObject, hitData.transform.gameObject);

                if (!hitAlreadySelectedObject)
                {
                    currentlySelected?.Deselect();
                    currentlySelected = hitData.transform.GetComponent<Interactable>();
                    currentlySelected.Select();
                }
            }
            else if (currentlySelected != null)
            {
                currentlySelected.Deselect();
                currentlySelected = null;
            }
        }

        private void OnInteract(InputAction.CallbackContext callbackContext)
        {
            currentlySelected?.Interact();
        }
    }
}
