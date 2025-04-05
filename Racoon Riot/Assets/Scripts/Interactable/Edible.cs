using System.Collections;
using UnityEngine;

public class Edible : Pickupable
{
    [SerializeField] private int healAmount = 30;
    [SerializeField] private float consumeDuration = 3f;

    private Coroutine consumeRoutine;
    private GameObject currentHolder;
    private PlayerHealth _playerHealth;

    public override void Interact()
    {
        base.Interact();

        if (consumeRoutine == null)
        {
            currentHolder = transform.parent?.gameObject;
            if (currentHolder != null)
            {
                consumeRoutine = StartCoroutine(ConsumeAfterDelay(currentHolder));
            }
        }
    }

    private IEnumerator ConsumeAfterDelay(GameObject holder)
    {
        float timer = 0f;

        while (timer < consumeDuration)
        {
            if (transform.parent == null || transform.parent.gameObject != holder)
            {
                Debug.Log($"{name} was dropped/stolen before it could be consumed.");
                consumeRoutine = null;
                yield break;
            }

            timer += Time.deltaTime;
            yield return null;
        }


        if (_playerHealth != null)
        {
            _playerHealth.RestoreHealth(healAmount);
            Debug.Log($"{name} consumed after {consumeDuration} seconds. Restored {healAmount} HP.");
        }
        // else
        // {
        //     Debug.LogWarning("No PlayerHealth component found on holder.");
        // }

        Destroy(gameObject);
    }

    public override void Drop(Transform dropperTransform, float placeDistance)
    {
        base.Drop(dropperTransform, placeDistance);
        if (consumeRoutine != null)
        {
            StopCoroutine(consumeRoutine);
            consumeRoutine = null;
            Debug.Log($"{name} dropped. Consumption canceled.");
        }

        currentHolder = null;
    }

    public void SetHolder(GameObject holder)
    {
        Debug.Log($"Setting holder: {holder.name}");  // Logs the name of the object being passed

        currentHolder = holder;
        _playerHealth = holder.GetComponent<PlayerHealth>();

        if (_playerHealth == null)
        {
            Debug.LogWarning($"No PlayerHealth component found on {holder.name}. Make sure PlayerHealth is attached.");
        }
        else
        {
            Debug.Log($"PlayerHealth found on {holder.name}. Health: {_playerHealth.health}");
        }
    }
}
