using UnityEngine;
using UnityEngine.EventSystems;

public class AnimalWeightScaleDrop : MonoBehaviour, IDropHandler
{
    public AnimalData animalData;

    public void OnDrop(PointerEventData eventData)
    {
        WeightScale weightScale = eventData.pointerDrag.GetComponent<WeightScale>();
        if (weightScale != null)
        {
            weightScale.numberImage.SetActive(true);
            weightScale.UpdateText(animalData.weight);
        }
    }
}
