using UnityEngine;
using System.Collections;
using System;

public class PlayerSnapping : MonoBehaviour
{
    [Header("Default Snapping Settings")]
    [SerializeField] private float _snapDuration = 0.15f;
    [SerializeField] private float _snapDistance = 1.0f;
    [SerializeField] private bool _enablePositionSnap = true;

    //[SerializeField] private float _snapRotationSpeed = 360f;
    //[SerializeField] private float _snapMoveSpeed = 5f;

    private Coroutine _activeSnapCoroutine = null;
    private bool _isSnapping = false;
    public bool IsSnapping => _isSnapping;

    public void StartSnap(Transform target, Action onSnapComplete = null)
    {
        if (target == null)
        {
            Debug.LogError("Target transform is null. Cannot start snapping.");
            return;
        }

        if (_isSnapping)
        {
            StopCurrentSnap();
            Debug.LogWarning("Already snapping. Cannot start a new snap.");
            return;
        }

        _activeSnapCoroutine = StartCoroutine(SnapCoroutine(target, _snapDuration, _enablePositionSnap, _snapDistance, onSnapComplete));
    }

    private void StopCurrentSnap()
    {
        if (_activeSnapCoroutine != null)
        {
            StopCoroutine(_activeSnapCoroutine);
            _activeSnapCoroutine = null;
        }
        _isSnapping = false;
    }

    private IEnumerator SnapCoroutine (Transform target, float duration, bool enablePositionSnap, float snapDistance, Action onSnapComplete = null)
    {
        _isSnapping = true;

        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;
        float elapsedTime = 0f;

        Vector3 initialTargetPosition = target.position;
        Vector3 initialDirectionToTarget = initialTargetPosition - startPosition;
        initialDirectionToTarget.y = 0;

        Quaternion finalTargetRotation = startRotation;
        Vector3 finalTargetPosition = startPosition;

        if (initialDirectionToTarget.sqrMagnitude > 0.001f)
        {
            finalTargetRotation = Quaternion.LookRotation(initialDirectionToTarget.normalized, Vector3.up);

            if (_enablePositionSnap)
            {
                finalTargetPosition = initialTargetPosition - initialDirectionToTarget.normalized * snapDistance;
                finalTargetPosition.y = startPosition.y;
            } 
            else
            {
                finalTargetPosition = startPosition;
            }
        }
        else
        {
            finalTargetRotation = startRotation;
            finalTargetPosition = startPosition;
            _enablePositionSnap = false;
            Debug.Log("Player already at target horizontal position. Skipping snap movement/rotation.");
        }

        while (elapsedTime < duration)
        {
            if (target == null || !target.gameObject.activeInHierarchy)
            {
                Debug.Log("Target transform is null or inactive. Stopping snap.");
                _isSnapping = false;
                _activeSnapCoroutine = null;
                yield break;
            }

            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            transform.rotation = Quaternion.Slerp(startRotation, finalTargetRotation, t);

            if (_enablePositionSnap)
            {
                GetComponent<Rigidbody>().MovePosition(Vector3.Lerp(startPosition, finalTargetPosition, t));
            }
            yield return null;
        }

        _isSnapping = false;
        _activeSnapCoroutine = null;

        if (target != null)
        {
            transform.rotation = finalTargetRotation;
            if (_enablePositionSnap)
            {
                GetComponent<Rigidbody>().MovePosition(finalTargetPosition);
            }

            onSnapComplete?.Invoke();
        }
    }
}
