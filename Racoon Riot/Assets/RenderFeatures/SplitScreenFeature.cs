using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SplitScreenFeature : ScriptableRendererFeature
{
    private SplitScreenPass _renderPass;

    // Assign your post-process material (which uses your shader) via the Inspector.
    public Material postProcessMaterial;
    private PlayerManager _playerManager;

    // Create() is called only once, so we delay initialization.
    public override void Create()
    {
        // Optionally, you can do minimal setup here.
    }

    // Inject the render pass into URP’s pipeline.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        // Ensure that this is running in play mode and our render pass hasn't been initialized yet.
        if (Application.isPlaying && _renderPass == null)
        {
            _playerManager = UnityEngine.Object.FindFirstObjectByType<PlayerManager>();
            if (_playerManager != null)
            {
                _renderPass = new SplitScreenPass
                {
                    renderPassEvent = RenderPassEvent.AfterRenderingTransparents,
                    postProcessMaterial = postProcessMaterial,
                    playerManager = _playerManager
                };
            }
        }

        if (_renderPass != null)
        {
            renderer.EnqueuePass(_renderPass);
        }
    }
}
