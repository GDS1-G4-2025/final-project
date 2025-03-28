using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public Transform[] playerSpawnPoints;
    public int playerCount;
    public GameObject playerPrefab;

    private GameObject[] _players;
    private PlayerInputManager _playerInputManager;

    private void Start()
    {
        _playerInputManager = GetComponent<PlayerInputManager>();
        _playerInputManager.playerPrefab = playerPrefab;
        SpawnPlayers();
    }

    public void SpawnPlayers()
    {
        if (playerCount > playerSpawnPoints.Length)
        {
            Debug.LogWarning("Player spawn points exceeds " + playerCount + " players");
            return;
        }
        _players = new GameObject[playerCount];
        for (var i = 0; i < playerCount; i++)
        {
            _players[i] = _playerInputManager.JoinPlayer(i).gameObject;
            _players[i].transform.position = playerSpawnPoints[i].position;
            _players[i].transform.rotation = playerSpawnPoints[i].rotation;
        }
    }

    public void RespawnPlayer(int playerIndex)
    {
        _players[playerIndex].transform.position = playerSpawnPoints[playerIndex].position;
        _players[playerIndex].transform.rotation = playerSpawnPoints[playerIndex].rotation;
    }

    public void DestroyPlayers()
    {
        for (var i = 0; i < playerCount; i++)
        {
            Destroy(_players[i]);
        }
    }
}
