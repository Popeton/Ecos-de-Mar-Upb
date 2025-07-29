using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


public class LastInteractionController : MonoBehaviour
{
    [SerializeField] float delayAppear,nextAppearDelay;
    [Range(1, 2)]
    [SerializeField] int fish;
    private XRSimpleInteractable fishInteractable;
    [SerializeField] GameObject card;
    [SerializeField] Transform cardSpawn;
    GrowObject grow;
    bool onInteraction;
    AudioManager audioMana;
    private void Awake()
    {
        fishInteractable = GetComponent<XRSimpleInteractable>();
        grow = card.GetComponent<GrowObject>();
    }
    void Start()
    {
        audioMana = FullTwilightFlow.Instance.audiMana;
        if (fish == 1) {
            StartCoroutine(WhenArrive());
        }
    }

    IEnumerator WhenArrive()
    {
        //pasa el guia a la mantarraya
        GuidePathTwilight.Instance.StingRay();
        //yield return new WaitForSeconds(12.5f);
        //AudioTesting.Instance.InitializeClip(7);
        //yield return new WaitForSeconds(6f);
        yield return new WaitForSeconds(18.5f);//tiempo con fmod
        LastInteractionManager.Instance.ActivateInteractiveObject(fish-1, true);
    }

    public void StartInteraction(XRSimpleInteractable interactable)
    {
        if (interactable == fishInteractable && !onInteraction) {
            ActivateFishCard();
        }
    }

    void ActivateFishCard()
    {
        card.transform.position = cardSpawn.position;
        GuidePathTwilight.Instance.InfoCard();
        onInteraction = true;
        card.SetActive(true);
        LastInteractionManager.Instance.ActivateInteractiveObject(fish-1, false);
    }

    public void NextCard()
    {
        switch (fish) {
            case 1:
                grow.Shrink();
                //poner audio interaccion 5 medio
                audioMana.CleanUp();
                audioMana.InitializeVoice(FmodEvents.instance.Interaccion5_Fase2, FullTwilightFlow.Instance.guide.position);
                TwilightSubtitles.Instance.ThreeSubtitles(26, 27, 28, 3f, 3.52f, 2.7f);
                //AudioTesting.Instance.InitializeClip(8);
                //pasa el guia al tiburon
                GuidePathTwilight.Instance.Mako();
                StartCoroutine(AppearNextCard(1));
                break;
            case 2:
                StartCoroutine(SlowAnim());
                break;
        }
    }

    IEnumerator AppearNextCard(int i)
    {
        yield return new WaitForSeconds(nextAppearDelay);
        LastInteractionManager.Instance.ActivateInteractiveObject(i, true);
    }

    
    //testing


    //poner corrutina para ralentizar mordida

    IEnumerator SlowAnim()
    {
        LastInteractionManager.Instance.EndInteraction();
        grow.Shrink();
        //poner audio interaccion 5 final
        audioMana.CleanUp();
        audioMana.InitializeVoice(FmodEvents.instance.Interaccion6_Fase1, FullTwilightFlow.Instance.guide.position);
        TwilightSubtitles.Instance.End();
        //AudioTesting.Instance.InitializeClip(9);
        //pasa el guia al loop final
        GuidePathTwilight.Instance.End();
        LastInteractionManager.Instance.endPaths[1].fishPathFollowing.loop = false;
        GetComponent<Animator>().SetBool("atk", true);
        yield return new WaitForSeconds(4f);
        //GetComponent<Animator>().speed = 0.21106f;
    }
    public void PlaySoundShark()
    {
        //sonido de mordida
        audioMana.PlayOneShot(FmodEvents.instance.Sound13);
    }
    private void OnEnable()
    {
        if (onInteraction) {
            NextCard();
        }
    }

    private void OnDisable()
    {
        if (!onInteraction) {
            ActivateFishCard();
        }
    }

}
