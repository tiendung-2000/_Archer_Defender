using Sirenix.OdinInspector;
using UnityEngine;

public class RemoveComponent : MonoBehaviour
{
    public GameObject parentObject;
    public string componentName;

    [Button("Remove")]
    private void RemoveComponents()
    {
        Component[] components = parentObject.GetComponentsInChildren<Component>();

        int removedCount = 0;

        foreach (Component component in components)
        {
            if (component.GetType().Name == componentName)
            {
                DestroyImmediate(component);
                removedCount++;
            }
        }

       Debug.Log($"Removed {removedCount} instances of {componentName} from {parentObject.name} and its children.");
    }
}
