using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using EPOOutline;
public class FliyingSpore : MonoBehaviour
{

    [SerializeField] GameObject[] spores;
    [SerializeField] GameObject sporeCollection,video;
    [SerializeField] ParticleSystem effect1, effect2;
    

    [SerializeField] float delayButton=13f;
    [SerializeField] GameObject button,replayButton;

    private XRSimpleInteractable sporeInteractable;
    [SerializeField] VRInteractionHandler interactionHandler;
    private bool onInteraction;
 

    private AudioManager audioMana;

    Outlinable outline;

    // Start is called before the first frame update
    private void Awake()
    {
        outline = GetComponent<Outlinable>();
        sporeInteractable = GetComponent<XRSimpleInteractable>();
    }


    void Start()
    {
        //audioMana = FullCoralFlow.Instance.audioMana;
        if (sporeInteractable != null && interactionHandler != null) {
            // Añadir la interacción con el cangrejo al VRInteractionHandler
            interactionHandler.AddInteractable(sporeInteractable);

            // Suscribirse al evento de interacción
            interactionHandler.OnInteractionStarted += StartInteraction;
        } else {
            Debug.LogError("Error: Falta alguna referencia en CrabInteractions.");
        }
        audioMana = FullCoralFlow.Instance.audioMana;
        //ajustar efecto
        //StartCoroutine(ActivateAndDeactivateGlow());
    }

    void StartInteraction(XRSimpleInteractable interactable)
    {
        if(interactable==sporeInteractable && !onInteraction) {
            StartCoroutine(FullInteraction());
        }
    }

    [System.Obsolete]
    IEnumerator FullInteraction()
    {
        //audio video esporas
        audioMana.CleanUp();
        ArrecifeSubtitles.Instance.EsporasVideo();
        audioMana.InitializeVoice(FmodEvents.instance.Interaccion2_Fase1, audioMana.transform.position);
        //inicia audio efecto de video
        audioMana.PlayOneShot(FmodEvents.instance.Sound2);
        //AudioTesting.Instance.InitializeClip(5);
        GuidePath.Instance.VideoSpores();
        video.SetActive(true);
        outline.enabled = false;
        sporeInteractable.enabled = false;
        onInteraction = true;
        effect1.loop = false;
        effect2.loop = false;
        yield return new WaitForSeconds(delayButton);
        button.SetActive(true);
        replayButton.SetActive(true);
        Destroy(sporeCollection);
    }
    


    //testeo
    private void OnDisable()
    {
        if (!onInteraction) {
            StartCoroutine(FullInteraction());
        }
    }

    private void OnDestroy()
    {
        
        interactionHandler.OnInteractionStarted -= StartInteraction;
    }
}
