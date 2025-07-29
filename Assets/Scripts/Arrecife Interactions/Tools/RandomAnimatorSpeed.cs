using UnityEngine;

public class RandomAnimatorSpeed : MonoBehaviour
{
    [Header("Rango de velocidad aleatoria")]
    [Tooltip("Velocidad mínima de la animación")]
    public float minSpeed = 0.8f;

    [Tooltip("Velocidad máxima de la animación")]
    public float maxSpeed = 1.5f;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (animator != null)
        {
            float randomSpeed = Random.Range(minSpeed, maxSpeed);
            animator.speed = randomSpeed;
        }
        else
        {
            Debug.LogWarning("No se encontró un componente Animator en " + gameObject.name);
        }
    }
}
