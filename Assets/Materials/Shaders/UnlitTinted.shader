// Identical to Unlit Transparent, except you can define a color
Shader "Unlit/Unlit Tinted" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
}

SubShader {
	//Tags { "ForceSupported" = "True" "RenderType"="Overlay" }
	
	Lighting Off
	Blend SrcAlpha OneMinusSrcAlpha 
	//Cull Off 
	//ZWrite Off
	Fog { Mode Off } 
	//ZTest Always 
	
	BindChannels { 
		Bind "vertex", vertex 
		Bind "color", color 
		Bind "TexCoord", texcoord 
	} 
		
	Pass { 
		SetTexture [_MainTex] {
		    constantColor [_Color]
			combine constant * texture, constant * texture
		} 
	} 
}

Fallback "Unlit/Transparent"
}