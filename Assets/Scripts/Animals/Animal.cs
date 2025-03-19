using UnityEngine;

public class Animal : MonoBehaviour
{
    [Header("Animal Properties")]
    public float weight = 10f;
    public float height = 1.0f;
    public float width = 1.0f;
    public Color color;

    [Header("Random Variation Settings")]
    [SerializeField] private Vector2 heightRange = new Vector2(0.8f, 1.2f);
    [SerializeField] private Vector2 widthRange = new Vector2(0.8f, 1.2f);
    [SerializeField] private Vector2 weightRange = new Vector2(0.8f, 1.2f);
    [SerializeField] private Color[] possibleColors;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        ApplyRandomVariations();
        // AdjustCollider();
    }

    private void ApplyRandomVariations()
    {
        height *= Random.Range(heightRange.x, heightRange.y);
        width *= Random.Range(widthRange.x, widthRange.y);
        weight *= Random.Range(weightRange.x, weightRange.y);
        transform.localScale = new Vector3(width, height, 1);

        if (possibleColors.Length > 0)
        {
            color = possibleColors[Random.Range(0, possibleColors.Length)];
            spriteRenderer.color = color;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Esto imprimirá el nombre del objeto con el que el animal está colisionando
        Debug.Log("Animal colisionó con: " + collision.gameObject.name);
    }
}
