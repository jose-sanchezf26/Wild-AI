using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    // Jugador al que sigue la cámara
    public Transform player;
    // Suavidad de la cámara
    public float smoothing = 5f;
    public float rigthOffset = 4;
    public float minZoom = 5f;
    public float maxZoom = 15f;
    public float zoomSpeed = 5f;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            // Calcula la posición deseada de la cámara
            Vector3 targetposition = new Vector3(player.position.x + rigthOffset, player.position.y, transform.position.z);

            // Mueve suavemente la cámara hacia la posición deseada
            transform.position = Vector3.Lerp(transform.position, targetposition, smoothing * Time.fixedDeltaTime);
        }


        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            float newSize = cam.orthographicSize - scroll * zoomSpeed;
            newSize = Mathf.Clamp(newSize, minZoom, maxZoom);
            cam.orthographicSize = newSize;
        }
    }
}
