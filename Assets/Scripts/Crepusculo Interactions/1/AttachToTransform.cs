using UnityEngine;

public class AttachToTransform : MonoBehaviour
{
    [SerializeField] private string targetTag; // Tag del objeto que activará la adhesión
    [SerializeField] private Transform attachPoint; // Transform de referencia al que se pegará

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag) && attachPoint != null) {
            transform.position = attachPoint.position; // Se pega en la posición del punto de referencia
            transform.rotation = attachPoint.rotation; // Se alinea con la rotación

            transform.SetParent(attachPoint); // Se vuelve hijo del attachPoint para seguirlo
        }
    }
}
