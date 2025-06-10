#ifndef ADDITIONAL_LIGHT_INCLUDED
#define ADDITIONAL_LIGHT_INCLUDED

void MainLight_float(float3 WorldPos, out float3 Direction, out float3 Color, out float Attenuation) 
{
    #ifdef SHADERGRAPH_PREVIEW
        Direction = normalize(float3(1.0f, 1.0f, 0.0f));
        Color = 1.0f;
        Attenuation = 1.0f;
    #else
        Light mainLight = GetMainLight();
        Direction = mainLight.direction;
        Color = mainLight.color;
        Attenuation = mainLight.distanceAttenuation;
    #endif
    return;
}

void AdditionalLight_float(float3 WorldPos, int LightID, out float3 Direction, out float3 Color, out float Attenuation) {
    #ifdef SHADERGRAPH_PREVIEW
        Direction = normalize(float3(1.0f, 1.0f, 0.0f));
        Color = 0.0f;
        Attenuation = 0.0f;
    #else 
        int lightCount = GetAdditionalLightsCount();
        if (LightID < lightCount) {
            Light light = GetAdditionalLight(LightID, WorldPos);
            Direction = light.direction;
            Color = light.color;
            Attenuation = light.distanceAttenuation;
        } else {
            Direction = float3(0,0,0);
            Color = float3(0,0,0);
            Attenuation = 0.0f;
        }
    #endif
    return;
}

void AllAdditionalLights_float(float3 WorldPos, float3 WorldNormal, float2 CutoffThresholds, out float3 LightColor) {
    #ifdef SHADERGRAPH_PREVIEW
        CutoffThresholds = float2(0,0);
        LightColor = float3(0,0,0);
    #else
        int lightCount = GetAdditionalLightsCount();
        for (int i=0; i<lightCount; i++) {
            Light light = GetAdditionalLight(i, WorldPos);
            float3 color = dot(light.direction, WorldNormal);
            color = smoothstep(CutoffThresholds.x, CutoffThresholds.y, color);
            color *= light.color;
            color *= light.distanceAttenuation;

            LightColor += color;
        }
    #endif
}

#endif