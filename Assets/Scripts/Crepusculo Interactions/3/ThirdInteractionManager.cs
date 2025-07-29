using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using EPOOutline;

public class ThirdInteractionManager : MonoBehaviour
{
    public static ThirdInteractionManager Instance { get; private set; }

    public List<GameObject> fishes;
    public List<Outlinable> outlines;
    public List<XRSimpleInteractable> interactables;
    public List<Collider> colliders;
    [SerializeField] VRInteractionHandler interactionHandler;

    private void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //sonido cachalote
        FullTwilightFlow.Instance.audiMana.PlayOneShot(FmodEvents.instance.Sound16);
        //sonido de nado de ballena
        FullTwilightFlow.Instance.audiMana.PlayOneShot(FmodEvents.instance.Sound6);

        foreach (GameObject fish in fishes) {
            XRSimpleInteractable interactable = fish.GetComponent<XRSimpleInteractable>();
            if (interactable != null) {
                interactionHandler.AddInteractable(interactable);
            } else {
                Debug.LogWarning($"El objeto {fish.name} no tiene un componente XRSimpleInteractable.");
            }
            interactionHandler.OnInteractionStarted += fish.GetComponent<ThirdInteractionController>().StartInteraction;
        }
        //cambia al cachalote
        GuidePathTwilight.Instance.CachaloteCard();
    }

    public void ActivateObject(int i, bool value)
    {
        outlines[i].enabled = value;
        interactables[i].enabled = value;
        colliders[i].enabled = value;
    }

    private void OnDestroy()
    {
        foreach (GameObject fish in fishes) {

            interactionHandler.OnInteractionStarted -= fish.GetComponent<ThirdInteractionController>().StartInteraction;
        }
    }
}
