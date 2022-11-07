using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;

    [SerializeField] private Transform destination;

    public string InteractionPrompt => _prompt;

    public bool Interact(Interactor interactor)
    {
        interactor.playerTransform.position = destination.position;
        return true;
    }
}
