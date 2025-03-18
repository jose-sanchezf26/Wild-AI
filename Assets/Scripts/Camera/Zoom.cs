using UnityEngine;

public class ZoomSpeed : MonoBehaviour
{
    public float zoomSpeed = 5f;
    public float minZoomDistance = 2f;
    public float maxZoomDistance = 10f;

    void Update()
    {
        // Obtiene el valor de desplazamiento de la rueda del ratón
        float scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");

        // Calcula el nuevo desplazamiento de la cámara
        float zoomDelta = scrollWheelInput * zoomSpeed * Time.deltaTime;
        float newZoomDistance = Camera.main.transform.position.z - zoomDelta;

        // Limita el desplazamiento de la cámara dentro del rango permitido
        newZoomDistance = Mathf.Clamp(newZoomDistance, -maxZoomDistance, -minZoomDistance);

        // Actualiza la posición de la cámara
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, newZoomDistance);
    }
}