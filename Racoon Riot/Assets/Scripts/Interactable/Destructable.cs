using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Fracture))]
public class Destructable : Interactable
{
    private Fracture _fracture;
    private bool _isDestroyed = false;

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
        if (_fracture != null && !_isDestroyed)
        {
            _isDestroyed = true;
            _fracture.CauseFracture();
        }
        else
        {
            //Debug.Log("Fracture component is missing. Cannot destroy.");
        }
    }
}
