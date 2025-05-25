using TMPro;
using UnityEngine;

public class InteractionPrompt : MonoBehaviour
{
    [SerializeField] private string interactionText = "Interactuar";
    [SerializeField] private GameObject floatingInteractionCanvas; // Canvas que contiene el texto 3D

    private void Start()
    {
        if (floatingInteractionCanvas != null)
        {
            floatingInteractionCanvas.SetActive(false);
            floatingInteractionCanvas.GetComponentInChildren<TMP_Text>().text = interactionText;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && floatingInteractionCanvas != null)
        {
            floatingInteractionCanvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && floatingInteractionCanvas != null)
        {
            floatingInteractionCanvas.SetActive(false);
        }
    }
}
