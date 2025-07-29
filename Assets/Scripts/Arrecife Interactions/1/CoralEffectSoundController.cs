using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CoralEffectSoundController : MonoBehaviour
{
    
    public GameObject interfaz;
    private bool isRunning;

    [SerializeField] float delayAudio;

    [Range(1, 3)]
    [SerializeField] int audio;

    private XRSimpleInteractable coralInteractable;
    
    private Collider collider;

    [SerializeField] GrowObject card;
    [SerializeField] AudioManager audioMana;

    private void Awake()
    {
        collider = GetComponent<Collider>();
        coralInteractable = GetComponent<XRSimpleInteractable>();
        interfaz.SetActive(false);
    }
    private void Start()
    {
        //audioMana = FullCoralFlow.Instance.audioMana;
    }
    public void StartInteraction(XRSimpleInteractable interactable)
    {
        if (interactable == coralInteractable && !isRunning) {
            SoundAndEffect();
        }
    }

    void SoundAndEffect()
    {
        CoralsEffectSoundManager.Instance.ActivateInteractiveObject(audio-1, false);
        collider.enabled = false;
        isRunning = true;
        interfaz.SetActive(true);    
    }
   
    public void InterfazSum()
    {
        StartCoroutine(DelayInteraction());
    }

    IEnumerator DelayInteraction()
    {
        FullCoralFlow.Instance.CoralInteractionSum();
        switch (audio) {
            case 1:
                //reproduce el audio de ramo
                ArrecifeSubtitles.Instance.SingleSubtitle(5,3f);
                audioMana.CleanUp();
                audioMana.InitializeVoice(FmodEvents.instance.Interaccion1_Fase2, audioMana.transform.position);
                //AudioTesting.Instance.InitializeClip(2);
                GuidePath.Instance.SecondCoral();
                card.Shrink();

                yield return new WaitForSeconds(delayAudio);
                
                CoralsEffectSoundManager.Instance.ActivateInteractiveObject(1, true);
                
                break;
            case 2:
                //reproduce el audio de montaña
                ArrecifeSubtitles.Instance.TercerCoral();
                audioMana.CleanUp();
                audioMana.InitializeVoice(FmodEvents.instance.Interaccion1_Fase3, audioMana.transform.position);
                //AudioTesting.Instance.InitializeClip(3);
                GuidePath.Instance.ThirdCoral();
                card.Shrink();

                yield return new WaitForSeconds(delayAudio);
                
                CoralsEffectSoundManager.Instance.ActivateInteractiveObject(2, true);
                break;
            case 3:
                card.Shrink();
                break;
        }
    }

    //testeo
    private void OnDisable()
    {
        if (!isRunning) {
            SoundAndEffect();
        }
    }

    private void OnEnable()
    {
        if (isRunning) {
            InterfazSum();
        }
    }

}
