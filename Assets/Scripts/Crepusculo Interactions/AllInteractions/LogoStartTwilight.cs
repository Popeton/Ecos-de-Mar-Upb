using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using Unity.VRTemplate;

public class LogoStartTwilight : MonoBehaviour
{
    XRSimpleInteractable logoInteractable;
    [SerializeField] float delay = 3f;
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
        if (logoInteractable != null && interactionHandler != null) {

            interactionHandler.AddInteractable(logoInteractable);

            // Suscribirse al evento de interacción
            interactionHandler.OnInteractionStarted += StartInteraction;
        } else {
            Debug.LogError("Error: Falta alguna referencia en CrabInteractions.");
        }
        audioMana = FullTwilightFlow.Instance.audiMana;
    }

    void StartInteraction(XRSimpleInteractable interactable)
    {
        if (interactable == logoInteractable && !onInteraction) {
            StartCoroutine(LogoInteraction());
        }
    }

    IEnumerator LogoInteraction()
    {
        //sonido de Logo
        audioMana.PlayOneShot(FmodEvents.instance.Sound17);
        onInteraction = true;
        anim.SetTrigger("Reverse");
        coll.enabled = false;
        logoInteractable.enabled = false;
        yield return new WaitForSeconds(delay);
        rot.enabled = false;
        rend.enabled = false;
        //poner audio intro 
        audioMana.CleanUp();
        audioMana.InitializeVoice(FmodEvents.instance.Voice, FullTwilightFlow.Instance.guide.position);
        TwilightSubtitles.Instance.Intro();

        //empieza el guia
        GuidePathTwilight.Instance.StartExperience();
        yield return new WaitForSeconds(delay);
        //AudioTesting.Instance.InitializeClip(0);
        FullTwilightFlow.Instance.ActivateInteraction(1, 0f, true);

    }

    //testing

    private void OnDisable()
    {
        if (!onInteraction) {
            StartCoroutine(LogoInteraction());
        }
    }

}
