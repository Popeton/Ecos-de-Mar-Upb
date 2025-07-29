using System.Collections;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    Material targetMaterial; // Material a cambiar
    [SerializeField] Color targetColor = Color.red; // Color objetivo
    [SerializeField] float duration = 6f; // Duración de la transición en segundos
    [SerializeField] CustomEvent changeEvent;

    private void Start()
    {
        if (targetMaterial == null) {
            targetMaterial = GetComponent<Renderer>().material; // Obtiene el material del objeto si no se asigna uno
        }
        changeEvent.GEvent += ChangeColor;
    }

     void ChangeColor()
    {
        StartCoroutine(ChangeColorCoroutine(targetColor, duration));
    }

    private IEnumerator ChangeColorCoroutine(Color newColor, float time)
    {
        Color initialColor = targetMaterial.color;
        float elapsedTime = 0f;

        while (elapsedTime < time) {
            elapsedTime += Time.deltaTime;
            targetMaterial.color = Color.Lerp(initialColor, newColor, elapsedTime / time);
            yield return null;
        }

        targetMaterial.color = newColor; // Asegurar que el color final se establezca correctamente
    }
    private void OnDestroy()
    {
        changeEvent.GEvent -= ChangeColor;
    }
}
