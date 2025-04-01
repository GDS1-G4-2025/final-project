using UnityEngine;

public class RoombaVacuum : MonoBehaviour
{
    [SerializeField] private Transform _vacuumLocation;
    [SerializeField] private float sizeThreshold = 1.0f;

    private void OnCollisionEnter(Collision collision)
    {
        // If other object is not a rigidbody then don't vacuum it
        if (collision.rigidbody == null) return;

        float objectSize = collision.transform.localScale.magnitude;

        if (objectSize <= sizeThreshold)
        {
            collision.transform.position = _vacuumLocation.position;
        }

    }
}
