using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Transform targetCamera; // C�mara hacia la que el objeto mirar�
    public bool vertical;

    private void LateUpdate()
    {
        if (targetCamera != null) {
            // Hacer que el objeto mire directamente a la c�mara
            transform.LookAt(targetCamera.transform);

            // Invertir la rotaci�n en el eje Y si es necesario (si el objeto aparece al rev�s)
            if (vertical) {
                transform.Rotate(0, 180, -90);
            } else {
                transform.Rotate(0, 180, 0);
            }
        }
    }
}

