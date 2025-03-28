using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

public class SplitScreenPass : ScriptableRenderPass
{
    private static readonly int PlayerCount = Shader.PropertyToID("_PlayerCount");
    private class PassData { }

    public Material postProcessMaterial;
    public PlayerManager playerManager;

    public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
    {
        string passName = "Custom Shader Render Pass";
        using (var builder = renderGraph.AddRasterRenderPass<PassData>(passName, out var passData))
        {
            // Get the camera data (casting because of ContextContainer limitations)
            UniversalCameraData cameraData = (UniversalCameraData)frameData.Get<UniversalCameraData>();
            RenderTextureDescriptor desc = cameraData.cameraTargetDescriptor;
            desc.msaaSamples = 1;
            desc.depthBufferBits = 0;

            // Create a temporary destination texture.
            TextureHandle destination = UniversalRenderer.CreateRenderGraphTexture(renderGraph, desc, "CustomShaderDestination", false);

            // Declare that this pass writes to the destination.
            builder.SetRenderAttachment(destination, 0);
            builder.AllowPassCulling(false);

            builder.SetRenderFunc((PassData data, RasterGraphContext context) =>
            {
                // Update the material with the textures and player count from PlayerManager.
                if (playerManager && postProcessMaterial)
                {
                    for (var i = 0; i < playerManager.playerCount; i++)
                    {
                        postProcessMaterial.SetTexture($"_Player{i+1}Texture", playerManager.renderTextures[i]);
                    }
                    postProcessMaterial.SetInt(PlayerCount, playerManager.playerCount);
                }

                // Use the overload that doesn't require a source texture,
                // so we don't read from the destination texture.
                Blitter.BlitTexture(context.cmd, new Vector4(1, 1, 0, 0), postProcessMaterial, 0);
            });
        }
    }
}
