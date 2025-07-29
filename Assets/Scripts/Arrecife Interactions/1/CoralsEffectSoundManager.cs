using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using EPOOutline;

public class CoralsEffectSoundManager : MonoBehaviour
{
    public static CoralsEffectSoundManager Instance { get; private set; }

    

    public List<GameObject> corals;
    public List<Outlinable> outlines;
    public List<XRSimpleInteractable> interactables;
    [SerializeField] VRInteractionHandler interactionHandler;

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
        foreach (GameObject coral in corals) {
            XRSimpleInteractable interactable = coral.GetComponent<XRSimpleInteractable>();
            if (interactable != null) {
                interactionHandler.AddInteractable(interactable);
            } else {
                Debug.LogWarning($"El objeto {coral.name} no tiene un componente XRSimpleInteractable.");
            }
            interactionHandler.OnInteractionStarted += coral.GetComponent<CoralEffectSoundController>().StartInteraction;
        }
    }


    public void ActivateInteractiveObject(int i , bool value)
    {
        outlines[i].enabled = value;
        interactables[i].enabled = value;
    }

    

    private void OnDestroy()
    {
        foreach (GameObject coral in corals) {

            interactionHandler.OnInteractionStarted -= coral.GetComponent<CoralEffectSoundController>().StartInteraction;
        }
    }
}
