using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleColorFader : MonoBehaviour
{
    private ParticleSystem ps;
    private Color originalColor;

    
    [SerializeField] Color targetColor = Color.red;
    [SerializeField] float transitionDuration = 2.5f;

    [SerializeField] CustomEvent changeColorEvent;
    [SerializeField] CustomEvent restoreColorEvent;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        originalColor = ps.main.startColor.color;
        changeColorEvent.GEvent += FadeToTargetColor;
        restoreColorEvent.GEvent += RestoreOriginalColor;
    }

    public void FadeToTargetColor()
    {
        StartCoroutine(FadeColorCoroutine(ps.main.startColor.color, targetColor, transitionDuration));
    }

    public void RestoreOriginalColor()
    {
        StartCoroutine(FadeColorCoroutine(ps.main.startColor.color, originalColor, transitionDuration));
    }

    private IEnumerator FadeColorCoroutine(Color fromColor, Color toColor, float duration)
    {
        float elapsed = 0f;
        var main = ps.main;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            Color newColor = Color.Lerp(fromColor, toColor, t);
            main.startColor = newColor;
            elapsed += Time.deltaTime;
            yield return null;
        }

        main.startColor = toColor;
        ps.Clear();
        ps.Play();
    }

    private void OnDestroy()
    {
        changeColorEvent.GEvent -= FadeToTargetColor;
        restoreColorEvent.GEvent -= RestoreOriginalColor;
    }
}
