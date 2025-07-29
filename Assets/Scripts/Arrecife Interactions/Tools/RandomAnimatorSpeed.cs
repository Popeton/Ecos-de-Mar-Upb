using UnityEngine;

public class RandomAnimatorSpeed : MonoBehaviour
{
    [Header("Rango de velocidad aleatoria")]
    [Tooltip("Velocidad m�nima de la animaci�n")]
    public float minSpeed = 0.8f;

    [Tooltip("Velocidad m�xima de la animaci�n")]
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
            Debug.LogWarning("No se encontr� un componente Animator en " + gameObject.name);
        }
    }
}
