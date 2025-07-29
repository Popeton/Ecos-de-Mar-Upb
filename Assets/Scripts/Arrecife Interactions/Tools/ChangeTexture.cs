using System.Collections;
using UnityEngine;

public class ChangeTexture : MonoBehaviour
{
    private Renderer targetRenderer;
    public float transitionDuration = 6f;

    [SerializeField] CustomEvent custom;

    private Material[] materials;
    private float mixFactor = -3;
    private float targetMixFactor = 5;

    private void Awake()
    {
        targetRenderer = GetComponent<Renderer>();
    }

    void Start()
    {
        if (targetRenderer == null)
        {
            Debug.LogError("El Renderer no está asignado");
            return;
        }

        materials = targetRenderer.materials; // obtener todos los materiales

        custom.GEvent += StartTransition;
    }

    public void StartTransition()
    {
        StartCoroutine(TransitionMixFactor());
    }

    private IEnumerator TransitionMixFactor()
    {
        float elapsedTime = 0;
        float startValue = mixFactor;

        while (elapsedTime < transitionDuration)
        {
            mixFactor = Mathf.Lerp(startValue, targetMixFactor, elapsedTime / transitionDuration);

            foreach (var mat in materials)
            {
                mat.SetFloat("_Mix_factor", mixFactor);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        foreach (var mat in materials)
        {
            mat.SetFloat("_Mix_factor", targetMixFactor);
        }
    }

    private void OnDestroy()
    {
        custom.GEvent -= StartTransition;
    }
}
