using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputManager))]
public class PlayerManager : MonoBehaviour
{
    [SerializeField] private Transform[] _playerSpawnPoints;
    [SerializeField] private int _playerCount;
    [SerializeField] private GameObject _playerPrefab;

    private GameObject[] _players;
    private PlayerInputManager _playerInputManager;

    // For interaction UI
    private string[] _playerUILayerNames = { "Player1UI", "Player2UI", "Player3UI", "Player4UI" };

    private void Start()
    {
        _playerInputManager = GetComponent<PlayerInputManager>();
        _playerInputManager.playerPrefab = _playerPrefab;
        SpawnPlayers();
    }

    public void SpawnPlayers()
    {
        if (_playerCount > _playerSpawnPoints.Length)
        {
            Debug.LogWarning("Player spawn points exceeds " + _playerCount + " players");
            return;
        }
        
        _players = new GameObject[_playerCount];
        for (var i = 0; i < _playerCount; i++)
        {
            _players[i] = _playerInputManager.JoinPlayer(i)?.gameObject;
            if (_players[i] == null)
            {
                Debug.LogWarning("Failed to spawn player " + i);
                return;
            }
            _players[i].transform.position = _playerSpawnPoints[i].position;
            _players[i].transform.rotation = _playerSpawnPoints[i].rotation;

            //Camera layer configuration

            Camera playerCamera = _players[i].GetComponentInChildren<Camera>();
            if (playerCamera != null)
            {
                int cullingMask = playerCamera.cullingMask;
                int ownUILayer = LayerMask.NameToLayer(_playerUILayerNames[i]);

                for (int layerNameIndex = 0; layerNameIndex < _playerUILayerNames.Length; layerNameIndex++)
                {
                    int currentLayerBit = LayerMask.NameToLayer(_playerUILayerNames[layerNameIndex]);
                    if (currentLayerBit != -1)
                    {
                        if (layerNameIndex == i)
                        {
                            // player's OWN UI layer is visible
                            cullingMask |= (1 << currentLayerBit);
                        }
                        else
                        {
                            // ANOTHER player's UI is hidden
                            cullingMask &= ~(1 << currentLayerBit);
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"The UI Layer '{_playerUILayerNames[layerNameIndex]}' defined in PlayerManager does not exist in Project Settings > Tags and Layers.");
                    }
                }

                playerCamera.cullingMask = cullingMask;
            }
            else
            {
                Debug.LogWarning("Player " + i + " does not have a camera component.");
            }
        }
    }

    public void RespawnPlayer(int playerIndex)
    {
        _players[playerIndex].transform.position = _playerSpawnPoints[playerIndex].position;
        _players[playerIndex].transform.rotation = _playerSpawnPoints[playerIndex].rotation;
    }

    public void DestroyPlayers()
    {
        for (var i = 0; i < _playerCount; i++)
        {
            Destroy(_players[i]);
        }
    }
}
