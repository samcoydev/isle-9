using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractionPromptUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _promptText;
    [SerializeField] private GameObject textObject;
    public bool isDisplayed = false;

    public void SetUp(string promptText)
    {
        _promptText.text = promptText;
        isDisplayed = true;
    }

    public void Close()
    {
        _promptText.text = "";
        isDisplayed = false;
    }
}
