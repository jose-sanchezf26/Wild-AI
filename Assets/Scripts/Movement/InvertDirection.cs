using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertDirection : MonoBehaviour
{
    private Vector2 lastPosition;
    private Vector2 currentPosition;

    // Frecuencia de cambio de dirección
    public float maxCheckFrequency = 0.1f;
    private float checkTimer = 0f;

    // Update is called once per frame
    void Update()
    {
        // Incrementa el temporizador
        checkTimer += Time.deltaTime;

        // Verifica la dirección solo si ha pasado el tiempo máximo de verificación
        if (checkTimer >= maxCheckFrequency)
        {
            //Se detecta si hay movimiento
            currentPosition = transform.position;

            // Calcula la dirección del movimiento
            float direction = Mathf.Sign(currentPosition.x - lastPosition.x);
            if (direction > 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (direction < 0)
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }

            lastPosition = currentPosition;

            // Reinicia el temporizador
            checkTimer = 0f;
        }
    }
}
