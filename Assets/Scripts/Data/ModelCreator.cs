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
        level = ObjectiveManager.Instance.level;
        createModelFileName = "create_model_l" + level + ".py";
        testModelFileName = "test_model_l" + level + ".py";
    }
    public void CreateModel()
    {
        ExportAnimalData();
        modelCreated = true;
        ExecutePythonFile(createModelFileName);
        if (modelCreated)
        {
            NotificationManager.Instance.ShowNotification("Modelo creado con éxito!");
        }
    }

    public void CreateModelWithParameters()
    {
        ExportAnimalData();
        ExportModelParameters();
        modelCreated = true;
        ExecutePythonFile(createModelFileName);
        if (modelCreated)
        {
            NotificationManager.Instance.ShowNotification("Modelo creado con éxito!");
            EventLogger.Instance.LogEvent(new EventData("wai-model_created", new CreateModelEvent(parameters)));
        }
    }

    string SafeNumber(float val) =>
    float.IsNaN(val) ? "" : val.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture);

    public void ExportAnimalData(string fileName = "animal_data.csv")
    {
        StringBuilder csvContent = new StringBuilder();



        if (level == 1)
        {
            // Cabecera
            csvContent.AppendLine("Weight,Height,Width");

            // Contenido
            foreach (var animal in AnimalDataSingleton.Instance.animalDataList)
            {
                string width = SafeNumber(animal.width);
                string height = SafeNumber(animal.height);
                string weight = SafeNumber(animal.weight);
                string line = $"{weight},{height},{width}";
                csvContent.AppendLine(line);
            }
        }
        else
        {
            // Cabecera
            csvContent.AppendLine("Weight,Height,Width,Color,Animal");

            // Contenido
            foreach (var animal in AnimalDataSingleton.Instance.animalDataList)
            {
                string width = SafeNumber(animal.width);
                string height = SafeNumber(animal.height);
                string weight = SafeNumber(animal.weight);
                string color = animal.color;
                string animalType = animal.name;
                string line = $"{weight},{height},{width},{color},{animalType}";
                csvContent.AppendLine(line);
            }
        }



        // Ruta de guardado
        string folderPath = Path.Combine(Application.streamingAssetsPath, "Python/data");
        string filePath = Path.Combine(folderPath, fileName);
        File.WriteAllText(filePath, csvContent.ToString());

        UnityEngine.Debug.Log($"CSV exportado correctamente en: {filePath}");
    }

    public void ExecutePythonFile(string fileName)
    {
        try
        {
            // Crea un proceso de Python
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "py"; // O usa "python3" si estás en un sistema que lo requiere

            // Ruta del script Python
            string folderPath = Path.Combine(Application.streamingAssetsPath, "Python/Scripts");
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
                modelCreated = false;
                UnityEngine.Debug.LogError("Python Error: " + error);
                NotificationManager.Instance.ShowNotification("Error al crear el modelo");
            }
        }
        catch (Exception e)
        {
            modelCreated = false;
            UnityEngine.Debug.LogError("Error ejecutando el script Python: " + fileName + " " + e.Message);
            NotificationManager.Instance.ShowNotification("Error al crear el modelo");
        }
    }

    public TMP_InputField widthInput;
    public TMP_InputField heightInput;
    public TMP_InputField weightInput;
    public TMP_Dropdown colorDropdown;
    public TextMeshProUGUI predictionText;
    public TextMeshProUGUI errorText;

    public void GoToTestScene()
    {
        // Cambia a la escena de prueba
        if (modelCreated)
        {
            Time.timeScale = 1f;
            string sceneName = "TestLevel" + level.ToString();
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
            EventLogger.Instance.LogEvent(new EventData("wai-go_test_scene", new LevelEvent(level)));
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
        Dictionary<string, object> data = new Dictionary<string, object>();
        if (level == 1)
        {
            data = new Dictionary<string, object>
            {
                { "width", float.Parse(widthInput.text)},
                { "height", float.Parse(heightInput.text) }
            };
        }
        else
        {
            data = new Dictionary<string, object>
            {
                { "width", widthInput.text },
                { "height", heightInput.text },
                { "weight", weightInput.text },
                { "color", colorDropdown.options[colorDropdown.value].text }
            };
        }
        json = JsonConvert.SerializeObject(data, Formatting.Indented);

        string path = Application.streamingAssetsPath + "/Python/Data/input.json";
        File.WriteAllText(path, json);

        // Ejecuta el script de Python
        ExecutePythonFile(testModelFileName);

        // Obtiene la predicción y el error del modelo
        string outputPath = Application.streamingAssetsPath + "/Python/Data/output.json";
        if (File.Exists(outputPath))
        {
            string outputJson = File.ReadAllText(outputPath);
            // Usar JObject de Newtonsoft.Json para leer el JSON
            JObject jsonObject = JObject.Parse(outputJson);
            if (level == 1)
            {
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
            }
            else
            {

                string predictedAnimal = jsonObject["predicted_animal"].Value<string>();
                EventLogger.Instance.LogEvent(new EventData("wai-predict_animal", new TestAnimalEvent(ObjectiveManager.Instance.level, widthInput.text, heightInput.text, weightInput.text, colorDropdown.options[colorDropdown.value].text, predictedAnimal)));
                predictionText.text = predictedAnimal;
                CheckModelUsingBalancing();
            }
            NotificationManager.Instance.ShowNotification("Predicción realizada con éxito!");
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

    // Exportar configuración del modelo a JSON
    public TMP_Dropdown modelTypeDropdown;
    public List<ModelParameter> modelParameters;
    private string parameters = "";

    public void ExportModelParameters()
    {
        Dictionary<string, string> parametersDict = new Dictionary<string, string>();
        // Se crea esta variable para poder exportar el tipo de modelo
        parametersDict["modelType"] = modelTypeDropdown.options[modelTypeDropdown.value].text;
        // Se recorre la lista de parámetros del modelo y se exportan sus valores
        foreach (var parameter in modelParameters)
        {
            // Se comprueba si es nulo para evitar errores
            if (parameter == null)
            {
                UnityEngine.Debug.LogWarning("Parameter is null, skipping export.");
                continue;
            }
            parameter.GetValues();
            if (!string.IsNullOrEmpty(parameter.parameterName))
            {
                parametersDict[parameter.parameterName] = parameter.parameterValue;
            }
        }
        // Se crea el JSON con los parámetros del modelo y se guarda
        string json = JsonConvert.SerializeObject(parametersDict, Formatting.Indented);
        parameters = json;
        string path = Application.streamingAssetsPath + "/Python/Data/model_parameters.json";
        File.WriteAllText(path, json);
    }

    // Función para comprobar si el modelo utiliza técnica de desbalanceo
    public void CheckModelUsingBalancing()
    {
        string path = Application.streamingAssetsPath + "/Python/Data/model_parameters.json";
        string json = File.ReadAllText(path);
        if (json.Contains("Técnica de desbalanceo\": \"\""))
        {
            ObjectiveManager.Instance.AddScoreToObjective("Create model desbalanced", 1);
        }
        else
        {
            ObjectiveManager.Instance.AddScoreToObjective("Create model balanced", 1);
        }
    }
}

[System.Serializable]
public class ParameterDataList
{
    public List<ParameterData> parameters;
}
