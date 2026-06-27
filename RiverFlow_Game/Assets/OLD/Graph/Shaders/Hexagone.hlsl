//Reference : https://www.shadertoy.com/view/fdtGDH
void HexagoneGridSDF_float(float2 UV, out float result)
{
    UV *= 2.0;
    UV.y += 0.125;
    
    UV /= float2(sqrt(3.0), 1.5);
    
    UV.y -= 0.25;
    UV.x -= frac(floor(UV.y) * 0.5);
    
    UV = abs(frac(UV) - 0.5);
    
    result = abs(1.0 - max(UV.x + UV.y * 1.5, UV.x * 2.0)) * sqrt(3.0) * 0.5;
}

//Reference : https://www.shadertoy.com/view/fdtGDH
void Hexagone_float(float2 UV, out float2 hexaUV, out float2 hexaIndex)
{
    //sqrt(3) = 1.73205080757
    const float2 s = float2(1.7320508, 1);

    float4 hC = floor(float4(UV, UV - float2(1, .5)) / s.xyxy) + .5;
    // Centering the coordinates with the hexagon centers above.
    float4 h = float4(UV - hC.xy * s, UV - (hC.zw + .5) * s);

    
    float4 result = dot(h.xy, h.xy) < dot(h.zw, h.zw)
                    ? float4(h.xy, hC.xy)
                    : float4(h.zw, hC.zw + .5);
    
    hexaUV = result.xy;
    hexaIndex = result.zw;
}

//Reference : https://www.shadertoy.com/view/fd3SRf
float sdfHexagon(float2 UV, float size, float radius)
{
    float circleTau = 6.28318530;
    float2x2 rot = float2x2(cos(circleTau / 6.), sin(circleTau / 6.), -sin(circleTau / 6.), cos(circleTau / 6.));
    UV = mul(rot, UV);
    
    const float3 k = float3(-0.866025404, 0.5, 0.577350269);
    UV = abs(UV);
    UV -= 2.0 * min(dot(k.xy, UV), 0.0) * k.xy;
    UV -= float2(clamp(UV.x, -k.z * size, k.z * size), size);
    return length(UV) * sign(UV.y) - radius;
}
void HexagoneSDF_float(float2 UV, out float result)
{
    result = 2 * frac(sdfHexagon(UV, 1., 0.));
}