using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;
    [SerializeField] private Transform destination;

    [SerializeField] private float floatSpeed;
    [SerializeField] private float floatFactor;
    private Vector3 origin;

    public string InteractionPrompt => _prompt;

    public bool Interact(Interactor interactor)
    {
        interactor.playerTransform.position = destination.position;
        return true;
    }

    public bool IsDisabled() {
        return false;
    }

    private void Start() {
        origin = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, this.transform.localPosition.z);
    }

    private void Update() {
        this.transform.localPosition = new Vector3(origin.x, origin.y + (Mathf.Sin((Time.time * floatSpeed)) * floatFactor), origin.z);
    }
}
