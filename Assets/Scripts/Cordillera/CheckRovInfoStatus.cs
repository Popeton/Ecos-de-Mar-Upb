using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CheckRovInfoStatus : MonoBehaviour
{
    public bool isRovInfoActive = false;

    [SerializeField] private GameObject rovPhotoPanel;
    [SerializeField] private GameObject rovInfoPanel;
    [SerializeField] private VRInteractionHandler interactionHandler;

    private XRSimpleInteractable rovInteractable;

    void Start()
    {
        // Initialize the Rov Interactable
        rovInteractable = GetComponent<XRSimpleInteractable>();

        if (rovInteractable != null && interactionHandler != null)
        {
            // Añadir la interacción con el cangrejo al VRInteractionHandler
            interactionHandler.AddInteractable(rovInteractable);

            // Suscribirse al evento de interacción
            interactionHandler.OnInteractionStarted += OnRovInteractionStarted;
        }
        else
        {
            Debug.LogError("Error: Falta alguna referencia en CrabInteractions.");
        }

    }
    private void OnRovInteractionStarted(XRSimpleInteractable interactable)
    {
        if (interactable == rovInteractable && isRovInfoActive)
        {
            CheckRovInfoStatusActive();
        }
        else if (interactable == rovInteractable && !isRovInfoActive)
        {
            RovDescription();
        }
    }


    private void RovDescription()
    {
        if (rovInfoPanel != null)
        {
            // Activate the Rov Info panel
            AudioManager.instance.PlaySoundByName("Sound1");
            rovInfoPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Rov Info panel is not assigned.");
        }
    }
    private void CheckRovInfoStatusActive()
    {
        if (isRovInfoActive)
        {
            // Activate the Rov Info panel
            if (rovPhotoPanel != null)
            {
                AudioManager.instance.PlaySoundByName("Sound3");
                rovPhotoPanel.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Rov Info panel is not assigned.");
            }
           
        }
        else
        {
            Debug.Log("Rov Info is not active.");
            // Additional logic can be added here if needed
            if (rovPhotoPanel != null)
            {
                rovPhotoPanel.SetActive(false);
            }
            else
            {
                Debug.LogWarning("Rov Info panel is not assigned.");
            }
        }
    }
}
