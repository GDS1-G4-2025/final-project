Shader "Custom/SplitCamera"
{
    Properties
    {
         _Player1Texture ("Player 1 Texture", 2D) = "white" {}
        _Player2Texture ("Player 2 Texture", 2D) = "white" {}
        _Player3Texture ("Player 3 Texture", 2D) = "white" {}
        _Player4Texture ("Player 4 Texture", 2D) = "white" {}
        _PlayerCount ("Player Count", Range(1, 4)) = 3
        _BorderThickness ("Border Thickness", Range(0.001, 0.02)) = 0.005
        _BorderColor ("Border Color", Color) = (0, 0, 0, 1)
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 100

         Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _Player1Texture;
            sampler2D _Player2Texture;
            sampler2D _Player3Texture;
            sampler2D _Player4Texture;
            float _PlayerCount;
            float _BorderThickness;
            fixed4 _BorderColor;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                //one player, return the texture
                if (_PlayerCount <= 1) {
                    return tex2D(_Player1Texture, i.uv);
                }

                float2 centered_uv = i.uv - 0.5;

                // angle of where we are in the screen
                float angle = atan2(centered_uv.y, centered_uv.x);

                // Convert to 0-2pi
                if (angle < 0) angle += 2 * UNITY_PI;

                // Calculate distance from center
                float dist_from_center = length(centered_uv);

                // Calculate segment angle based on player count
                float segment_angle = 2 * UNITY_PI / _PlayerCount;

                // Determine which player's texture to sample based on the angle
                int player_index = floor(angle / segment_angle);

                // Clamp playerIndex to valid range (just in case)
                player_index = min(player_index, _PlayerCount - 1);

                // Sample the appropriate texture based on player index
                fixed4 col;
                if (player_index == 0) {
                    col = tex2D(_Player1Texture, i.uv);
                }
                else if (player_index == 1) {
                    col = tex2D(_Player2Texture, i.uv);
                }
                else if (player_index == 2) {
                    col = tex2D(_Player3Texture, i.uv);
                }
                else { // playerIndex == 3
                    col = tex2D(_Player4Texture, i.uv);
                }

                // Draw borders between segments
                float angle_mod = fmod(angle, segment_angle);
                float border_width = _BorderThickness * segment_angle;

                // Draw borders between segments and at center
                if ((angle_mod < border_width || segment_angle - angle_mod < border_width) &&
                    dist_from_center > 0.05) {
                    col = _BorderColor;
                }

                // Optional: Draw center point
                if (dist_from_center < 0.05) {
                    col = _BorderColor;
                }

                return col;
            }
            ENDCG
        }
    }
}
