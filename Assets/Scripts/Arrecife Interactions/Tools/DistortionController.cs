using System.Collections;
using UnityEngine;

public class DistortionController : MonoBehaviour
{
    public static DistortionController Instance { get; private set; }

    

    [Header("Valores Objetivo (asignar desde Inspector)")]
    public Vector4 targetSpeeds = new Vector4(0.2f, 0.05f, 0f, 0f);
    public float targetDistortion = 0.002f;
    public float targetBlendDistance = 1.0f;
    public float targetScale = 0.5f;
    public float targetWaterLevel = 3.5f;
    [ColorUsage(true, true)]
    public Color targetTint = new Color(0.1f, 0.5f, 1.0f, 1.0f);

    [Header("Valores Originales (se restauran con Reset)")]
    [SerializeField] Vector4 originalSpeeds;
    [SerializeField] float originalDistortion;
    [SerializeField] float originalBlendDistance;
    [SerializeField] float originalScale;
    [SerializeField] float originalWaterLevel;
    [ColorUsage(true, true)]
    [SerializeField] Color originalTint;

    [Header("Configuraciones generales")]
    public float transitionDuration = 2.5f;
    public Material distortionMaterial;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    void Start()
    {
        // Guardar los valores originales actuales del material
        //originalSpeeds = distortionMaterial.GetVector("_Speeds");
        //originalDistortion = distortionMaterial.GetFloat("_Distortion");
        //originalBlendDistance = distortionMaterial.GetFloat("_DistortionBlendDistance");
        //originalScale = distortionMaterial.GetFloat("_Distortion_Scale");
        //originalWaterLevel = distortionMaterial.GetFloat("_World_Water_Level");
        //originalTint = distortionMaterial.GetColor("_UnderwaterTint");

        // Puedes iniciar aquí si lo deseas, o llamarlo externamente
        ResetDistortionProperties();
    }

    public void TriggerDistortionChange()
    {
        StartCoroutine(AnimateDistortionProperties(
            targetSpeeds,
            targetDistortion,
            targetBlendDistance,
            targetScale,
            targetWaterLevel,
            targetTint,
            transitionDuration
        ));
    }

    public void ResetDistortionProperties()
    {
        StartCoroutine(AnimateDistortionProperties(
            originalSpeeds,
            originalDistortion,
            originalBlendDistance,
            originalScale,
            originalWaterLevel,
            originalTint,
            transitionDuration
        ));
    }

    IEnumerator AnimateDistortionProperties(
        Vector4 targetSpeeds,
        float targetDistortion,
        float targetBlendDistance,
        float targetScale,
        float targetWaterLevel,
        Color targetTint,
        float duration)
    {
        float elapsed = 0f;

        Vector4 startSpeeds = distortionMaterial.GetVector("_Speeds");
        float startDistortion = distortionMaterial.GetFloat("_Distortion");
        float startBlendDistance = distortionMaterial.GetFloat("_DistortionBlendDistance");
        float startScale = distortionMaterial.GetFloat("_Distortion_Scale");
        float startWaterLevel = distortionMaterial.GetFloat("_World_Water_Level");
        Color startTint = distortionMaterial.GetColor("_UnderwaterTint");

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            distortionMaterial.SetVector("_Speeds", Vector4.Lerp(startSpeeds, targetSpeeds, t));
            distortionMaterial.SetFloat("_Distortion", Mathf.Lerp(startDistortion, targetDistortion, t));
            distortionMaterial.SetFloat("_DistortionBlendDistance", Mathf.Lerp(startBlendDistance, targetBlendDistance, t));
            distortionMaterial.SetFloat("_Distortion_Scale", Mathf.Lerp(startScale, targetScale, t));
            distortionMaterial.SetFloat("_World_Water_Level", Mathf.Lerp(startWaterLevel, targetWaterLevel, t));
            distortionMaterial.SetColor("_UnderwaterTint", Color.Lerp(startTint, targetTint, t));

            elapsed += Time.deltaTime;
            yield return null;
        }

        distortionMaterial.SetVector("_Speeds", targetSpeeds);
        distortionMaterial.SetFloat("_Distortion", targetDistortion);
        distortionMaterial.SetFloat("_DistortionBlendDistance", targetBlendDistance);
        distortionMaterial.SetFloat("_Distortion_Scale", targetScale);
        distortionMaterial.SetFloat("_World_Water_Level", targetWaterLevel);
        distortionMaterial.SetColor("_UnderwaterTint", targetTint);
    }

    void RestoreInitial()
    {
        distortionMaterial.SetVector("_Speeds", originalSpeeds);
        distortionMaterial.SetFloat("_Distortion", originalDistortion);
        distortionMaterial.SetFloat("_DistortionBlendDistance", originalBlendDistance);
        distortionMaterial.SetFloat("_Distortion_Scale", originalScale);
        distortionMaterial.SetFloat("_World_Water_Level", originalWaterLevel);
        distortionMaterial.SetColor("_UnderwaterTint", originalTint);
    }
    private void OnDestroy()
    {
        RestoreInitial();
    }
}