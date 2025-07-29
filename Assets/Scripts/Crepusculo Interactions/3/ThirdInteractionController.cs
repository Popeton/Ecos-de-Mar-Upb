using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ThirdInteractionController : MonoBehaviour
{
    [SerializeField] SwitchFishPaths endPath;
    [SerializeField] GameObject card;
    [SerializeField] GameObject brightFish;
    [SerializeField] GameObject fishes;
    [SerializeField] Transform cardSpawn;
    [Range(1,2)]
    [SerializeField] int fish;
    [SerializeField] float delayArrive;
    private XRSimpleInteractable fishInteractable;
    bool onInteraction;
    AudioManager audioMana;
    private void Awake()
    {
        fishInteractable = GetComponent<XRSimpleInteractable>();
    }

    void Start()
    {
        audioMana = FullTwilightFlow.Instance.audiMana;
        StartCoroutine(WhenArrive());
    }
    IEnumerator WhenArrive()
    {
        yield return new WaitForSeconds(delayArrive);
        ThirdInteractionManager.Instance.ActivateObject(fish-1, true);
    }
    public void StartInteraction(XRSimpleInteractable interactable)
    {
        if (interactable == fishInteractable && !onInteraction) {
            AppearCard();
        }
    }

    void AppearCard()
    {
        onInteraction = true;
        card.transform.position = cardSpawn.position;
        card.SetActive(true);
        GuidePathTwilight.Instance.InfoCard();
        ThirdInteractionManager.Instance.ActivateObject(fish - 1, false);
    }

    public void FishCloseCard()
    {
        //aca podria ir el tiempo de lo que dura la secuencia de las medusas
        fishes.SetActive(true);
        switch (fish) {
            case 1:
                card.GetComponent<GrowObject>().Shrink();
                endPath.SwitchPath();
                brightFish.SetActive(true);
                FullTwilightFlow.Instance.ActivateInteraction(2, 0f, false);
                //audio linternilla
                FullTwilightFlow.Instance.audiMana.PlayOneShot(FmodEvents.instance.Sound3);
                //sonido de nado de ballena
                FullTwilightFlow.Instance.audiMana.PlayOneShot(FmodEvents.instance.Sound6);
                //poner audio interaccion 3 final
                audioMana.CleanUp();
                audioMana.InitializeVoice(FmodEvents.instance.Interaccion2_Fase2, FullTwilightFlow.Instance.guide.position);
                TwilightSubtitles.Instance.ThreeSubtitles(16, 17, 18, 3.6f, 4.7f, 4.6f);
                //AudioTesting.Instance.InitializeClip(4);
                //cambia al brillante
                GuidePathTwilight.Instance.BrigthFishCard();
                break;
            case 2:
                card.GetComponent<GrowObject>().Shrink();
                endPath.SwitchPath();
                //cambia a medusas
                GuidePathTwilight.Instance.JellyfishToFinal();
                //poner audio medusas
                audioMana.CleanUp();
                audioMana.InitializeVoice(FmodEvents.instance.Interaccion3_Fase1, FullTwilightFlow.Instance.guide.position);
                TwilightSubtitles.Instance.ThreeSubtitles(19, 20, 21, 4.3f, 5.3f, 4f);
                //AudioTesting.Instance.InitializeClip(5);
                break;
        }
    }
    
    //testing
    private void OnEnable()
    {
        if (onInteraction) {
            FishCloseCard();
        }
    }
    private void OnDisable()
    {
        if (!onInteraction) {
            AppearCard();
        }
    }
}
