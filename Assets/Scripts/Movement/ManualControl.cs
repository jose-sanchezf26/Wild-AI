using UnityEngine;

public class ManualControl : MonoBehaviour
{
     [Header("Movimiento")]
    public float speed = 5f;
    private Vector2 movement;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    public Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Capturar movimiento en X e Y
        float movX = Input.GetAxisRaw("Horizontal");
        float movY = Input.GetAxisRaw("Vertical");
        movement = new Vector2(movX, movY).normalized * speed;
        animator.SetFloat("Speed", Mathf.Abs(movement.magnitude));

        
        // Rotar sprite según dirección
        if (movX > 0) sr.flipX = false;
        if (movX < 0) sr.flipX = true;
    }

    void FixedUpdate()
    {
        // Aplicar movimiento
        rb.linearVelocity = movement;
    }
}
