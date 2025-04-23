using UnityEngine;

public class Explore : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float changeDirectionInterval = 2f;
    public float directionSmoothing = 0.5f;
    public bool active = false;
    public float minDurationIdle = 2f;
    public float maxDurationIdle = 5f;
    public float minDurationRun = 2f;
    public float maxDurationRun = 5f;
    private float durationIdle;
    private float durationRun;

    private float timer;
    private Vector2 currentDirection;
    private Vector2 targetDirection;

    void Start()
    {
        // Estado inicial
        currentState = State.Moving;
        // Duración del estado
        stateTimer = Random.Range(minDurationIdle, maxDurationIdle);
        // Comienza con una dirección aleatoria
        SetRandomDirection();
    }

    public void SetSpeed(float s)
    {
        moveSpeed = s;
    }

    public Animator animator;
    private enum State { Moving, Idle }
    private State currentState = State.Idle;
    private float stateTimer;

    void Update()
    {
        // Si no está activo, no hace nada
        if (!active) return;

        // Disminuimos el temporizador del estado actual
        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0)
        {
            // Cambiamos de estado
            if (currentState == State.Moving)
            {
                currentState = State.Idle;
                stateTimer = Random.Range(minDurationIdle, maxDurationIdle);
            }
            else
            {
                currentState = State.Moving;
                stateTimer = Random.Range(minDurationRun, maxDurationRun);
                SetRandomDirection();
            }
        }

        float speed = 0f;
        if (currentState == State.Moving)
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
            // Establecemos speed para la animación
            speed = currentDirection.magnitude * moveSpeed;
        }
        // Cambia la animación de idle a run y viceversa
        animator.SetFloat("Speed", speed);
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
