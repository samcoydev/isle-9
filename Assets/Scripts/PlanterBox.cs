using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanterBox : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;

    public GameObject productToPlant;
    private IPlant plantScript;

    public string InteractionPrompt => _prompt;

    public bool Interact(Interactor interactor)
    {
        plantScript.StartGrowth();

        return true;
    }

    private void Start()
    {
        plantScript = productToPlant.GetComponent<IPlant>();
    }
}
