using UnityEngine;

public class AttachToTransform : MonoBehaviour
{
    [SerializeField] private string targetTag; // Tag del objeto que activar� la adhesi�n
    [SerializeField] private Transform attachPoint; // Transform de referencia al que se pegar�

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag) && attachPoint != null) {
            transform.position = attachPoint.position; // Se pega en la posici�n del punto de referencia
            transform.rotation = attachPoint.rotation; // Se alinea con la rotaci�n

            transform.SetParent(attachPoint); // Se vuelve hijo del attachPoint para seguirlo
        }
    }
}
