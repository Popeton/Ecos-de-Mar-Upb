using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Renderer))]
public class EmissiveColorLerp : MonoBehaviour
{
    [SerializeField] Color endColor; // Solo defines el color destino
    [SerializeField] float duration = 2f;
    [SerializeField] CustomEvent changeEvent;

    private Material targetMaterial;
    private Coroutine currentLerp;

    void Awake()
    {
        // Obtiene el material instanciado del Renderer
        targetMaterial = GetComponent<Renderer>().material;
    }

    private void Start()
    {
        changeEvent.GEvent += StartLerp;
    }

    public void StartLerp()
    {
        if (targetMaterial == null)
        {
            Debug.LogWarning("Material no encontrado en el objeto.");
            return;
        }

        if (currentLerp != null)
            StopCoroutine(currentLerp);

        currentLerp = StartCoroutine(LerpEmissiveColor());
    }

    private IEnumerator LerpEmissiveColor()
    {
        Color startColor = targetMaterial.GetColor("_EmissiveColour");
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            Color currentColor = Color.Lerp(startColor, endColor, t);
            targetMaterial.SetColor("_EmissiveColour", currentColor);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        targetMaterial.SetColor("_EmissiveColour", endColor);
        currentLerp = null;
    }

    private void OnDestroy()
    {
        changeEvent.GEvent -= StartLerp;
    }
}
