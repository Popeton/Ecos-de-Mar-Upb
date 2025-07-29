using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FullCoralFlow : MonoBehaviour
{
    public static FullCoralFlow Instance { get; private set; }

    [Header("Configurations")]

    public AudioManager audioMana;
    

    [Header("Corals Interactions")]

    [SerializeField] private int coralInteractionDone;
    [SerializeField] GameObject otherSpores;
    [SerializeField] CustomEvent desappearSpores;

    [Header("Seeds Interactions")]

    [SerializeField] private GameObject seedsInteractions;
    [SerializeField] CustomEvent turnDownLight;

    [Header("Fishes Interactions")]

    [SerializeField] CustomEvent fishesInteraction;
    [SerializeField] CustomEvent turnUpLight;
    [SerializeField] private float delayAppearMero,delayAppearLora;
    [SerializeField] MeroInteractionController meroInteraccion;
    [SerializeField] private GameObject loraInteractions;
    [SerializeField] private float fullLoraTime;
    [SerializeField] GameObject otherFishes;

    [Header("Web Interactions")]

    [SerializeField] private GameObject webInteraction;
    [SerializeField] CustomEvent removeFishEvent;

    [Header("Hurt Lora Interaction")]
    [SerializeField] GameObject hurtInteraction;

    private void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    private void Start()
    {
        //empieza el sonido del ambiente
        audioMana.PlayOneShot(FmodEvents.instance.Ambient);
        audioMana.PlayOneShot(FmodEvents.instance.Sound10);
    }
    public void StartAllExperience()
    {
        StartCoroutine(BeginExperience());
        ResetAllVariables();
    }

    void ResetAllVariables()
    {
        coralInteractionDone = 0;
        seedsInteractions.SetActive(false);
        //fishesInteraction.SetActive(false);
        loraInteractions.SetActive(false);
        webInteraction.SetActive(false);
        hurtInteraction.SetActive(false);
    }

    

    IEnumerator BeginExperience()
    {
        //path del guia
        GuidePath.Instance.InteractionCorals();
        //aca puede ir el audio 1 intro
        audioMana.CleanUp();
        ArrecifeSubtitles.Instance.Intro();
        audioMana.InitializeVoice(FmodEvents.instance.Voice, audioMana.transform.position);
        
        
        //AudioTesting.Instance.InitializeClip(0);

        yield return new WaitForSeconds(20f);
        //reproduce el audio de cerebro
        audioMana.CleanUp();
        audioMana.InitializeVoice(FmodEvents.instance.Interaccion1_Fase1, audioMana.transform.position);
        //AudioTesting.Instance.InitializeClip(1);

        yield return new WaitForSeconds(4f);
        CoralsEffectSoundManager.Instance.ActivateInteractiveObject(0,true);
    }

    //sumatoria de la interaccion de los corales
    public void CoralInteractionSum()
    {
        coralInteractionDone++;
        if (coralInteractionDone == 3) {
            
            //path del guia
            GuidePath.Instance.InteractionSpores();
            //aca va el audio 2 de las esporas inicio
            audioMana.CleanUp();
            ArrecifeSubtitles.Instance.EsporasIntro();
            audioMana.InitializeVoice(FmodEvents.instance.Interaccion1_Fase4, audioMana.transform.position);
            //apagar luz
            turnDownLight.FireEvent();
            DistortionController.Instance.TriggerDistortionChange();
            //AudioTesting.Instance.InitializeClip(4);
            StartCoroutine(EsporasDelay());
        }
    }

    IEnumerator EsporasDelay()
    {
        yield return new WaitForSeconds(11f);
        audioMana.PlayOneShot(FmodEvents.instance.Sound1);
        otherSpores.SetActive(true);
        yield return new WaitForSeconds(7f);
        seedsInteractions.SetActive(true);
    }

    //sumatoria de la interaccion de las semillas de los corales
    public void SeedInteraction()
    {
        //path del guia
        GuidePath.Instance.InteractionFishesAndLora();
        //aca va el audio 3 de interaccion de los peces
        audioMana.CleanUp();
        ArrecifeSubtitles.Instance.IntroPeces();
        //sonido de coral creciendo
        //audioMana.PlayOneShot(FmodEvents.instance.Sound16);
        audioMana.InitializeVoice(FmodEvents.instance.Interaccion2_Fase2, audioMana.transform.position);
        //AudioTesting.Instance.InitializeClip(6);
        StartCoroutine(AppearFishes());
    }

    IEnumerator AppearFishes()
    {
        //desaparecen esporas
        desappearSpores.FireEvent();
        //se acalara el cielo
        DistortionController.Instance.ResetDistortionProperties();
        //subir luces
        turnUpLight.FireEvent();
        yield return new WaitForSeconds(6f);
        fishesInteraction.FireEvent();
        yield return new WaitForSeconds(delayAppearMero);
        //sonido de peces pasando
        audioMana.PlayOneShot(FmodEvents.instance.Sound5);
        meroInteraccion.enabled=true;
    }

    public void AppearLora()
    {
        //intro lora
        audioMana.CleanUp();
        audioMana.InitializeVoice(FmodEvents.instance.Interaccion6_Fase1, audioMana.transform.position);
        //subtitulos de la lora
        ArrecifeSubtitles.Instance.SingleSubtitle(22, 3.3f);
        loraInteractions.SetActive(true);
    }

    //aparicion de la red y el tiempo total de la interaccion de la lora
    public void WebInteractionAppear()
    {
        //path del guia
        GuidePath.Instance.InteractionWeb();
        //va el audio de las redes
        audioMana.CleanUp();
        ArrecifeSubtitles.Instance.Webs();
        audioMana.InitializeVoice(FmodEvents.instance.Interaccion5_Fase2, audioMana.transform.position);
        removeFishEvent.FireEvent();
        //AudioTesting.Instance.InitializeClip(10);
        
        StartCoroutine(TransitionLoraToWeb());
    }

    IEnumerator TransitionLoraToWeb()
    {
        audioMana.StopAllOneShots();
        //musica triste y nuevo sonido ambiente sin peces
        audioMana.PlayOneShot(FmodEvents.instance.Sound14);
        audioMana.PlayOneShot(FmodEvents.instance.Sound13);
        yield return new WaitForSeconds(fullLoraTime);
        //peces nadando
        audioMana.PlayOneShot(FmodEvents.instance.Sound5);
        webInteraction.SetActive(true);
        //sonido de lancha
        audioMana.PlayOneShot(FmodEvents.instance.Sound3);
        //sonido de red cayendo
        audioMana.PlayOneShot(FmodEvents.instance.Sound4);
        yield return new WaitForSeconds(1.5f);
        audioMana.PlayOneShot(FmodEvents.instance.Sound4);
        yield return new WaitForSeconds(2.5f);
        loraInteractions.SetActive(false);
    }

    public void HurtLoraInteraction()
    {
        hurtInteraction.SetActive(true);
        otherFishes.SetActive(false);
    }

    public void EndInteraction()
    {
        //transicion de arrecife a home
        Initiate.Fade("Home", Color.black, 29f);
        //path final guia
        GuidePath.Instance.EndInteraction();
        //va el audio del final
        //audioMana.CleanUp();
        ArrecifeSubtitles.Instance.End();
        //musica final
        audioMana.PlayOneShot(FmodEvents.instance.Sound12);
        audioMana.InitializeVoice(FmodEvents.instance.Interaccion7_Fase1, audioMana.transform.position);
        //AudioTesting.Instance.InitializeClip(13);
    }


    //testeo
    private void OnDisable()
    {
        CoralInteractionSum();
    }
}
