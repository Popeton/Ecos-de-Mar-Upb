using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class LogoInteraction : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private VRInteractionHandler interactionHandler;
    [SerializeField] private Animator logoAnimator;
    [SerializeField] private GameObject guideObject;
    [SerializeField] private float startDelay = 4f;
    [SerializeField] private MangroveInteraction mangrove;

    [Header("Configuración")]
    [SerializeField] private string sceneName;

    private bool hasInteracted = false;
    private PathMover pathMover;

    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;

        // Buscar el VRInteractionHandler si no está asignado en el inspector
        if (interactionHandler == null)
        {
            //interactionHandler = VRInteractionHandler.Instance;
            if (interactionHandler == null)
            {
                Debug.LogError("No se encontró un VRInteractionHandler en la escena.");
                return;
            }
        }

        // Configurar interacción
        XRSimpleInteractable interactable = GetComponentInChildren<XRSimpleInteractable>();
        if (interactable != null)
        {
            interactionHandler.AddInteractable(interactable);
            interactionHandler.OnInteractionStarted += HandleLogoInteraction;
        }
        else
        {
            Debug.LogWarning("No se encontró XRSimpleInteractable en los hijos del logo.");
        }

        if (guideObject != null)
        {
            pathMover = guideObject.GetComponent<PathMover>();
            guideObject.SetActive(false);
        }
    }

    private void HandleLogoInteraction(XRSimpleInteractable interactable)
    {
        if (hasInteracted) return;

        logoAnimator.enabled = true;
        GetComponent<Collider>().enabled = false;
        interactable.enabled = false;

        logoAnimator.SetBool("Active", true);
        ActivateFirstMangrove();

        Invoke("StartGuideExperience", startDelay);
        hasInteracted = true;
    }

    private void ActivateFirstMangrove()
    {
        if (mangrove != null)
        {
            mangrove.enabled = true;
            mangrove.SetManglarState(mangrove.GetManglarByIndex(0), true);
        }
    }

    private void StartGuideExperience()
    {
        if (guideObject != null && pathMover != null)
        {
            guideObject.SetActive(true);
            pathMover.StartExperience();
            AudioManager.instance?.PlayZoneAudio(sceneName);

            GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            Debug.LogError("Referencias de la guía no asignadas!");
        }
    }

    private void OnDestroy()
    {
        if (interactionHandler != null)
        {
            interactionHandler.OnInteractionStarted -= HandleLogoInteraction;
        }
    }

    public void StarReverseAnimation()
    {
        logoAnimator.SetTrigger("Reverse");
    }
}
