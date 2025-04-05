using UnityEngine;

public class ItemData : MonoBehaviour
{
    [SerializeField] private InteractableType _itemType;
    public InteractableType ItemType { get; private set; }
}

public enum InteractableType
{
    Food,
    Hazard,
    Payload,
    None
}