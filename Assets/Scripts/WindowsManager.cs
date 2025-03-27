using UnityEngine;

public class WindowsManager : MonoBehaviour
{
    public GameObject registerWindow;
    public GameObject dataWindow;
    public GameObject animalImage;
    public Ruler ruler;
    public WeightScale weightScale;
    public GameObject animalNumber;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Activa el panel de registro de un animal
        if (Input.GetKeyDown(KeyCode.R))
        {
            dataWindow.SetActive(false);
            if (!registerWindow.activeSelf)
            {
                Time.timeScale = 0f;
                ruler.RestartPosition();
                weightScale.RestartPosition();
            }
            else
            {
                Time.timeScale = 1f;
                animalImage.SetActive(false);
                animalNumber.SetActive(false);
            }
            registerWindow.SetActive(!registerWindow.activeSelf);
        }

        // Activa el panel de datos
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (!dataWindow.activeSelf)
                Time.timeScale = 0f;
            else
                Time.timeScale = 1f;
            registerWindow.SetActive(false);
            dataWindow.SetActive(!dataWindow.activeSelf);
        }
    }


}
