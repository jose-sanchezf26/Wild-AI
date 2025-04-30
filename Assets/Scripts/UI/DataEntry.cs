using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataEntry : MonoBehaviour
{
    public AnimalData animal;

    public void Destroy()
    {
        AnimalDataSingleton.Instance.RemoveAnimal(animal);
        Destroy(gameObject);
    }

    public void Initialize(AnimalData animal)
    {
        this.animal = animal;
        transform.Find("ID").GetComponent<TextMeshProUGUI>().text = animal.id.ToString();
        transform.Find("Width").GetComponent<TextMeshProUGUI>().text = animal.width.ToString();
        transform.Find("Height").GetComponent<TextMeshProUGUI>().text = animal.height.ToString();
        transform.Find("Weight").GetComponent<TextMeshProUGUI>().text = animal.weight.ToString();
        if (transform.Find("Color") != null)
            transform.Find("Color").GetComponent<TextMeshProUGUI>().text = animal.color;
        if (transform.Find("Level") != null)
            transform.Find("Name").GetComponent<TextMeshProUGUI>().text = animal.name;
    }
}
