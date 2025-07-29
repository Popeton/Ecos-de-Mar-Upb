using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuConfiguration : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private AudioManager audioInstance;
    [SerializeField] private RuneActivationSystem rune;
    [SerializeField] private PathMover GuiapathMover;
    [SerializeField] private CountdownSystem countdownSystem;
    [SerializeField] private ExecutionSettings executionSettings;

    [Header("UI Popups")]
    [SerializeField] private GameObject[] uiPopups;
   

    public static bool hasReturnedFromZone = false;

    void Start()
    {
        rune = GetComponent<RuneActivationSystem>();
        countdownSystem.gameObject.SetActive(false);
        ShowWelcomeCard();
    }

    // ============ STORE ============

    public void InitializeExperience()
    {
        GuiapathMover.gameObject.SetActive(true);
        GuiapathMover.StartExperience();
        StartCoroutine(WaitForAudioToFinish(24f));
        audioInstance.PlayZoneAudio("Home");
    }

    private void SkipToRuneSelection()
    {
        countdownSystem.gameObject.SetActive(false);
        GuiapathMover.gameObject.SetActive(true);
        GuiapathMover.StartExperience();

        Invoke(nameof(ShowRunesImmediately), 1f);
    }

    private void ShowRunesImmediately()
    {
        rune.RaiseAndEnableRequiredRunes();
       
        if (executionSettings.mode == ExecutionMode.Store)
            uiPopups[2].SetActive(true);
       
        hasReturnedFromZone = false;
        print("sortemode");
    }

    // ============ MUSEUM ============

    public void PreZonesAudio()
    {
        audioInstance.InitializeVoice(FmodEvents.instance.Interaccion1_Fase1, audioInstance.transform.position);
    }
    private void ShowWelcomeCard()
    {
        //Se muestra la ficha cuando: modo museum activado || modo store con submodo FullExperience || AllRunesActivated sin haber regresado de la zona.
        bool showWelcome =
        executionSettings.mode == ExecutionMode.Museum ||
        (executionSettings.mode == ExecutionMode.Store && ( executionSettings.storeSubMode == StoreSubMode.FullExperience || (executionSettings.storeSubMode == StoreSubMode.AllRunesActivated && !hasReturnedFromZone)
        ));

        if (showWelcome)
        {
            uiPopups[0].SetActive(true); // Mostrar ficha bienvenida
        }

        if (executionSettings.mode == ExecutionMode.Store)
        {
            if (executionSettings.storeSubMode == StoreSubMode.FullExperience ||
                (executionSettings.storeSubMode == StoreSubMode.AllRunesActivated && !hasReturnedFromZone))
            {
                countdownSystem.OnCountdownComplete.AddListener(InitializeExperience);
            }
            else if (executionSettings.storeSubMode == StoreSubMode.AllRunesActivated && hasReturnedFromZone)
            {
                Invoke(nameof(SkipToRuneSelection), 3f);
            }
        }
    }

    public void StartMuseumExperience()
    {
        uiPopups[0].SetActive(false);
        GuiapathMover.gameObject.SetActive(true);
        GuiapathMover.StartExperience();

        rune.StartStoreModeRunes(); // Inicia secuencia de activación con audio


    }
    public void ShowMuseumRuneSelection()
    {
        if (executionSettings.mode == ExecutionMode.Museum)
        {
            uiPopups[1].SetActive(true);
        }
        if (executionSettings.mode == ExecutionMode.Store)
        {
            uiPopups[2].SetActive(true);
        }


    }


    public void ActivatedTutorial() { StartCoroutine(WaitForAudioToFinish(24f)); }

    private IEnumerator WaitForAudioToFinish(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        uiPopups[3].SetActive(true); // Ficha post-audio
        audioInstance.StopAudio();
    }




   
}
