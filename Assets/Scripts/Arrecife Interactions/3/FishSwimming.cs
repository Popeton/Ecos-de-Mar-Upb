using UnityEngine;

public class FishSwimming : MonoBehaviour
{
    [Header("Zona de movimiento")]
    public Transform swimAreaCenter;     // Centro de la esfera de movimiento (puede ser cualquier GameObject)
    public float sphereRadius = 10f;     // Radio de la esfera
    public bool initialOriginalPos;

    [Header("Comportamiento del pez")]
    public float speed = 2f;             // Velocidad de nado
    public float rotationSpeed = 1f;     // Suavidad al girar
    public float minTargetDistance = 2f; // Distancia mínima entre el pez y el nuevo destino

    private Vector3 targetPosition;

    void Start()
    {
        if (swimAreaCenter == null)
        {
            Debug.LogError("El centro de la zona de nado (swimAreaCenter) no está asignado.");
            return;
        }

        // Coloca al pez inicialmente dentro del área
        if(!initialOriginalPos)
        {
            transform.position = swimAreaCenter.position + Random.insideUnitSphere * sphereRadius;
        }
        SetNewTargetPosition();
    }

    void Update()
    {
        if (swimAreaCenter == null) return;

        MoveTowardsTarget();

        if (Vector3.Distance(transform.position, targetPosition) < 0.5f)
        {
            SetNewTargetPosition();
        }
    }

    private void MoveTowardsTarget()
    {
        Vector3 direction = (targetPosition - transform.position).normalized;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void SetNewTargetPosition()
    {
        int attempts = 0;
        do
        {
            targetPosition = swimAreaCenter.position + Random.insideUnitSphere * sphereRadius;
            attempts++;
        }
        while (Vector3.Distance(transform.position, targetPosition) < minTargetDistance && attempts < 10);
    }

    private void OnDrawGizmosSelected()
    {
        if (swimAreaCenter != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(swimAreaCenter.position, sphereRadius);
        }
    }
}

