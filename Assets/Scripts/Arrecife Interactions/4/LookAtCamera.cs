using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Transform targetCamera; // Cámara hacia la que el objeto mirará
    public bool vertical;

    private void LateUpdate()
    {
        if (targetCamera != null) {
            // Hacer que el objeto mire directamente a la cámara
            transform.LookAt(targetCamera.transform);

            // Invertir la rotación en el eje Y si es necesario (si el objeto aparece al revés)
            if (vertical) {
                transform.Rotate(0, 180, -90);
            } else {
                transform.Rotate(0, 180, 0);
            }
        }
    }
}

