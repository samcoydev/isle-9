using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] private LayerMask _interactableMask;
    [SerializeField] private float maxDistance;
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private InteractionPromptUI interactionPromptUI;
    [SerializeField] private PlayerData playerData;
    public Transform playerTransform;

    private IInteractable interactable;

    private void Update()
    {
        var ray = new Ray(this.transform.position, this.transform.forward);
        RaycastHit hit;
        if  (Physics.Raycast(ray, out hit, maxDistance, _interactableMask))
        {
            interactable = hit.collider.gameObject.GetComponent<IInteractable>();

            if (interactable != null)
            {
                if (!interactionPromptUI.isDisplayed) 
                    interactionPromptUI.SetUp(interactable.InteractionPrompt);

                if (Input.GetKeyDown(interactKey))
                    interactable.Interact(this);
            }
        }
        else
        {
            if (interactable != null) interactable = null;
            if (interactionPromptUI.isDisplayed) interactionPromptUI.Close();
        }

    }

    public PlayerData GetPlayerData()
    {
        return playerData;
    }
}
