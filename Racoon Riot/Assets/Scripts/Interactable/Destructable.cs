using UnityEngine;

[RequireComponent(typeof(Fracture))]
public class Destructable : Interactable
{
    private Fracture _fracture;

    private void Awake()
    {
        _fracture = GetComponent<Fracture>();
        if (_fracture == null)
        {
            Debug.LogError("Fracture component not found on " + gameObject.name);
        }
    }

    public void Destroy()
    {
        if (_fracture != null)
        {
            _fracture.CauseFracture();
        }
        else
        {
            Debug.LogWarning("Fracture component is missing. Cannot destroy.");
        }
    }
}
