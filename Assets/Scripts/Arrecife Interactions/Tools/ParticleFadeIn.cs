using UnityEngine;
using System.Collections;

public class ParticleFadeIn : MonoBehaviour
{
    public ParticleSystem targetParticleSystem;
    public float fadeDuration = 6f;
    [SerializeField] CustomEvent changeEvent;

    void Start()
    {
        if (targetParticleSystem == null)
            targetParticleSystem = GetComponent<ParticleSystem>();

        if (changeEvent != null)
            changeEvent.GEvent += AppearParticles;

    }

    void AppearParticles()
    {
        StartCoroutine(FadeInParticles());
    }

    private IEnumerator FadeInParticles()
    {
        float elapsed = 0f;
        var mainModule = targetParticleSystem.main;
        Color startColor = mainModule.startColor.color;

        // Asegura que el sistema de partículas esté corriendo
        if (!targetParticleSystem.isPlaying)
            targetParticleSystem.Play();

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            mainModule.startColor = new ParticleSystem.MinMaxGradient(
                new Color(startColor.r, startColor.g, startColor.b, newAlpha));
            yield return null;
        }

        // Asegura que quede en alpha 1
        SetAlpha(1f);
    }

    private void SetAlpha(float alpha)
    {
        if (targetParticleSystem == null) return;

        var mainModule = targetParticleSystem.main;
        Color originalColor = mainModule.startColor.color;
        mainModule.startColor = new ParticleSystem.MinMaxGradient(
            new Color(originalColor.r, originalColor.g, originalColor.b, alpha));
    }

    private void OnDestroy()
    {
        if (changeEvent != null)
            changeEvent.GEvent -= AppearParticles;
    }
}