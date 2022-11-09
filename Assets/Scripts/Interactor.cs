using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour {

    [SerializeField] private LayerMask _interactableMask;
    [SerializeField] private float maxDistance;
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private InteractionPromptUI interactionPromptUI;
    [SerializeField] private PlayerData playerData;

    public Transform playerTransform;
    private IInteractable interactable;
    private bool canInteract = true;

    private void Update() {
        if (canInteract)
            CheckForInteractable();
    }

    public void CheckForInteractable() {
        var ray = new Ray(this.transform.position, this.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxDistance, _interactableMask)) {
            interactable = hit.collider.gameObject.GetComponent<IInteractable>();

            if (interactable != null && !interactable.IsDisabled()) {
                if (!interactionPromptUI.isDisplayed)
                    interactionPromptUI.SetUp(interactable.InteractionPrompt);

                if (Input.GetKeyDown(interactKey)) {
                    interactable.Interact(this);
                    interactionPromptUI.Close();
                    StartCoroutine(InteractCooldown());
                }
            }
        } else {
            if (interactable != null) interactable = null;
            if (interactionPromptUI.isDisplayed) interactionPromptUI.Close();
        }
    }

    private IEnumerator InteractCooldown() {
        canInteract = false;
        yield return new WaitForSeconds(.2f);
        canInteract = true;
    }

    public PlayerData GetPlayerData() {
        return playerData;
    }
}
