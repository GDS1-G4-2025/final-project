using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName; 
    public bool isInteractable = true;

    public virtual void Interact()
    {
        Debug.Log("Interacting with " + itemName);
    }
}
