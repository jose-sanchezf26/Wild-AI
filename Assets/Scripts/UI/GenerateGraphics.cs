using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GenerateGraphics : MonoBehaviour
{
    public ModelCreator modelCreator;
    public List<string> imagesNames = new List<string>();
    public List<Image> images = new List<Image>();
    private string pythonFileName;
    
    void Start()
    {
        foreach (Image image in images)
        {
            image.gameObject.SetActive(false);
        }
        pythonFileName = "generate_graphics_l" + ObjectiveManager.Instance.level + ".py";
    }

    public void GenerateGraphicsPy()
    {
        foreach (Image image in images)
        {
            image.gameObject.SetActive(true);
        }
        // Exportamos los datos
        modelCreator.ExportAnimalData();
        // Ejecutar el .py que genera los gráficos
        modelCreator.ExecutePythonFile(pythonFileName);
        // Una vez se han generado los archivos, los cargamos
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
        {

        }
    }
}
