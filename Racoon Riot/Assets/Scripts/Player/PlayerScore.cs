using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    [SerializeField] public float score;

    public void AddPoints(int points)
    {
        score += points;
    }

    public void RemovePoints(int points)
    {
        score -= points;
    }

    public void Reset()
    {
        score = 0;
    }
}
