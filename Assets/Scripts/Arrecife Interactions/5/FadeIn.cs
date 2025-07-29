using UnityEngine;
using System.Collections;

public class FadeIn : MonoBehaviour
{
    public float fadeDuration = 2f; // Duraci�n del fade in en segundos
    private Renderer objectRenderer;
    private Color objectColor;
    private float fadeSpeed;

    [SerializeField] CustomEvent fadeEvent;

    private void Start()
    {
        fadeEvent.GEvent += StartFadeIn;

        // Obt�n el Renderer del objeto para manipular su material
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            objectColor = objectRenderer.material.color;
            //objectColor.a = 0f; // Comienza totalmente transparente
            objectRenderer.material.color = objectColor;
            fadeSpeed = 1f / fadeDuration;
            //gameObject.SetActive(true); // Aseg�rate de que est� activo
        }
        else
        {
            Debug.LogError("No se encontr� un Renderer en el objeto.");
        }
    }

    public void StartFadeIn()
    {
        if (objectRenderer != null)
        {
            StartCoroutine(FadeInCoroutine());
        }
    }

    IEnumerator FadeInCoroutine()
    {
        Color currentColor = objectRenderer.material.color;
        float alpha = currentColor.a;

        while (alpha < 1f)
        {
            alpha += Time.deltaTime * fadeSpeed;
            currentColor.a = Mathf.Clamp01(alpha);
            objectRenderer.material.color = currentColor;
            yield return null;
        }
    }


    private void OnDestroy()
    {
        fadeEvent.GEvent -= StartFadeIn;
    }
}
