using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WindowsManager : MonoBehaviour
{
    public GameObject registerWindow;
    public GameObject preprocessingWindow;
    public GameObject dataWindow;
    public GameObject modelCreationWindow;
    public GameObject objectivesWindow;
    public List<GameObject> windowsList;
    public GameObject animalImage;
    public Ruler ruler;
    public WeightScale weightScale;
    public GameObject animalNumber;
    private bool onModelCreation = false;

    void Update()
    {
        // Activa el panel de registro de un animal
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (registerWindow != null)
            {
                if (!registerWindow.activeSelf)
                {
                    Time.timeScale = 0f;
                    if (ruler != null) ruler.RestartPosition();
                    if (weightScale != null) weightScale.RestartPosition();
                }
                else
                {
                    Time.timeScale = 1f;
                    if (animalImage != null) animalImage.SetActive(false);
                    if (animalNumber != null) animalNumber.SetActive(false);
                }
                CloseWindowsExcept(registerWindow);
            }
        }

        // Activa el panel de datos
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (dataWindow != null)
            {
                if (!dataWindow.activeSelf)
                    Time.timeScale = 0f;
                else
                    Time.timeScale = 1f;

                CloseWindowsExcept(dataWindow);
            }
        }

        // Activa el panel de preprocesamiento
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (preprocessingWindow != null)
            {
                if (!preprocessingWindow.activeSelf)
                    Time.timeScale = 0f;
                else
                    Time.timeScale = 1f;

                CloseWindowsExcept(preprocessingWindow);
            }
        }

        // Activa el panel de creación de modelo
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (modelCreationWindow != null)
            {
                if (!modelCreationWindow.activeSelf)
                    Time.timeScale = 0f;
                else
                    Time.timeScale = 1f;

                CloseWindowsExcept(modelCreationWindow);
            }
        }

        // Abrir panel de objetivos
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (objectivesWindow != null)
            {
                CloseWindowsExcept(objectivesWindow);
            }
        }
    }

    private void CloseWindowsExcept(GameObject activeWindow)
    {
        foreach (GameObject window in windowsList)
        {
            if (window != activeWindow)
                window.SetActive(false);
            else
                window.SetActive(!window.activeSelf);
        }
    }

    public void ClosePreprocessingWindow()
    {
        Time.timeScale = 1f;
        preprocessingWindow.SetActive(false);
        onModelCreation = false;
    }

    public int level = 0;

    public void OpenLevel(int level)
    {
        this.level = level;
        string sceneName = "Level" + level.ToString();
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(sceneName);
    }

    public void OpenLevelFromTest(int level)
    {
        this.level = level;
        string sceneName = "Level" + level.ToString();
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(sceneName);
        // AnimalDataManager.Instance.LoadAnimalList();
    }

    public void OpenLevelSelection()
    {
        string sceneName = "LevelSelection";
        ObjectiveManager.InitializeAllLevelObjectives();
        SceneManager.LoadScene(sceneName);
    }

    public void OpenTrainScene(int level)
    {
        string sceneName = "TestLevel" + level.ToString();
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(sceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Cuando la escena termine de cargar, renderiza los objetivos
        ObjectiveManager.Instance.LoadObjectivesForLevel(level);
        var renderer = FindFirstObjectByType<ObjectiveUIRenderer>();
        if (renderer != null)
        {
            renderer.RenderObjectives();
            Debug.Log("Objetivos renderizados");
        }

        // Importante: desuscribirse para evitar llamadas duplicadas
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}
