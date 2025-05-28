using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadTestGraphics : MonoBehaviour
{
    public List<string> imagesNames = new List<string>();
    public List<Image> images = new List<Image>();
    public TextMeshProUGUI metricText;
    public string metricFileName = "classification_report.txt";

    void Start()
    {
        // Cargar imágenes
        string folderPath = Path.Combine(Application.dataPath, "Python/Images");
        foreach (string imageName in imagesNames)
        {
            string imagePath = Path.Combine(folderPath, imageName);
            if (File.Exists(imagePath))
            {
                byte[] imageBytes = File.ReadAllBytes(imagePath);
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(imageBytes);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                images[imagesNames.IndexOf(imageName)].sprite = sprite;
            }
            else
            {
                Debug.LogError("Image file not found: " + imagePath);
            }
        }
        // Cargar métricas
        string path = Path.Combine(Application.dataPath, "Python", "Data", metricFileName);

        if (File.Exists(path))
        {
            string reportText = File.ReadAllText(path);
            metricText.text = reportText;
            Debug.Log("Reporte de clasificación:\n" + reportText);
        }
        else
        {
            Debug.LogWarning("No se encontró el archivo de métricas en: " + path);
        }
    }
}
