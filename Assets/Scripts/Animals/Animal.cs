using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.EventSystems;

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
    [SerializeField] private float densityFactor = 250f; // Factor de densidad para el peso
    [SerializeField] private Color[] possibleColors;
    [SerializeField] private float multiplier = 1.0f;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private GameObject uiImage;
    private RectTransform uiRectTransform;
    private Camera mainCamera;

    void Awake()
    {
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        uiImage = GameObject.Find("AnimalImage");
        if (uiImage != null)
        {
            uiRectTransform = uiImage.GetComponent<RectTransform>();
            Debug.Log("UI Image found");
        }

        ApplyRandomVariations();
    }

    private void ApplyRandomVariations()
    {
        height = Random.Range(heightRange.x, heightRange.y);
        width = Random.Range(widthRange.x, widthRange.y);

        if (possibleColors.Length > 0)
        {
            color = possibleColors[Random.Range(0, possibleColors.Length)];
            spriteRenderer.color = color;
        }
    }

    public void CalculateWeight(float noisePercentage)
    {
        // Ahora calculamos el peso basado en las dimensiones y la densidad
        float weightBase = height * width * densityFactor;
        float noise = weightBase * Random.Range(-noisePercentage, noisePercentage);
        weight = weightBase + noise;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Esto imprimirá el nombre del objeto con el que el animal está colisionando
        Debug.Log("Animal colisionó con: " + collision.gameObject.name);
    }


    private bool isMouseOver = false;
    private void Update()
    {
        if (Time.timeScale == 0f)
        {
            // Detectar la posición del ratón
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            // Detectar colisiones con un Raycast2D
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                if (!isMouseOver)
                {
                    Debug.Log("El ratón está sobre el animal: " + gameObject.name);
                    isMouseOver = true;
                    AdaptImage();
                }
            }
            else if (isMouseOver)
            {
                isMouseOver = false;
                uiImage.SetActive(false);
            }
        }
    }

    private void AdaptImage()
    {
        if (uiImage != null)
        {
            // Pasamos los atributos del animal a la imagen
            AnimalRulerDrop rulerDrop = uiImage.GetComponent<AnimalRulerDrop>();
            AnimalWeightScaleDrop weightScaleDrop = uiImage.GetComponent<AnimalWeightScaleDrop>();
            AnimalData animal = new AnimalData();
            animal.weight = weight;
            animal.height = height;
            animal.width = width;
            animal.color = "indeterminado";
            rulerDrop.animal = animal; 
            weightScaleDrop.animalData = animal;

            // Convertir las dimensiones del animal a la UI teniendo en cuenta el zoom de la cámara
            float worldWidth = GetComponent<SpriteRenderer>().bounds.size.x;
            float worldHeight = GetComponent<SpriteRenderer>().bounds.size.y;

            // Invertir el cálculo de zoom: cuanto mayor es el zoom, más grande es la imagen
            float zoomFactor = 1f / (mainCamera.orthographicSize / 5f); // Invertir el zoom
            float uiWidth = worldWidth * zoomFactor * 100;  // Escala la imagen a la UI y ajusta al zoom
            float uiHeight = worldHeight * zoomFactor * 100;

            // Aplicar tamaño y posición en la UI
            uiRectTransform.sizeDelta = new Vector2(uiWidth, uiHeight); // Ajuste de tamaño
            uiRectTransform.position = mainCamera.WorldToScreenPoint(transform.position); // Ajuste de posición
            uiImage.SetActive(true);
        }
    }
}

public class AnimalData
{
    public int id;
    public string name;
    public float weight;
    public float height;
    public float width;
    public string color;
}
