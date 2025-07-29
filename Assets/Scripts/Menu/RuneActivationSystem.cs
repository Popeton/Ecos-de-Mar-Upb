using UnityEngine;
using DG.Tweening;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System.Collections;
using EPOOutline;
using UnityEngine.SceneManagement;
using FMODUnity;

public class RuneActivationSystem : MonoBehaviour
{
    [System.Serializable]
    public class Rune
    {
        public GameObject runeObject;
        public XRSimpleInteractable interactable;
        public Outlinable outlinable;
        public GameObject crackEffect;
        public GameObject waterBall;
        public GameObject runeMat;
        public bool isRequired;
        public int targetSceneIndex;
        public EventReference audioEvent;
        public float audioDuration = 5f;
        public Material[] runeMaterials;
        public bool enableInteractionOnActivation;

        [HideInInspector] public bool isActivated;
    }

    [Header("Configuración")]
    [SerializeField] private Rune[] runes;
    [SerializeField] private int museumRuneIndex = 0;
    [SerializeField] private float raiseDuration = 2f;
    [SerializeField] private ExecutionSettings executionSettings;
    [SerializeField] private GameObject guia;

    [Header("Final Step Settings")]
    [SerializeField] private EventReference readyToChooseAudio;
    [SerializeField] private BallDisolve waterBallMuseum;
    [SerializeField] private AudioManager audioManager;


    private bool interactionsEnabled;
   

    void Start()
    {
        InitializeRunes();
        SetupInteractions();
      
    }

    public void ActivatedAutomaticallyRunes()
    {
        if (executionSettings.mode == ExecutionMode.Store)
            StartCoroutine(ActivateRunesSequentially());
    }

    public void StartStoreModeRunes()
    {
        if (executionSettings.mode == ExecutionMode.Store)
        {
            StartCoroutine(ActivateFirstRuneThenSequence());
        }
        else if (executionSettings.mode == ExecutionMode.Museum)
        {
            ActivateMuseumRunePreview();
        }
    }

    private void InitializeRunes()
    {
        foreach (var rune in runes)
        {
            ResetRune(rune);
        }
    }

    private void ResetRune(Rune rune)
    {
        rune.interactable.enabled = false;
        rune.outlinable.enabled = false;
        rune.crackEffect.SetActive(false);
        rune.isActivated = false;
    }

    private void SetupInteractions()
    {
        foreach (var rune in runes)
        {
            rune.interactable.selectEntered.AddListener(_ => OnRuneSelected(rune));
        }
    }

    private IEnumerator ActivateFirstRuneThenSequence()
    {
        yield return StartCoroutine(ActivationProcess(runes[0], 1));
        yield return new WaitForSeconds(2f);
        if (runes[0].audioDuration > 0f)
        {
            AudioManager.instance.InitializeVoice(runes[0].audioEvent, transform.position);
            yield return new WaitForSeconds(runes[0].audioDuration);
        }
        yield return StartCoroutine(ActivateRunesSequentially());
    }

    private IEnumerator ActivateRunesSequentially()
    {
        yield return new WaitForSeconds(5f); // Initial delay
        for (int i = 1; i < runes.Length; i++)
        {
            yield return StartCoroutine(ActivationProcess(runes[i], i + 1));

            if (!runes[i].audioEvent.IsNull)
            {
                AudioManager.instance.InitializeVoice(runes[i].audioEvent, transform.position);
                yield return new WaitForSeconds(runes[i].audioDuration);
            }
        }

        yield return StartCoroutine(FinalStepBeforeZoneSelection());
    }

    private IEnumerator ActivationProcess(Rune rune, int index)
    {
        guia.GetComponent<PathMover>().JumpToSpecificPath(index);
        rune.crackEffect.SetActive(true);
        AudioManager.instance.PlayOneShot(FmodEvents.instance.Sound5);

        yield return rune.runeObject.transform.DOLocalMoveY(67.21f, raiseDuration)
            .SetEase(Ease.OutQuad)
            .WaitForCompletion();

        rune.waterBall.GetComponent<BallDisolve>().Disolve();

        if (rune.enableInteractionOnActivation)
        {
            rune.outlinable.enabled = true;
            rune.interactable.enabled = true;
        }

        rune.crackEffect.SetActive(false);
        rune.isActivated = true;
     

        yield return new WaitForSeconds(0.5f);
    }

    public void ActivateMuseumRunePreview()
    {
        if (executionSettings.mode != ExecutionMode.Museum) return;

        Rune visualRune = runes[0];
        Rune dataRune = runes[museumRuneIndex];

        guia.GetComponent<PathMover>().JumpToSpecificPath(1);

        // Apply outer material
        Renderer mainRenderer = visualRune.runeMat.GetComponent<Renderer>();
        if (mainRenderer != null && dataRune.runeMaterials.Length > 0)
            mainRenderer.material = dataRune.runeMaterials[0];

        // Apply inner water + preview
        Renderer ballRenderer = visualRune.waterBall.GetComponent<Renderer>();
        if (ballRenderer != null && dataRune.runeMaterials.Length > 2)
        {
            ballRenderer.materials = new Material[]
            {
                dataRune.runeMaterials[1],
                dataRune.runeMaterials[2]
            };
        }

        StartCoroutine(RaiseMuseumRuneAndActivate(visualRune, dataRune));
    }

