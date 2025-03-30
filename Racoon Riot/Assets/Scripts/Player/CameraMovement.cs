using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    public float lookAtIntensity = 3.0f;
    public float smoothSpeed = 10f; // Adjust to control smoothness

    private Vector2 _lookAtInput;
    private Vector3 _lookingTarget;
    private Vector2 _smoothedInput = Vector2.zero;

    [SerializeField] private GameObject _camera, _locRef, _rotRef;

    void Update()
    {
        // Camera position logic
        if (Physics.Linecast(transform.position, _locRef.transform.position, out var hit))
        {
            _camera.transform.position = new Vector3(
                Mathf.Lerp(transform.position.x, hit.point.x, 0.9f),
                Mathf.Lerp(transform.position.y, hit.point.y, 0.9f),
                Mathf.Lerp(transform.position.z, hit.point.z, 0.9f)
            );
        }
        else
        {
            _camera.transform.position = _locRef.transform.position;
        }

        // Input smoothing
        _smoothedInput = Vector2.Lerp(_smoothedInput, _lookAtInput, Time.deltaTime * smoothSpeed);

        // Input magnitude + non-linear adjustment
        var normalizedInput = _smoothedInput.magnitude > 1 ? _smoothedInput.normalized : _smoothedInput;
        var inputMagnitude = normalizedInput.magnitude;
        var adjustedMagnitude = Mathf.Pow(inputMagnitude, 3);
        var adjustedInput = normalizedInput * adjustedMagnitude;

        // Get the right and up vectors of the player/reference object
        var rightDir = _rotRef.transform.right;
        var upDir = _rotRef.transform.up;

        // Calculate the look target relative to the player's orientation
        _lookingTarget = _rotRef.transform.position
            + rightDir * (lookAtIntensity * adjustedInput.x)
            + upDir * (lookAtIntensity * adjustedInput.y);

        _camera.transform.LookAt(_lookingTarget);
    }

    public void OnLookAt(InputAction.CallbackContext ctx)
    {
        _lookAtInput = ctx.ReadValue<Vector2>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_lookingTarget, 0.1f);
    }
}
