using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using EPOOutline;

public class PlanctonInteractionController : MonoBehaviour
{
    [Header("Start Configuration")]

    [SerializeField] GameObject Plancton;
    [SerializeField] float planctonDelay;
    [SerializeField] VRInteractionHandler interactionHandler;

    XRSimpleInteractable planctonInteractable;
    
    Outlinable outline;
    
    Collider coll;
    bool onInteraction;
    AudioManager audioMana;
    [Header("Interaction Configuration")]

    [SerializeField] GameObject card;
    [SerializeField] GameObject video;
    [SerializeField] GameObject continueButton;

    [Header("End Configuration")]

    [SerializeField] GameObject endPlancton;

    //testing
    int i = 0;

    private void Awake()
    {
        outline = GetComponent<Outlinable>();
        planctonInteractable = GetComponent<XRSimpleInteractable>();
        
        coll = GetComponent<Collider>();
    }

    void Start()
    {
        if (planctonInteractable != null && interactionHandler != null) {

            interactionHandler.AddInteractable(planctonInteractable);

            // Suscribirse al evento de interacción
            interactionHandler.OnInteractionStarted += StartInteraction;
        } else {
            Debug.LogError("Error: Falta alguna referencia en CrabInteractions.");
        }
        audioMana = FullTwilightFlow.Instance.audiMana;
        StartCoroutine(PlanctonArrive());
        //mover guia a plancton
        GuidePathTwilight.Instance.PlanctonInteraction();
    }

    IEnumerator PlanctonArrive()
    {
        //poner audio inicio´plancton
        audioMana.CleanUp();
        audioMana.InitializeVoice(FmodEvents.instance.Interaccion1_Fase1, FullTwilightFlow.Instance.guide.position);
        TwilightSubtitles.Instance.ThreeSubtitles(6, 7, 8,6.2f,2.4f,4f);
        //AudioTesting.Instance.InitializeClip(1);
        Plancton.SetActive(true);
        yield return new WaitForSeconds(planctonDelay);
        FullTwilightFlow.Instance.ActivateInteraction(1, 2f, false);
        
        outline.enabled = true;
        planctonInteractable.enabled = true;
    }

    void StartInteraction(XRSimpleInteractable interactable)
    {
        if (interactable == planctonInteractable && !onInteraction) {
            PlanctonInteraction();
        }
    }

    void PlanctonInteraction()
    {
        onInteraction = true;
       
        outline.enabled = false;
        planctonInteractable.enabled = false;
        coll.enabled = false;
        card.SetActive(true);
    }

    public void Video()
    {
        StartCoroutine(ChangeToVideo());
    }
    IEnumerator ChangeToVideo()
    {
        card.GetComponent<GrowObject>().Shrink();
        video.SetActive(true);
        //poner audio video de plancton
        audioMana.CleanUp();
        FullTwilightFlow.Instance.audiMana.PlayOneShot(FmodEvents.instance.Sound15);
        audioMana.InitializeVoice(FmodEvents.instance.Interaccion1_Fase2, FullTwilightFlow.Instance.guide.position);
        TwilightSubtitles.Instance.ThreeSubtitles(9, 10, 11, 5f, 3.75f, 3.4f);
        //AudioTesting.Instance.InitializeClip(2);
        yield return new WaitForSeconds(12.5f);
        continueButton.SetActive(true);
        Plancton.SetActive(false);
    }

    public void EndInteraction()
    {
        video.GetComponent<GrowObject>().Shrink();
        //aca sigue la interaccion 3
        //poner audio inicio interaccion 3
        audioMana.CleanUp();
        audioMana.InitializeVoice(FmodEvents.instance.Interaccion2_Fase1, FullTwilightFlow.Instance.guide.position);
        TwilightSubtitles.Instance.FourSubtitles(12, 13, 14,15, 6.2f, 3.1f, 3.5f,5.5f);
        //AudioTesting.Instance.InitializeClip(3);
        FullTwilightFlow.Instance.ActivateInteraction(3, 0f,true);
        endPlancton.SetActive(true);
    }

    //testing
    private void OnEnable()
    {
        if (onInteraction) {
            switch (i) {
                case 0:
                    i++;
                    Video();
                    break;
                case 1:
                    EndInteraction();
                    break;
            }
        }
    }
    private void OnDisable()
    {
        if (!onInteraction) {
            PlanctonInteraction();
        }
    }
}
