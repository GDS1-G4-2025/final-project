using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Transform[] playerSpawnPoints;
    public GameObject playerPrefab;
    public int playerCount;
    public int renderTextureResolution = 1024;

    [HideInInspector]
    public RenderTexture[] renderTextures;

    private GameObject[] _players;

    private void Start()
    {
        SpawnPlayers();
    }

    public void SpawnPlayers()
    {

        renderTextures = new RenderTexture[playerCount];
        _players = new GameObject[playerCount];

        for (var i = 0; i < playerCount; i++)
        {
            renderTextures[i] = CreateRenderTexture();

            _players[i] = Instantiate(playerPrefab, playerSpawnPoints[i].position, playerSpawnPoints[i].rotation);

            var cameraRef = _players[i].GetComponent<CameraRef>();
            if (cameraRef != null)
            {
                cameraRef.cam.enabled = true;
                cameraRef.cam.targetTexture = renderTextures[i];
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
            Destroy(renderTextures[i]);
        }
    }

    private RenderTexture CreateRenderTexture()
    {
        // Create a render texture with desired settings
        return new RenderTexture(renderTextureResolution, renderTextureResolution, 24)
        {
            filterMode = FilterMode.Bilinear
        };
    }
}