    public void ActivedDisolveMuseumMode()
    {
        if (executionSettings.mode == ExecutionMode.Museum)
        {
            StartCoroutine(RuneDisolve());
        }
       
    }

    private IEnumerator RuneDisolve()
    {
        yield return new WaitForSeconds(6.25f);
        waterBallMuseum.Disolve();
        // Espera breve para garantizar inicialización completa de componentes
        
        
    }
    private IEnumerator RaiseMuseumRuneAndActivate(Rune visualRune, Rune dataRune)
    {
        // Elevar la runa
        yield return visualRune.runeObject.transform.DOLocalMoveY(67.21f, raiseDuration)
            .SetEase(Ease.OutQuad)
            .WaitForCompletion();

        // Espera breve para garantizar inicialización completa de componentes
        yield return new WaitForSeconds(3.9f);

        // Disolver la esfera de agua
        //BallDisolve disolver = visualRune.waterBall.GetComponent<BallDisolve>();
        //if (disolver != null)
        //{
        //    disolver.Disolve();
        //}
        //else
        //{
        //    Debug.LogWarning("No se encontró BallDisolve en el objeto waterBall.");
        //}

        // Activar visual y lógica
        //visualRune.outlinable.enabled = true;
       // visualRune.interactable.enabled = false;
        visualRune.isActivated = true;

        // Reproducir audio si existe
        if (!dataRune.audioEvent.IsNull)
        {
            AudioManager.instance.InitializeVoice(dataRune.audioEvent, transform.position);
        }

        // Esperar duración del audio y continuar flujo
        StartCoroutine(EnableMuseumRuneAfterAudio(visualRune, dataRune.audioDuration));
    }

    private IEnumerator EnableMuseumRuneAfterAudio(Rune visualRune, float duration)
    {
        yield return new WaitForSeconds(duration);
        yield return StartCoroutine(FinalStepBeforeZoneSelection());
    }

    private IEnumerator FinalStepBeforeZoneSelection()
    {
      

        if (!readyToChooseAudio.IsNull)
            AudioManager.instance.InitializeVoice(readyToChooseAudio, transform.position);

        yield return new WaitForSeconds(2f); 

        EnableRequiredInteractions();

        this.gameObject.GetComponent<MenuConfiguration>().ShowMuseumRuneSelection();
        
    }

    public void EnableRequiredInteractions()
    {
        if (executionSettings.mode == ExecutionMode.Store)
        {
            foreach (var rune in runes)
            {
                if (rune.isRequired)
                {
                    rune.interactable.enabled = true;
                    rune.outlinable.enabled = true;
                }
            }

            interactionsEnabled = true;
        }
        else if (executionSettings.mode == ExecutionMode.Museum)
        {
            // In museum mode, only activate the visual preview rune (runes[0])
            Rune visualRune = runes[0];
            visualRune.interactable.enabled = true;
            visualRune.outlinable.enabled = true;
            interactionsEnabled = true;
        }
    }

    public void RaiseAndEnableRequiredRunes()
    {
        foreach (var rune in runes)
        {
            if (rune.isRequired)
            {
                // Solo elevar si aún no ha sido activada
                if (!rune.isActivated)
                {
                    rune.runeObject.transform.DOLocalMoveY(67.21f, raiseDuration)
                        .SetEase(Ease.OutQuad)
                        .OnComplete(() =>
                        {
                            rune.waterBall.GetComponent<BallDisolve>().Disolve();
                            rune.outlinable.enabled = true;
                            rune.interactable.enabled = true;
                            rune.isActivated = true;
                        });
                }
                else
                {
                    // Si ya está activada, solo asegura que pueda interactuar
                     rune.waterBall.GetComponent<BallDisolve>().Disolve();
                    rune.outlinable.enabled = true;
                    rune.interactable.enabled = true;
                }
            }
        }

        interactionsEnabled = true;
    }  

    private void OnRuneSelected(Rune selectedRune)
    {
        if (interactionsEnabled && selectedRune.isRequired)
        {
            MenuConfiguration.hasReturnedFromZone = true;
            int targetScene = selectedRune.targetSceneIndex;

            // En modo museo, usamos el índice de la runa configurada
            if (executionSettings.mode == ExecutionMode.Museum && selectedRune == runes[0])
            {
                targetScene = runes[museumRuneIndex].targetSceneIndex;
               // audioManager.CleanAll();
            }
            SceneReloader.ResetFlag();
            
            string sceneName = GetSceneNameByIndex(targetScene); // <-- esta es la línea corregida
            audioManager.StopAudio();
            //audioManager.CleanAll();
            Initiate.Fade(sceneName, Color.black, 1f);
           
        }
    }

    private string GetSceneNameByIndex(int index)
    {
        string path = SceneUtility.GetScenePathByBuildIndex(index);
        if (!string.IsNullOrEmpty(path))
        {
            int lastSlash = path.LastIndexOf('/');
            int lastDot = path.LastIndexOf('.');
            if (lastSlash >= 0 && lastDot >= 0)
                return path.Substring(lastSlash + 1, lastDot - lastSlash - 1);
        }
        return null;
    }
}
