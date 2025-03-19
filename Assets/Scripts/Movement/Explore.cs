using UnityEngine;

public class Explore : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float changeDirectionInterval = 2f;
    public float directionSmoothing = 0.5f;
    public bool active = false;

    private float timer;
    private Vector2 currentDirection;
    private Vector2 targetDirection;

    void Start()
    {
        // Comienza con una dirección aleatoria
        SetRandomDirection();
    }

    public void SetSpeed(float s)
    {
        moveSpeed = s;
    }

    void Update()
    {
        if (active)
        {
            // Cambia la dirección suavemente
            currentDirection = Vector2.Lerp(currentDirection, targetDirection, directionSmoothing * Time.deltaTime);

            // Mueve al bot en la dirección actual
            transform.Translate(currentDirection * moveSpeed * Time.deltaTime);

            // Actualiza el temporizador
            timer += Time.deltaTime;

            // Cambia de dirección en el intervalo especificado
            if (timer >= changeDirectionInterval)
            {
                SetRandomDirection();
                timer = 0f;
            }
        }
    }

    void SetRandomDirection()
    {
        // Genera una nueva dirección
        targetDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    public void SetActive(bool active)
    {
        this.active = active;
    }
}
