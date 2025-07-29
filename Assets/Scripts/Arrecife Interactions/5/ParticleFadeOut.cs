using UnityEngine;
using System.Collections;
public class ParticleFadeOut : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public float fadeDuration = 6f; // Tiempo en segundos para desaparecer
    [SerializeField] CustomEvent changeEvent;

    void Start()
    {
        if (particleSystem == null) {
            particleSystem = GetComponent<ParticleSystem>();
        }
        changeEvent.GEvent += DisappearParticles;
    }

    void DisappearParticles()
    {
        StartCoroutine(FadeOutParticles());
    }

    private IEnumerator FadeOutParticles()
    {
        float elapsed = 0f;
        ParticleSystem.MainModule mainModule = particleSystem.main;
        Color startColor = mainModule.startColor.color;

        while (elapsed < fadeDuration) {
            elapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration); // Reduce alpha de 1 a 0
            mainModule.startColor = new Color(startColor.r, startColor.g, startColor.b, newAlpha);
            yield return null;
        }

        particleSystem.Stop();
    }

    private void OnDestroy()
    {
        changeEvent.GEvent -= DisappearParticles;
    }
}

