using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool isInteractable = true;

    public virtual void Interact()
    {
        Debug.Log("Interacting with " + name);
    }
}
