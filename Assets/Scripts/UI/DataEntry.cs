using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataEntry : MonoBehaviour
{
    public AnimalData animal;
    
    public void Destroy()
    {
        AnimalDataManager.Instance.RemoveAnimal(animal);
        Destroy(gameObject);
    }

    public void Initialize(AnimalData animal)
    {
        this.animal = animal;
        transform.Find("Width").GetComponent<TextMeshProUGUI>().text = animal.width.ToString();
        transform.Find("Height").GetComponent<TextMeshProUGUI>().text = animal.height.ToString();
        transform.Find("Weight").GetComponent<TextMeshProUGUI>().text = animal.weight.ToString();
        transform.Find("Color").GetComponent<TextMeshProUGUI>().text = animal.color;
        transform.Find("Name").GetComponent<TextMeshProUGUI>().text = animal.name;
    }
}
