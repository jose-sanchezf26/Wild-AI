using UnityEngine;

public class ObjectiveUIRenderer : MonoBehaviour
{
    public GameObject uiPrefab;
    public GameObject uiParent;

    public void RenderObjectives()
    {
        foreach (Objective objective in ObjectiveManager.Instance.objectives)
        {
            objective.Initialize(uiPrefab, uiParent);
        }
    }

}
