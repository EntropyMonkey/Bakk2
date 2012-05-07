// colored vertex lighting
Shader "Simple colored lighting" {
    // a single color property
    Properties {
        _Color ("Main Color", Color) = (1, 0.5, 0.5, 1)
		_HighlightColor ("Highlight Color", Color) = (1, 1, 1, 1)
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }
    // define one subshader
    SubShader {
        Pass {
            Material {
                Diffuse [_Color]
				Ambient (1, 1, 1, 1)
            }
			
			Lighting On
			
			SetTexture [_MainTexture] {
				constantColor [_Color]
				constantColor [_HighlightColor]
				combine constant * constant, constant + constant
			}
            
        }
    }
} 