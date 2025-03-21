using UnityEngine;

public class WindowsManager : MonoBehaviour
{
    public GameObject registerWindow;
    public GameObject animalImage;
    public Ruler ruler;
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
            if (!registerWindow.activeSelf)
            {
                Time.timeScale = 0f;
                ruler.RestartPosition();
            }
            else
            {
                Time.timeScale = 1f;
                animalImage.SetActive(false);
                animalNumber.SetActive(false);
            }
            registerWindow.SetActive(!registerWindow.activeSelf);
        }
    }


}
