using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using Unity.VRTemplate;

public class LogoStartCorals : MonoBehaviour
{
    XRSimpleInteractable logoInteractable;
    [SerializeField] float delay=3f;
    [SerializeField] VRInteractionHandler interactionHandler;

    Rotator rot;
    Animator anim;
    bool onInteraction;
    Collider coll;
    SpriteRenderer rend;
    AudioManager audioMana;
    private void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
        rot = GetComponent<Rotator>();
        logoInteractable = GetComponent<XRSimpleInteractable>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        audioMana=FullCoralFlow.Instance.audioMana;
        if (logoInteractable != null && interactionHandler != null) {

            interactionHandler.AddInteractable(logoInteractable);

            // Suscribirse al evento de interacción
            interactionHandler.OnInteractionStarted += StartInteraction;
        } else {
            Debug.LogError("Error: Falta alguna referencia en CrabInteractions.");
        }
    }

    void StartInteraction(XRSimpleInteractable interactable)
    {
        if (interactable == logoInteractable && !onInteraction) {
            StartCoroutine(LogoInteraction());
        }
    }

    IEnumerator LogoInteraction()
    {
        onInteraction = true;
        anim.SetTrigger("Reverse");
        //sonido de Logo
        audioMana.PlayOneShot(FmodEvents.instance.Sound17);
        coll.enabled = false;
        logoInteractable.enabled = false;
        yield return new WaitForSeconds(delay);
        rot.enabled = false;
        rend.enabled = false;
        FullCoralFlow.Instance.StartAllExperience();
    }

    //testing

    private void OnDisable()
    {
        if (!onInteraction) {
            StartCoroutine(LogoInteraction());
        }
    }

}
