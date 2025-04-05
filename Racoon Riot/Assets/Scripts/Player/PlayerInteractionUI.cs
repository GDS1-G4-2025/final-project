using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerInteractionUI : MonoBehaviour
{

    [SerializeField] private GameObject _promptPanel;
    [SerializeField] private Image _upPromptImage;
    [SerializeField] private Image _westPromptImage;
    [SerializeField] private Image _eastPromptImage;
    [SerializeField] private Vector3 _targetOffset = new Vector3(0, 30, 0);
    [SerializeField] private Transform _heldObjectAnchor;
   
    private RectTransform _panelRectTransform;
    private GameObject _currentTargetObject;
    private Pickupable _currentHeldObject;
    private PlayerPickupThrow _playerPickupThrow;
    private Camera _playerCamera;
    private Collider _col;
    private Coroutine _disablePromptsCoroutine;

    void Awake()
    {
        _playerCamera = GetComponentInChildren<Camera>();
        _playerPickupThrow = GetComponent<PlayerPickupThrow>();
        _col = GetComponent<Collider>();
        _panelRectTransform = _promptPanel.GetComponent<RectTransform>();

        HideAllPrompts();
    }

    void OnEnable()
    {
        if (_playerPickupThrow != null)
        {
            // Subscribe the HandleHeldObjectChanged method to the event
            _playerPickupThrow.OnHeldObjectChanged += HandleHeldObjectChanged;
            HandleHeldObjectChanged(_playerPickupThrow.HeldObject);
        }
    }

    void OnDisable()
    {
        if (_playerPickupThrow != null)
        {
            _playerPickupThrow.OnHeldObjectChanged -= HandleHeldObjectChanged;
        }
    }

    private void HandleHeldObjectChanged(Pickupable newHeldItem)
    {
        Debug.Log($"UIManager received held object change: {(newHeldItem != null ? newHeldItem.name : "null")}");
        _currentHeldObject = newHeldItem;
        UpdatePromptsVisibility();
    }

    void Update()
    {
        UpdatePromptsVisibility();

        if (_promptPanel.activeSelf)
        {
            PositionPromptPanel();
        }
    }

    private void PositionPromptPanel()
    {
        Vector3 screenPos;
        Vector3 worldPositionAnchor;

        // Determine what the UI should anchor to
        if (_currentHeldObject != null && _heldObjectAnchor != null)
        {
            worldPositionAnchor = _heldObjectAnchor.position;
            screenPos = _playerCamera.WorldToScreenPoint(worldPositionAnchor);
            screenPos += _targetOffset;
        }
        else if (_currentTargetObject != null)
        {
            // If not holding, anchor near the target object
            worldPositionAnchor = _currentTargetObject.transform.position;
            screenPos = _playerCamera.WorldToScreenPoint(worldPositionAnchor);
            screenPos += _targetOffset;
        }
        else
        {
            _promptPanel.SetActive(false);
            return;
        }

        if (screenPos.z > 0)
        {
            _panelRectTransform.position = screenPos;
            if (!_promptPanel.activeSelf) _promptPanel.SetActive(true);
        }
        else
        {
            _promptPanel.SetActive(false);
        }
    }

    public void SetPotentialTarget(GameObject target)
    {
        if (_currentTargetObject != target)
        {
            _currentTargetObject = target;
            UpdatePromptsVisibility();
        }
    }

    public void RemovePotentialTarget(GameObject target)
    {
        if (_currentTargetObject == target)
        {
            _currentTargetObject = null;
            UpdatePromptsVisibility();
        }
    }

    private void UpdatePromptsVisibility()
    {
        if (_currentTargetObject == null && _currentHeldObject == null)
        {
            HideAllPrompts();
            return;
        }

        bool showUp = false;
        bool showWest = false;
        bool showEast = false;

        if (_currentHeldObject != null)
        {
            Debug.Log($"[InteractionUI] Checking held object: {_currentHeldObject.name}");
            Throwable throwable = _currentHeldObject.GetComponent<Throwable>();
            Debug.Log($"[InteractionUI] Found Throwable component: {(throwable != null ? "YES" : "NO")}");
            if (throwable != null)
            {
                showEast = true;
                Debug.Log("[InteractionUI] Setting showEast = true");
            }
        }
        else if (_currentTargetObject != null)
        {
            Interactable interactable = _currentTargetObject.GetComponent<Interactable>();

            if (interactable != null)
            {
                Destructible destructible = _currentTargetObject.GetComponent<Destructible>();

                //Destructible
                if (destructible != null)
                {
                    showWest = true;
                }
                //Interactable
                else
                {
                    showUp = true;
                }
            }
        }

        //Apply to UI
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

    private IEnumerator DisablePromptsForSeconds(float seconds)
    {
        HideAllPrompts();
        yield return new WaitForSeconds(seconds);
        UpdatePromptsVisibility();
    }
}
