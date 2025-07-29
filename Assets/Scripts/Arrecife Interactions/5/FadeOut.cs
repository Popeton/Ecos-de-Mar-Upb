using UnityEngine;
using System.Collections;
public class FadeOut : MonoBehaviour
{
    public float fadeDuration = 2f; // Duración del fade out en segundos
    private Renderer objectRenderer;
    private Color objectColor;
    private float fadeSpeed;

    [SerializeField] CustomEvent fadeEvent;

    private void Start()
    {
        fadeEvent.GEvent += StartFadeOut;

        // Obtén el Renderer del objeto para manipular su material
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null) {
            objectColor = objectRenderer.material.color;
            fadeSpeed = 1f / fadeDuration;
        } else {
            Debug.LogError("No se encontró un Renderer en el objeto.");
        }

    }

    public void StartFadeOut()
    {
        if (objectRenderer != null) {
            StartCoroutine(FadeOutCoroutine());
        }
    }

    IEnumerator FadeOutCoroutine()
    {
        Color currentColor = objectRenderer.material.color;
        float alpha = currentColor.a;

        while (alpha > 0f)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            currentColor.a = Mathf.Clamp01(alpha);
            objectRenderer.material.color = currentColor;
            yield return null;
        }

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        fadeEvent.GEvent -= StartFadeOut;
    }
}

