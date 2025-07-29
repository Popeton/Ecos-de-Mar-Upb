using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using EPOOutline;

public class JellyFishManager : MonoBehaviour
{
    public static JellyFishManager Instance { get; private set; }

    public List<GameObject> fishes;
    //public List<Outlinable> outlines;
    public List<XRSimpleInteractable> interactables;
    public List<Collider> colliders;
    [SerializeField] VRInteractionHandler interactionHandler;
    [SerializeField] CustomEvent fadeLinternillaEvent;
    [SerializeField] CustomEvent byeJelly;
    int cardAppear;
    bool onInteraction;
    AudioManager audioMana;
    
    [SerializeField] GameObject card;
    [SerializeField] Transform spawnPos;

    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        audioMana = FullTwilightFlow.Instance.audiMana;
        audioMana.PlayOneShot(FmodEvents.instance.Sound5);
        foreach (GameObject fish in fishes)
        {
            XRSimpleInteractable interactable = fish.GetComponent<XRSimpleInteractable>();
            if (interactable != null)
            {
                interactionHandler.AddInteractable(interactable);
            }
            else
            {
                Debug.LogWarning($"El objeto {fish.name} no tiene un componente XRSimpleInteractable.");
            }
            interactionHandler.OnInteractionStarted += fish.GetComponent<JellyFishInteractionController>().StartInteraction;
        }
        fadeLinternillaEvent.FireEvent();
    }

    public void AppearCard()
    {
        onInteraction = true;
        if (cardAppear == 0) {
            GuidePathTwilight.Instance.InfoCard();
            card.transform.position = spawnPos.position;
            card.SetActive(true);
            cardAppear++;
        }
    }

    public void AppearLast()
    {
        //poner audio interaccion 5 inicio
        audioMana.CleanUp();
        audioMana.InitializeVoice(FmodEvents.instance.Interaccion5_Fase1, FullTwilightFlow.Instance.guide.position);
        TwilightSubtitles.Instance.FourSubtitles(22, 23, 24, 25, 5.7f, 3.5f, 5.6f, 4f);
        //AudioTesting.Instance.InitializeClip(6);
        FullTwilightFlow.Instance.ActivateInteraction(5, 0f, true);
        card.GetComponent<GrowObject>().Shrink();
        onInteraction = false;
        byeJelly.FireEvent();
    }
    private void OnDestroy()
    {
        foreach (GameObject fish in fishes)
        {

            interactionHandler.OnInteractionStarted -= fish.GetComponent<JellyFishInteractionController>().StartInteraction;
        }
    }


    //testing

    private void OnDisable()
    {
        if (onInteraction) {
            AppearLast();
        }
    }
}
