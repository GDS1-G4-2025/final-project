using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Camera mainCamera;
    public Transform[] playerSpawnPoints;
    public GameObject playerPrefab;
    public int playerCount;

    private GameObject[] _players;
    private Camera[] _playerCameras;
    private RenderTexture[] _renderTextures;

    private Material _splitScreenMaterial;

    private void Start()
    {

    }

    public void SpawnPlayers()
    {
        _players = new GameObject[playerCount];
        for (var i = 0; i < playerCount; i++)
        {
            _players[i] = Instantiate(playerPrefab, playerSpawnPoints[i].position, playerSpawnPoints[i].rotation);

            var cameraRef = _players[i].GetComponent<CameraRef>();
            if (cameraRef != null)
            {
                cameraRef.cam.enabled = true;
            }
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
