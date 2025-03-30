using UnityEngine;

public class PickableItem : Item
{
    public override void Interact()
    {
        base.Interact();
        Debug.Log(itemName + " has been picked up!");
    }
}
