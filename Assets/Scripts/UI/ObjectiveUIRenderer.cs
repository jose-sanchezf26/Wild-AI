using UnityEngine;

public class ObjectiveUIRendererr : MonoBehaviour
{
    public GameObject uiPrefab;
    public GameObject uiParent;

    void Start()
    {
        foreach (Objective objective in ObjectiveManager.Instance.objectives)
        {
            objective.Initialize(uiPrefab, uiParent);
        }
    }

}
