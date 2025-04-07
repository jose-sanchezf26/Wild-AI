using UnityEngine;

public class WindowsManager : MonoBehaviour
{
    public GameObject registerWindow;
    public GameObject preprocessingWindow;
    public GameObject dataWindow;
    public GameObject modelCreationWindow;
    public GameObject animalImage;
    public Ruler ruler;
    public WeightScale weightScale;
    public GameObject animalNumber;
    private bool onModelCreation = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    // void Update()
    // {
    //     // Activa el panel de registro de un animal
    //     if (Input.GetKeyDown(KeyCode.R))
    //     {
    //         if (!registerWindow.activeSelf)
    //         {
    //             Time.timeScale = 0f;
    //             ruler.RestartPosition();
    //             weightScale.RestartPosition();
    //         }
    //         else
    //         {
    //             Time.timeScale = 1f;
    //             animalImage.SetActive(false);
    //             animalNumber.SetActive(false);
    //         }
    //         modelCreationWindow.SetActive(false);
    //         preprocessingWindow.SetActive(false);
    //         dataWindow.SetActive(false);
    //         registerWindow.SetActive(!registerWindow.activeSelf);
    //     }

    //     // Activa el panel de datos
    //     if (Input.GetKeyDown(KeyCode.D))
    //     {
    //         if (!dataWindow.activeSelf)
    //             Time.timeScale = 0f;
    //         else
    //             Time.timeScale = 1f;
    //         registerWindow.SetActive(false);
    //         modelCreationWindow.SetActive(false);
    //         preprocessingWindow.SetActive(false);
    //         dataWindow.SetActive(!dataWindow.activeSelf);
    //     }

    //     // Activa el panel de preprocesamiento
    //     if (Input.GetKeyDown(KeyCode.P))
    //     {
    //         if (!preprocessingWindow.activeSelf)
    //             Time.timeScale = 0f;
    //         else
    //             Time.timeScale = 1f;
    //         registerWindow.SetActive(false);
    //         dataWindow.SetActive(false);
    //         modelCreationWindow.SetActive(false);
    //         preprocessingWindow.SetActive(!preprocessingWindow.activeSelf);
    //     }

    //     // Activa el panel de creación de modelo
    //     if (Input.GetKeyDown(KeyCode.C))
    //     {
    //         if (!modelCreationWindow.activeSelf)
    //             Time.timeScale = 0f;
    //         else
    //             Time.timeScale = 1f;
    //         registerWindow.SetActive(false);
    //         dataWindow.SetActive(false);
    //         preprocessingWindow.SetActive(false);
    //         modelCreationWindow.SetActive(!modelCreationWindow.activeSelf);
    //     }
    // }

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
                if (modelCreationWindow != null) modelCreationWindow.SetActive(false);
                if (preprocessingWindow != null) preprocessingWindow.SetActive(false);
                if (dataWindow != null) dataWindow.SetActive(false);
                registerWindow.SetActive(!registerWindow.activeSelf);
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

                if (registerWindow != null) registerWindow.SetActive(false);
                if (modelCreationWindow != null) modelCreationWindow.SetActive(false);
                if (preprocessingWindow != null) preprocessingWindow.SetActive(false);
                dataWindow.SetActive(!dataWindow.activeSelf);
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

                if (registerWindow != null) registerWindow.SetActive(false);
                if (dataWindow != null) dataWindow.SetActive(false);
                if (modelCreationWindow != null) modelCreationWindow.SetActive(false);
                preprocessingWindow.SetActive(!preprocessingWindow.activeSelf);
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

                if (registerWindow != null) registerWindow.SetActive(false);
                if (dataWindow != null) dataWindow.SetActive(false);
                if (preprocessingWindow != null) preprocessingWindow.SetActive(false);
                modelCreationWindow.SetActive(!modelCreationWindow.activeSelf);
            }
        }
    }

    public void ClosePreprocessingWindow()
    {
        Time.timeScale = 1f;
        preprocessingWindow.SetActive(false);
        onModelCreation = false;
    }
}
