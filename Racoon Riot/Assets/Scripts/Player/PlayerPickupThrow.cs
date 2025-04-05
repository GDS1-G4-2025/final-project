using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;

public class PlayerPickupThrow : MonoBehaviour
{
    public event Action<Pickupable> OnHeldObjectChanged;

    [SerializeField] private Pickupable _heldObject = null;

    public Pickupable HeldObject
    {
        get => _heldObject;
        set
        {
            if (_heldObject != value)
            {
                Pickupable previousHeldObject = _heldObject;
                _heldObject = value;
                Debug.Log($"[PickupThrow] Held object changing. New object: {(_heldObject != null ? _heldObject.name : "null")}");
                OnHeldObjectChanged?.Invoke(_heldObject);
            }
        }
    }

    [SerializeField] private Pickupable _pickUpTarget;
    [SerializeField] private GameObject _itemHolder; // Child object of player to hold items

    // Interaction UI
    [SerializeField] private PlayerInteractionUI _playerInteractionUI;

    // Interaction
    private bool _canInteract = true;
    [SerializeField] private float _interactCooldown = 0.3f; // Time between interactions
    [SerializeField] private float _placeDistance = 1.0f; // Distance to place the object in front of the player
    [SerializeField] private float _throwStartPositionOffset = 0.9f;

    // Ignoring thrown object
    private Pickupable _recentlyThrownObject = null;
    private Coroutine _clearRecentlyThrownCoroutine = null;
    [SerializeField] private float _ignoreThrownObjectDuration = 0.5f;

    private void Awake()
    {
        _playerInteractionUI = GetComponent<PlayerInteractionUI>();
    }

    public bool IsHoldingObject()
    {
        return _heldObject != null;
    }

    public void OnObjectPickupDrop(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started && _canInteract)
        {
            _canInteract = false;

            if (IsHoldingObject())
            {
                AttemptDrop();
            }
            else
            {
                AttemptPickup();
            }

            StartCoroutine(InteractCooldownTimer());
        }
    }

    private IEnumerator InteractCooldownTimer()
    {
        yield return new WaitForSeconds(_interactCooldown);
        _canInteract = true;
    }

    public void OnObjectThrow(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started && _canInteract && IsHoldingObject())
        {
            _canInteract = false;
            AttemptThrow();
            StartCoroutine(InteractCooldownTimer());
        }
        else if (ctx.phase == InputActionPhase.Started && !_canInteract)
        {
            //Debug.Log("Throw attempt blocked by cooldown.");
        }
        else if (ctx.phase == InputActionPhase.Started && !IsHoldingObject())
        {
            //Debug.Log("Throw attempt failed: Not holding an object.");
        }

    }

    private void AttemptPickup()
    {
        if (_pickUpTarget == null)
        {
            //Debug.Log("Attempted pickup but no target.");
            return;
        }
        if (IsHoldingObject())
        {
            //Debug.Log("Attempted pickup but already holding object.");
            return;
        }

        //Debug.Log("Picking up: " + _pickUpTarget.name);

        HeldObject = _pickUpTarget;

        if (HeldObject != null)
        {
            HeldObject.AttachTo(_itemHolder);
        }
        else
        {
            Debug.LogError("HeldObject became null unexpectedly during pickup!", this);
        }

        _pickUpTarget = null;
    }

    private void AttemptDrop()
    {
        if (HeldObject == null) return;

        Debug.Log("Dropping: " + HeldObject.name);
        Pickupable objectToDrop = HeldObject;

        HeldObject = null;

        objectToDrop.Drop(transform, _placeDistance);
    }

    private void AttemptThrow()
    {
        if (HeldObject == null) {return;}

        if (HeldObject.TryGetComponent<Throwable>(out Throwable throwable))
        {
            //Debug.Log("Attempting to throw: " + HeldObject.name);

            Pickupable objectToThrow = HeldObject;

            HeldObject = null;
            _pickUpTarget = null;

            _recentlyThrownObject = objectToThrow;
            if (_clearRecentlyThrownCoroutine != null)
            {
                StopCoroutine(_clearRecentlyThrownCoroutine);
            }
            _clearRecentlyThrownCoroutine = StartCoroutine(ClearRecentlyThrownTimer(_ignoreThrownObjectDuration));

            // Perform throw physics
            Vector3 playerPos = transform.position;
            Vector3 playerForward = transform.forward;
            Vector3 startPosition = playerPos + playerForward.normalized * _throwStartPositionOffset;
            objectToThrow.transform.position = startPosition;
            objectToThrow.transform.rotation = transform.rotation;
            throwable.Throw(transform.forward);
        }
        else
        {
            Debug.LogWarning($"{HeldObject.name} is not Throwable. Cannot throw. Performing Drop action instead.");
            // Fallback: Drop
            AttemptDrop();
        }
    }

    private IEnumerator ClearRecentlyThrownTimer(float delay)
    {
        yield return new WaitForSeconds(delay);
        // After the delay, stop ignoring the object
        if (_recentlyThrownObject != null)
        {
            //Debug.Log($"[PickupThrow] No longer ignoring recently thrown object: {_recentlyThrownObject.name}");
        }
        _recentlyThrownObject = null;
        _clearRecentlyThrownCoroutine = null;
    }

    public void OnTriggerEnter(Collider other)
    {
        // Only look for a target if not holding something
        if (!IsHoldingObject() && other.gameObject.TryGetComponent(out Pickupable pickupable))
        {
            if (_recentlyThrownObject != null && pickupable == _recentlyThrownObject)
            {
                return; // Exit early, do not set as target
            }
            if (_pickUpTarget == null)
            {
                _pickUpTarget = pickupable;
                _playerInteractionUI.SetPotentialTarget(_pickUpTarget.gameObject);
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (_pickUpTarget != null && _pickUpTarget.gameObject == other.gameObject)
        {
            _playerInteractionUI.RemovePotentialTarget(_pickUpTarget.gameObject);
            _pickUpTarget = null;
        }
        else if (_recentlyThrownObject != null && _recentlyThrownObject.gameObject == other.gameObject)
        {
            _recentlyThrownObject = null;
            if (_clearRecentlyThrownCoroutine != null)
            {
                StopCoroutine(_clearRecentlyThrownCoroutine);
                _clearRecentlyThrownCoroutine = null;
            }
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        if (!IsHoldingObject() && _pickUpTarget == null && other.gameObject.TryGetComponent(out Pickupable pickupable))
        {
            if (_recentlyThrownObject != null && pickupable == _recentlyThrownObject) return;
            _pickUpTarget = pickupable;
            _playerInteractionUI.SetPotentialTarget(_pickUpTarget.gameObject);
            //Debug.Log($"[PickupThrow] OnCollisionEnter - SET _pickUpTarget to: {(_pickUpTarget != null ? _pickUpTarget.name : "null")} from {other.gameObject.name}");
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (_pickUpTarget != null && _pickUpTarget.gameObject == other.gameObject)
        {
            _playerInteractionUI.RemovePotentialTarget(_pickUpTarget.gameObject);
            //Debug.Log($"[PickupThrow] OnCollisionExit - CLEARED _pickUpTarget ({_pickUpTarget.name}) because {other.gameObject.name} exited.");
            _pickUpTarget = null;
        }
    }
}