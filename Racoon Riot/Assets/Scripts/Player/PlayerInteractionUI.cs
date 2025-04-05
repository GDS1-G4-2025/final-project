using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractionUI : MonoBehaviour
{

    [SerializeField] private GameObject _promptPanel;
    [SerializeField] private Image _upPromptImage;
    [SerializeField] private Image _westPromptImage;
    [SerializeField] private Image _eastPromptImage;
    [SerializeField] private Vector3 _offset = new Vector3(0, 30, 0);

    private GameObject _currentTargetObject;
    private Camera _playerCamera;
    private PlayerPickupThrow _playerPickupThrow;
    private Collider _col;

    void Awake()
    {
        _playerCamera = GetComponentInChildren<Camera>();
        _playerPickupThrow = GetComponent<PlayerPickupThrow>();
        _col = GetComponent<Collider>();

        HideAllPrompts();
    }

    void Update()
    {
        if (_currentTargetObject != null && _currentTargetObject.activeInHierarchy && _playerCamera != null)
        {
            UpdatePromptsVisibility();

            Vector3 screenPos = _playerCamera.WorldToScreenPoint(_currentTargetObject.transform.position);
            // Show prompt if object is in front of camera
            if (screenPos.z > 0)
            {
                _promptPanel.SetActive(true);
                _promptPanel.transform.position = screenPos + _offset;
            }
            else
            {
                _promptPanel.SetActive(false);
            }
        }
        else
        {
            if (_promptPanel.activeSelf)
            {
                HideAllPrompts();
            }
        }
    }

    public void SetPotentialTarget(GameObject target)
    {
        _currentTargetObject = target;
        if (_currentTargetObject != null)
        {
            _promptPanel.SetActive(true);
            UpdatePromptsVisibility();
        }
        else
        {
            HideAllPrompts();
        }
    }

    public void RemovePotentialTarget(GameObject target)
    {
        if (_currentTargetObject == target)
        {
            _currentTargetObject = null;
            HideAllPrompts();
        }
    }

    public void NotifyHeldObjectChanged()
    {
        if (_currentTargetObject != null)
        {
            UpdatePromptsVisibility();
        }
    }

    private void UpdatePromptsVisibility()
    {
        if (_currentTargetObject == null || _playerPickupThrow == null)
        {
            HideAllPrompts();
            return;
        }

        bool showUp = false;
        bool showWest = false;
        bool showEast = false;

        Interactable interactable = _currentTargetObject.GetComponent<Interactable>();
        Pickupable pickupable = _currentTargetObject.GetComponent<Pickupable>();
        Throwable throwable = _currentTargetObject.GetComponent<Throwable>();

        // Pickupable
        if (interactable != null && !(interactable is Destructible))
        {
            if (pickupable == null || !_playerPickupThrow.IsHoldingObject() || _playerPickupThrow.heldObject != pickupable)
            {
                showUp = true;
            }
        }

        // Dropable
        if (_playerPickupThrow.IsHoldingObject() && _playerPickupThrow.heldObject == pickupable && _playerPickupThrow.heldObject != throwable)
        {
            showUp = true; // Use 'Up' for Drop in your input setup
        }

        Destructible destructible = _currentTargetObject.GetComponent<Destructible>();

        // Destructible
        if (destructible != null && destructible.destructionEnabled)
        {
            showWest = true;
            showUp = false;
            showEast = false;
        }

        // Throwable
        if (_playerPickupThrow.IsHoldingObject() && _playerPickupThrow.heldObject == _currentTargetObject)
        {
            if (throwable != null)
            {
                showEast = true;
                showUp = false;
            }
        }

        //Apply

        _upPromptImage.gameObject.SetActive(showUp);
        _westPromptImage.gameObject.SetActive(showWest);
        _eastPromptImage.gameObject.SetActive(showEast);

        if (!showUp && !showWest && !showEast)
        {
            _promptPanel.SetActive(false);
        }
        else
        {
            _promptPanel.SetActive(true); // Ensure panel is visible if any prompt is
        }
    }

    private void HideAllPrompts()
    {
       _promptPanel.SetActive(false);
    }

    public void OnTriggerEnter(Collider other)
    {
        _currentTargetObject = other.gameObject;
        UpdatePromptsVisibility();
    }

    public void OnTriggerExit(Collider other)
    {
        _currentTargetObject = other.gameObject;
        UpdatePromptsVisibility();
    }
}
