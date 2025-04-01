using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float _lookAtIntensity = 3.0f;
    [SerializeField] private float _smoothSpeed = 10f; // Adjust to control smoothness
    [SerializeField] private GameObject _camera, _locRef, _rotRef;

    private Vector2 _lookAtInput;
    private Vector3 _lookingTarget;
    private Vector2 _smoothedInput = Vector2.zero;

    private void Update()
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
        _smoothedInput = Vector2.Lerp(_smoothedInput, _lookAtInput, Time.deltaTime * _smoothSpeed);

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
            + rightDir * (_lookAtIntensity * adjustedInput.x)
            + upDir * (_lookAtIntensity * adjustedInput.y);

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
