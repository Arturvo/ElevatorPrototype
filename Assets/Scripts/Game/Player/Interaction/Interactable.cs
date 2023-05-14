using QuickOutline;
using UnityEngine;
using UnityEngine.Assertions;

namespace ZadanieRekrutacyjne.Game.Player.Interaction
{
    [RequireComponent(typeof(Outline))]
    public abstract class Interactable : MonoBehaviour
    {
        private Outline outline;
        private Collider interctableCollider;

        public static readonly string interactableLayerName = "Interactable";

        private void Awake()
        {
            outline = GetComponent<Outline>();
            interctableCollider = GetComponent<Collider>();
            MoveObjectToInteractableLayer();
        }

        private void Start()
        {
            outline.enabled = false;
        }

        public void EnableInteraction()
        {
            interctableCollider.enabled = true;
        }

        public void DisableInteraction()
        {
            interctableCollider.enabled = false;
        }

        public void Select()
        {
            Assert.IsFalse(outline.enabled, "Attempted to select selected interactable");
            outline.enabled = true;
        }

        public void Deselect()
        {
            Assert.IsTrue(outline.enabled, "Attempted to deselect not selected interactable");
            outline.enabled = false;
        }

        public void Interact()
        {
            OnInteraction();
        }

        protected abstract void OnInteraction();

        private void MoveObjectToInteractableLayer()
        {
            int interactableLayer = LayerMask.NameToLayer(interactableLayerName);
            Assert.IsTrue(interactableLayer >= 0, "Interactable layer name is incorrectly defined");
            if (gameObject.layer != interactableLayer)
            {
                gameObject.layer = interactableLayer;
            }
        }
    }
}
