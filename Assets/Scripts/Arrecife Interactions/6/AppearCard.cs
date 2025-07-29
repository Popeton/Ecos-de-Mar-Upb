using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using EPOOutline;

public class AppearCard : MonoBehaviour
{
    [SerializeField] VRInteractionHandler interactionHandler;
    private XRSimpleInteractable objectInteractable;
    public bool onInteraction;
    [SerializeField] GameObject card;
    Outlinable outline;

    private void Awake()
    {
        outline = GetComponent<Outlinable>();
        objectInteractable = GetComponent<XRSimpleInteractable>();
    }
    void Start()
    {
        card.SetActive(false);
        if (objectInteractable != null && interactionHandler != null) {

            interactionHandler.AddInteractable(objectInteractable);

            // Suscribirse al evento de interacción
            interactionHandler.OnInteractionStarted += StartInteraction;
        } else {
            Debug.LogError("Error: Falta alguna referencia en CrabInteractions.");
        }
    }

    void StartInteraction(XRSimpleInteractable interactable)
    {
        if (interactable == objectInteractable && !onInteraction) {
            Appear();
        }
    }

    void Appear()
    {
        onInteraction = true;
        card.SetActive(true);
        outline.enabled = false;
        objectInteractable.enabled = false;
    }

    //testing

    private void OnEnable()
    {//testing corales no mas
        if (onInteraction) {
            GuidePath.Instance.InteractionPlastic();
            LoraHurtInteraction.Instance.AppearBottle();
            card.SetActive(false);
        }
    }

    private void OnDisable()
    {
        if (!onInteraction) {
          Appear();
       }
    }
}
