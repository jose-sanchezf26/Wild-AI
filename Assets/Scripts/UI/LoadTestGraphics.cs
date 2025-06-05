using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        string folderPath = Path.Combine(Application.streamingAssetsPath, "Python/Images");
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
        string path = Path.Combine(Application.streamingAssetsPath, "Python", "Data", metricFileName);

        if (File.Exists(path))
        {
            string reportText = File.ReadAllText(path);
            if (ObjectiveManager.Instance.level == 5)
            {
                CheckF1Score(File.ReadAllLines(path));
            }
            metricText.text = reportText;
            Debug.Log("Reporte de clasificación:\n" + reportText);
        }
        else
        {
            Debug.LogWarning("No se encontró el archivo de métricas en: " + path);
        }
    }

    private void CheckF1Score(string[] lines)
    {
        List<float> f1Scores = new List<float>();

        foreach (string line in lines)
        {
            if (line.Trim().StartsWith("Ciervo") ||
                line.Trim().StartsWith("Lobo") ||
                line.Trim().StartsWith("Oso") ||
                line.Trim().StartsWith("Zorro"))
            {
                string[] tokens = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (tokens.Length >= 4 && float.TryParse(tokens[3], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float f1))
                {
                    f1Scores.Add(f1);
                }
            }
        }
        foreach (float score in f1Scores)
        {
            Debug.Log("F1-score: " + score);
        }
        if (f1Scores.All(score => score >= 0.7f))
        {
            ObjectiveManager.Instance.AddScoreToObjective("Create model with f1-score", 1);
        }
    }
}
