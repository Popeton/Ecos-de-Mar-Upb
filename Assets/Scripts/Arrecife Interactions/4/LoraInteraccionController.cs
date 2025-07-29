using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using EPOOutline;

public class LoraInteraccionController : MonoBehaviour
{
   public static LoraInteraccionController Instance { get; private set; }

    [SerializeField] float interactionDelay = 4.5f;
    [SerializeField] float delayAppearCard;

    public float delayEatAudio;
    public float delayPSandAudio;
    [SerializeField] FishPathFollowing fishMov;
    [SerializeField] SwitchFishPaths[] switchPathRef;
    
    Outlinable outline;

    [SerializeField] GameObject loraInformation;

    [SerializeField] GrowObject loraShield;
    [SerializeField] ParticleSystem loraSand;
    
    

    [SerializeField] VRInteractionHandler interactionHandler;
    private XRSimpleInteractable loraInteractable;
    Collider coll;

    [SerializeField] AudioManager audioMana;

    int actualCard=0;

    private void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject); // Destruye la instancia duplicada
            return;
        }

        Instance = this;
        
        outline = GetComponent<Outlinable>();
        loraInteractable = GetComponent<XRSimpleInteractable>();
        coll = GetComponent<Collider>();
    }

    private void Start()
    {
        //audioMana = FullCoralFlow.Instance.audioMana;
        StartCoroutine(DelayGlowAndInteraction());
        if (loraInteractable != null && interactionHandler != null) {
            
            interactionHandler.AddInteractable(loraInteractable);

            // Suscribirse al evento de interacción
            interactionHandler.OnInteractionStarted += StartInteraction;
        } else {
            Debug.LogError("Error: Falta alguna referencia en CrabInteractions.");
        }
    }

    void StartInteraction(XRSimpleInteractable interactable)
    {
        if(interactable==loraInteractable && actualCard<=2) {
            StartCoroutine(LoraInteraction());
        }
    }


    IEnumerator LoraInteraction()
    {
        ActivateInteraction(false);
        switch (actualCard) {
            case 0:
                fishMov.loop = false;
                //path del guia
                GuidePath.Instance.LoraNight();
                //va el audio de la lora de noche
                audioMana.CleanUp();
                ArrecifeSubtitles.Instance.SingleSubtitle(23, 4.5f);
                audioMana.InitializeVoice(FmodEvents.instance.Interaccion3_Fase1, audioMana.transform.position);
                LoraNight();
                //AudioTesting.Instance.InitializeClip(7);
                yield return new WaitForSeconds(delayAppearCard);
                loraInformation.SetActive(true);
                actualCard++;
                break;
            case 1:
                SwitchInformation.Instance.AppearOtherCard();
                actualCard++;
                //sonido de lora comiendo
                audioMana.PlayOneShot(FmodEvents.instance.Sound8);
                break;
            case 2:
                SwitchInformation.Instance.AppearOtherCard();
                break;
        }

        
    }

    public void ActivateInteraction(bool state)
    {
        coll.enabled = state;
        outline.enabled = state;
        loraInteractable.enabled = state;
    }

    public void LoraNight()
    {
        StartCoroutine(Night());
    }

    IEnumerator Night()
    {
        switchPathRef[0].SwitchPath();
        yield return new WaitForSeconds(2f);
        loraShield.Grow();
    }

    public void LoraEat()
    {
        loraShield.Shrink();
        switchPathRef[1].SwitchPath();
        
    }

    public void LoraPoop()
    {
        loraSand.Play();
        switchPathRef[2].fishPathFollowing.loop = true;
        switchPathRef[2].SwitchPath();
    }

    public void EndInteraction()
    {
        loraSand.Stop();
        switchPathRef[3].fishPathFollowing.loop = false;
        switchPathRef[3].SwitchPath();
        FullCoralFlow.Instance.WebInteractionAppear();
    }
    
    
    private void OnDestroy()
    {
        interactionHandler.OnInteractionStarted -= StartInteraction;
    }
    IEnumerator DelayGlowAndInteraction()
    {
        yield return new WaitForSeconds(interactionDelay);
        ActivateInteraction(true);
    }
    //testing
    private void OnDisable()
    {
        if (actualCard <= 2) {
            StartCoroutine(LoraInteraction());
        }
    }
}
