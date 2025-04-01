using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    [SerializeField] private float _totalTime = 600f;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private float _remainingTime;

    private string _sceneToLoad = "Podium"; // Name of the scene to load

    public static event Action GameOverEvent;
    private bool _hasGameEnded = false;

    private void Start()
    {
        _remainingTime = _totalTime;
        UpdateTimerDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        if (_hasGameEnded) return;

        _remainingTime -= Time.deltaTime;

        if (_remainingTime <= 0f)
        {
            EndGame();
        }
        else
        {
            UpdateTimerDisplay();
        }
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(_remainingTime / 60);
        int seconds = Mathf.FloorToInt(_remainingTime % 60);
        _timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void EndGame()
    {
        _hasGameEnded = true;

        // Notify listeners about Game Over
        if (GameOverEvent != null)
            GameOverEvent.Invoke();

        // Switch to the Game Over scene
        SceneManager.LoadScene(_sceneToLoad);
    }
}
