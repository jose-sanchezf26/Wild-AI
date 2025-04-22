using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using System.Diagnostics;
using TMPro;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;

public class ModelCreator : MonoBehaviour
{
    public int level;
    private string createModelFileName;
    private string testModelFileName;
    private bool modelCreated = false;

    void Start()
    {
        createModelFileName = "create_model_l" + level + ".py";
        testModelFileName = "test_model_l" + level + ".py";
    }
    public void CreateModel()
    {
        ExportAnimalData();
        ExecutePythonFile(createModelFileName);
        modelCreated = true;

    }

    private void ExportAnimalData(string fileName = "animal_data.csv")
    {
        StringBuilder csvContent = new StringBuilder();

        // Cabecera
        csvContent.AppendLine("Weight,Height,Width");

        // Contenido
        foreach (var animal in AnimalDataManager.Instance.animalDataList)
        {
            string width = animal.width.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture);
            string height = animal.height.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture);
            string weight = animal.weight.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture);
            string line = $"{weight},{height},{width}";
            csvContent.AppendLine(line);
        }

        // Ruta de guardado
        string folderPath = Path.Combine(Application.dataPath, "Python/data");
        string filePath = Path.Combine(folderPath, fileName);
        File.WriteAllText(filePath, csvContent.ToString());

        UnityEngine.Debug.Log($"CSV exportado correctamente en: {filePath}");
    }

    private void ExecutePythonFile(string fileName)
    {
        try
        {
            // Crea un proceso de Python
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "py"; // O usa "python3" si estás en un sistema que lo requiere

            // Ruta del script Python
            string folderPath = Path.Combine(Application.dataPath, "Python/Scripts");
            string scriptPath = Path.Combine(folderPath, fileName);
            startInfo.Arguments = $"\"{scriptPath}\"";

            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;

            // Ejecuta el proceso
            Process process = Process.Start(startInfo);

            // Lee la salida y los errores del proceso
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            // Espera hasta que el proceso termine
            process.WaitForExit();

            // Log de la salida
            UnityEngine.Debug.Log("Python Output: " + output);
            if (!string.IsNullOrEmpty(error))
            {
                UnityEngine.Debug.LogError("Python Error: " + error);
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("Error ejecutando el script Python: " + e.Message);
        }
    }

    public TMP_InputField widthInput;
    public TMP_InputField heightInput;
    public TMP_InputField weightInput;
    public TextMeshProUGUI predictionText;
    public TextMeshProUGUI errorText;

    public void GoToTestScene()
    {
        Time.timeScale = 1f;
        // Cambia a la escena de prueba
        if (modelCreated)
        {
            string sceneName = "TestLevel" + level.ToString();
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
        else
            NotificationManager.Instance.ShowNotification("¡Primero crea el modelo!");
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        // Cuando la escena termine de cargar, renderiza los objetivos
        var renderer = FindFirstObjectByType<ObjectiveUIRenderer>();
        if (renderer != null)
        {
            renderer.RenderObjectives();
            UnityEngine.Debug.Log("Objetivos renderizados");
        }
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private Queue<float> errorHistory = new Queue<float>();

    public void TestModel()
    {
        // Exporta a json los datos del animal
        string json;
        var data = new Dictionary<string, float>();
        if (level == 1)
        {
            data = new Dictionary<string, float>
            {
                { "width", float.Parse(widthInput.text)},
                { "height", float.Parse(heightInput.text) }
            };
        }
        json = JsonConvert.SerializeObject(data, Formatting.Indented);

        string path = Application.dataPath + "/Python/Data/input.json";
        File.WriteAllText(path, json);

        // Ejecuta el script de Python
        ExecutePythonFile(testModelFileName);

        // Obtiene la predicción y el error del modelo
        string outputPath = Application.dataPath + "/Python/Data/output.json";
        if (File.Exists(outputPath))
        {
            string outputJson = File.ReadAllText(outputPath);

            // Usar JObject de Newtonsoft.Json para leer el JSON
            JObject jsonObject = JObject.Parse(outputJson);

            // Extraer los valores directamente
            float width = jsonObject["width"].Value<float>();
            float height = jsonObject["height"].Value<float>();
            float predictedWeight = jsonObject["predicted_weight"].Value<float>();

            // Mostrar los resultados
            predictionText.text = predictedWeight.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture);
            float realWeight = float.Parse(weightInput.text);
            errorText.text = Mathf.Abs(realWeight - predictedWeight).ToString("0.##", System.Globalization.CultureInfo.InvariantCulture);
            ObjectiveManager.Instance.AddScoreToObjective("Create model", 1);
            ProcessErrorHistory(realWeight, predictedWeight);

            UnityEngine.Debug.Log("Width: " + width);
            UnityEngine.Debug.Log("Height: " + height);
            UnityEngine.Debug.Log("Predicted Weight: " + predictedWeight);
        }
    }

    private int maxHistorySize = 5; // Tamaño máximo de la historia de errores

    private void ProcessErrorHistory(float realWeight, float predictedWeight)
    {
        float absoluteError = Mathf.Abs(realWeight - predictedWeight);
        float percentageError = (absoluteError / realWeight) * 100f;

        if (errorHistory.Count >= maxHistorySize)
        {
            errorHistory.Dequeue();
        }
        errorHistory.Enqueue(percentageError);
        if (errorHistory.Count == maxHistorySize)
        {
            float averageError = 0f;
            foreach (float error in errorHistory)
            {
                averageError += error;
            }
            averageError /= errorHistory.Count;
            ObjectiveManager.Instance.AddScoreToObjective("Reach Error", averageError);
            NotificationManager.Instance.ShowNotification("Error promedio: " + averageError.ToString("F2") + "%");
        }
    }

}
