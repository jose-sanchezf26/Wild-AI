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

    void Start()
    {
        createModelFileName = "create_model_l" + level + ".py";
        testModelFileName = "test_model_l" + level + ".py";
    }
    public void CreateModel()
    {
        ExportAnimalData();
        ExecutePythonFile(createModelFileName);
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

    // private void ExecutePythonFile(string fileName)
    // {
    //     // Crea un proceso de Python
    //     ProcessStartInfo startInfo = new ProcessStartInfo();

    //     // Establece el ejecutable de Python y pasa la ruta del script
    //     startInfo.FileName = "python3"; // Si estás en Windows, puede ser "python" o "python3"
    //     string folderPath = Path.Combine(Application.dataPath, "Python/Scripts");
    //     startInfo.Arguments = Path.Combine(folderPath, fileName);

    //     // Ejecuta el proceso
    //     Process process = Process.Start(startInfo);
    //     process.WaitForExit(); // Espera hasta que el proceso termine
    // }

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
        UnityEngine.SceneManagement.SceneManager.LoadScene("TestLevel1");
    }

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

            UnityEngine.Debug.Log("Width: " + width);
            UnityEngine.Debug.Log("Height: " + height);
            UnityEngine.Debug.Log("Predicted Weight: " + predictedWeight);
        }
    }

}
