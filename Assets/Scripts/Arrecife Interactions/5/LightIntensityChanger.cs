using UnityEngine;
using System.Collections;

public class LightIntensityChanger : MonoBehaviour
{
    public Light targetLight; // Luz a modificar
    public float duration = 6f; // Duración de la transición
    public float targetIntensity = 1f; // Intensidad objetivo asignable desde el Inspector
    [SerializeField] CustomEvent changeEvent;

    private void Start()
    {
        if (targetLight == null) {
            targetLight = GetComponent<Light>();
        }
        changeEvent.GEvent += ChangeLightIntensity;
    }

    void ChangeLightIntensity()
    {
        StartCoroutine(FadeLight(targetIntensity));
    }

    private IEnumerator FadeLight(float targetIntensity)
    {
        float startIntensity = targetLight.intensity;
        float elapsedTime = 0f;

        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            targetLight.intensity = Mathf.Lerp(startIntensity, targetIntensity, elapsedTime / duration);
            yield return null;
        }

        targetLight.intensity = targetIntensity; // Asegurar el valor final
    }
    private void OnDestroy()
    {
        changeEvent.GEvent -= ChangeLightIntensity;
    }
}
