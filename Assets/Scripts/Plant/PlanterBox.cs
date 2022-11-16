using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanterBox : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;
    [SerializeField] private int cost;

    public string InteractionPrompt => _prompt;
    public GameObject productToPlant;

    private IPlant plantScript;
    private bool isDisabled;
    private Vector3 origin;

    [Header("Shake")]
    public float shakeSpeed;
    public float shakeTime;
    public float shakeFactor;
    public bool isShaking;

    private void Start() {
        origin = this.transform.localPosition;
        plantScript = productToPlant.GetComponent<IPlant>();
        _prompt = $"(E) Plant {plantScript.GetName()} Tree ({cost}g)";
    }

    private void Update() {
        if (isShaking)
            this.transform.localPosition = new Vector3(Mathf.Sin(Time.time * shakeSpeed) * shakeFactor, origin.y, origin.z);
    }

    public bool Interact(Interactor interactor)
    {
        if (isDisabled) { return true; }

        if (interactor.GetPlayerData().money >= cost) {
            plantScript.StartGrowth();
            interactor.GetPlayerData().SubtractMoney(cost);
            Disable();
        } else {
            StartCoroutine(Shake());
        }

        return true;
    }

    private IEnumerator Shake() {
        isShaking = true;
        yield return new WaitForSeconds(shakeTime);
        isShaking = false;
    }

    private void Disable() {
        isDisabled = true;
        _prompt = "";
    }

    public bool IsDisabled() {
        return isDisabled;
    }
}
