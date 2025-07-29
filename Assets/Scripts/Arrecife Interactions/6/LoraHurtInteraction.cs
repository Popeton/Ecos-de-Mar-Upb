using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using EPOOutline;

public class LoraHurtInteraction : MonoBehaviour
{
    public static LoraHurtInteraction Instance { get; private set; }

    [Header("Lora Configuration")]
    [SerializeField] FishPathFollowing loraMovement;
    [SerializeField] SwitchFishPaths loraSwitch1, loraSwitch2, loraSwitch3;
    [SerializeField] GameObject plasticInLora;
    [SerializeField] float timeToArrive;
    [SerializeField] float timeToInteract=3f;
    [SerializeField] float timeBeforeEnd=8f;

    [Header("Plastic Configuration")]
    [SerializeField] CustomEvent trashEvent;
    [SerializeField] ParticleSystem microplastics;
    [SerializeField] MeshRenderer render;
    [SerializeField] MoveObjectDown movement;
    [SerializeField] Outlinable plasticOutline;
    [SerializeField] GameObject plasticCard;
    private XRSimpleInteractable bottleInteractable;
    [SerializeField] FloatingMovement floatMovement;

    [Header("Coral Configuration")]
    [SerializeField] Outlinable coralOutline;
    [SerializeField] XRSimpleInteractable coralInteractable;
    [SerializeField] Collider coralCollider;

    [Header("General Configuration")]
    [SerializeField] VRInteractionHandler interactionHandler;
    [SerializeField] AudioManager audioMana;
    [SerializeField] CustomEvent fadeOutGeneral;

    private bool onInteraction;

    private void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        bottleInteractable = GetComponent<XRSimpleInteractable>();
        //audioMana = FullCoralFlow.Instance.audioMana;
        if (bottleInteractable != null && interactionHandler != null) {
            // Añadir la interacción con el cangrejo al VRInteractionHandler
            interactionHandler.AddInteractable(bottleInteractable);

            // Suscribirse al evento de interacción
            interactionHandler.OnInteractionStarted += StartInteraction;
        } else {
            Debug.LogError("Error: Falta alguna referencia en CrabInteractions.");
        }
        StartCoroutine(WhenArrived());
    }

    IEnumerator WhenArrived()
    {
        //path del guia
        GuidePath.Instance.InteractionWhiteCorals();
        //audio de lora lastimada
        audioMana.CleanUp();
        ArrecifeSubtitles.Instance.SingleSubtitle(31, 8.2f);
        audioMana.InitializeVoice(FmodEvents.instance.Interaccion5_Fase3, audioMana.transform.position);
        //sonido de blanqueamiento
        audioMana.StopAllOneShots();
        audioMana.PlayOneShot(FmodEvents.instance.Ambient);
        //blanqueamiento sonido
        audioMana.PlayOneShot(FmodEvents.instance.Sound11);
        //musica triste
        audioMana.PlayOneShot(FmodEvents.instance.Sound13);
        //AudioTesting.Instance.InitializeClip(11);
        //desaparecen los corales
        fadeOutGeneral.FireEvent();

        yield return new WaitForSeconds(timeToArrive);
        //habilita coral blanqueado
        coralOutline.enabled = true;
        coralInteractable.enabled = true;
        coralCollider.enabled = true;
        
    }


    public void AppearBottle()
    {
        StartCoroutine(AppearPlastic());
    }

    IEnumerator AppearPlastic()
    {
        trashEvent.FireEvent();
        loraSwitch1.SwitchPath();
        render.enabled = true;
        movement.MoveDown();
        //aparece el sonido de la botella
        audioMana.PlayOneShot(FmodEvents.instance.Sound6);
        yield return new WaitForSeconds(timeToInteract);
        
        floatMovement.enabled = true;
        bottleInteractable.enabled = true;
        plasticOutline.enabled = true;
    }

    void StartInteraction(XRSimpleInteractable interactable)
    {
        if (interactable == bottleInteractable && !onInteraction)
            StartCoroutine(StartMicroplastics());
    }


    IEnumerator StartMicroplastics()
    {
        //audio de los microplasticos
        audioMana.CleanUp();
        ArrecifeSubtitles.Instance.Microplastic();
        audioMana.InitializeVoice(FmodEvents.instance.Interaccion5_Fase4, audioMana.transform.position);
        //AudioTesting.Instance.InitializeClip(12);
        onInteraction = true;
        plasticOutline.enabled = false;
        bottleInteractable.enabled = false;
        render.enabled = false;
        microplastics.Play();
        //sonido de microplasticos
        audioMana.PlayOneShot(FmodEvents.instance.Sound15);
        yield return new WaitForSeconds(0.5f);
        // Suavizar la reducción de playbackSpeed
        yield return StartCoroutine(SmoothPlaybackSpeed(microplastics, 0.01f, 1f));

        yield return new WaitForSeconds(7f);
        plasticInLora.SetActive(true);
        //sonido del corazon de la lora
        audioMana.PlayOneShot(FmodEvents.instance.Sound7);
        loraMovement.loop = true;
        loraSwitch2.SwitchPath();
        plasticCard.SetActive(true);
        //aca empieza el fin
    }

    public void RemovePlasticCard()
    {
        plasticInLora.SetActive(false);
        plasticCard.GetComponent<GrowObject>().Shrink();
        FullCoralFlow.Instance.EndInteraction();
        loraSwitch3.SwitchPath();
        loraMovement.loop = false;
    }

    IEnumerator SmoothPlaybackSpeed(ParticleSystem ps, float targetSpeed, float duration)
    {
        float startSpeed = ps.playbackSpeed;
        float elapsedTime = 0f;

        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            ps.playbackSpeed = Mathf.Lerp(startSpeed, targetSpeed, elapsedTime / duration);
            yield return null; // Esperar al siguiente frame
        }

        ps.playbackSpeed = targetSpeed; // Asegurar que llegue al valor exacto
    }

    

    //testing
    private void OnDisable()
    {
        if (!onInteraction) {
            StartCoroutine(StartMicroplastics());
        }
    }
}
