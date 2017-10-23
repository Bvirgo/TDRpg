
Shader "AngryBots/FX/Additive" {
	Properties {
		_MainTex ("Base", 2D) = "white" {}
		_TintColor ("TintColor", Color) = (1.0, 1.0, 1.0, 1.0)
	}
    Category {
        ZWrite Off
        Lighting On
        Fog {Mode Off}
        Tags {Queue=Transparent}
        Blend One One

        SubShader 
  	       {
            Pass {
            Cull Off
            Material {
                Emission [_TintColor]
            }
            SetTexture [_MainTex] {
                Combine texture * primary, texture + primary 
            } 
        }
    }
 } 
}