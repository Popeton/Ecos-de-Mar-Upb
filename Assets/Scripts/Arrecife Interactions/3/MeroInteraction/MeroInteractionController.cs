using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
public class MeroInteractionController : MonoBehaviour
{
    [SerializeField] float delayInteraction;
    [SerializeField] GameObject meroCard;

    //FishPathFollowing fishMov;
    Outlinable outline;
    Collider coll;
    XRSimpleInteractable meroInteractable;
    bool onInteraction;
    [SerializeField] VRInteractionHandler interactionHandler;


    private void Awake()
    {
        //fishMov=GetComponent<FishPathFollowing>();
        outline = GetComponent<Outlinable>();
        coll = GetComponent<Collider>();
        meroInteractable = GetComponent<XRSimpleInteractable>();
    }
    void Start()
    {
        if (meroInteractable != null && interactionHandler != null)
        {

            interactionHandler.AddInteractable(meroInteractable);

            // Suscribirse al evento de interacción
            interactionHandler.OnInteractionStarted += StartInteraction;
        }
        else
        {
            Debug.LogError("Error: Falta alguna referencia en CrabInteractions.");
        }
        StartCoroutine(MeroInteractionArrive());
    }

    IEnumerator MeroInteractionArrive()
    {
        //poner audio mero intro==================
        yield return new WaitForSeconds(delayInteraction);
        InteractionState(true);
    }


    void MeroInteraction()
    {
        onInteraction = true;
        InteractionState(false);
        meroCard.SetActive(true);
        //sonido de mero
        FullCoralFlow.Instance.audioMana.PlayOneShot(FmodEvents.instance.Sound9);
    }
    public void EndInteraction()
    {
        GuidePath.Instance.LoraPath();
        meroCard.GetComponent<GrowObject>().Shrink();
        FullCoralFlow.Instance.AppearLora();
        //fishMov.loop = true;
    }

    void InteractionState(bool state)
    {
        outline.enabled = state;
        coll.enabled = state;
        meroInteractable.enabled = state;
    }

    void StartInteraction(XRSimpleInteractable interactable)
    {
        if (interactable == meroInteractable && !onInteraction)
        {
            MeroInteraction();
        }
    }

    private void OnDestroy()
    {
        interactionHandler.OnInteractionStarted -= StartInteraction;
    }

    //testing

    private void OnDisable()
    {
        if (!onInteraction)
        {
            MeroInteraction();
        }
    }

    private void OnEnable()
    {
        if (onInteraction)
        {
            EndInteraction();
        }
    }
}
