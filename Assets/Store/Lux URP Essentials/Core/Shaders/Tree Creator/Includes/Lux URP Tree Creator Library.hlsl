#ifndef LUXLWRP_TREELIBRARY_INCLUDED
#define LUXLWRP_TREELIBRARY_INCLUDED

float LuxScreenDitherToAlpha(float x, float y, float c0)
{
#if (SHADER_TARGET > 30) || defined(SHADER_API_D3D11) || defined(SHADER_API_GLCORE) || defined(SHADER_API_GLES3)
    //dither matrix reference: https://en.wikipedia.org/wiki/Ordered_dithering
    const float dither[64] = {
        0, 32, 8, 40, 2, 34, 10, 42,
        48, 16, 56, 24, 50, 18, 58, 26 ,
        12, 44, 4, 36, 14, 46, 6, 38 ,
        60, 28, 52, 20, 62, 30, 54, 22,
        3, 35, 11, 43, 1, 33, 9, 41,
        51, 19, 59, 27, 49, 17, 57, 25,
        15, 47, 7, 39, 13, 45, 5, 37,
        63, 31, 55, 23, 61, 29, 53, 21 };

    int xMat = int(x) & 7;
    int yMat = int(y) & 7;

    float limit = (dither[yMat * 8 + xMat] + 11) / 64.0;

    return saturate( c0 * (1 + c0) - limit - 0.01h);

#else
    return 1.0;
#endif
}

float LuxComputeAlphaCoverage(float4 screenPos, float fadeAmount)
{
#if (SHADER_TARGET > 30) || defined(SHADER_API_D3D11) || defined(SHADER_API_GLCORE) || defined(SHADER_API_GLES3)
    float2 pixelPosition = screenPos.xy / (screenPos.w + 0.00001);
    pixelPosition *= _ScreenParams.xy;
    float coverage = LuxScreenDitherToAlpha(pixelPosition.x, pixelPosition.y, fadeAmount);
    return coverage;
#else
    return 1.0;
#endif
}

inline float3 Squash(in float3 pos)
{
    float3 planeNormal = UNITY_ACCESS_INSTANCED_PROP(Props, _SquashPlaneNormal).xyz;
    float3 projectedVertex = pos.xyz - (dot(planeNormal.xyz, pos.xyz) + UNITY_ACCESS_INSTANCED_PROP(Props, _SquashPlaneNormal).w) * planeNormal;
    pos = float3(lerp(projectedVertex, pos.xyz, UNITY_ACCESS_INSTANCED_PROP(Props, _SquashAmount)));
    return pos;
}

float4 SmoothCurve( float4 x ) {
    return x * x *( 3.0 - 2.0 * x );
}
float4 TriangleWave( float4 x ) {
    return abs( frac( x + 0.5 ) * 2.0 - 1.0 );
}
float4 SmoothTriangleWave( float4 x ) {
    return SmoothCurve( TriangleWave( x ) );
}

half2 SmoothCurve( half2 x ) {   
    return x * x *( 3.0h - 2.0h * x );   
}
half2 TriangleWave( half2 x ) {   
    return abs( frac( x + 0.5h ) * 2.0h - 1.0h );   
}
half2 SmoothTriangleWave( half2 x ) {   
    return SmoothCurve( TriangleWave( x ) );   
}

float4 _LuxURPWindDirSize;
float4 _LuxURPWindStrengthTurbulencePulsemagnitudePulseFrequency;

// Detail bending
inline float3 AnimateVertex(float3 pos, float3 normal, float4 animParams)
{
    // animParams stored in color
    // animParams.x = branch phase
    // animParams.y = edge flutter factor
    // animParams.z = primary factor
    // animParams.w = secondary factor

    float mainWindAnim = 1;
    float branchWindAnim = 1;

//  Fade in Wind
    float4 wind;

    #if defined(_WINDFROMSCRIPT)
        wind.xyz = TransformWorldToObjectDir(_LuxURPWindDirSize.xyz);
    //  In case we have no Wind Prefab foliage will vanish otherwise.
        wind.xyz = clamp(wind.xyz, -1, 1);
        wind.xyz *= _LuxURPWindStrengthTurbulencePulsemagnitudePulseFrequency.x;
        wind.w = _LuxURPWindStrengthTurbulencePulsemagnitudePulseFrequency.y;

    //  Animate incoming wind
        float3 objectWorldPos = UNITY_MATRIX_M._m03_m13_m23;
        float3 absObjectWorldPos = abs(objectWorldPos.xyz * 0.125h);
        half sinuswave = _LuxURPWindStrengthTurbulencePulsemagnitudePulseFrequency.w;
        half2 vOscillations = SmoothTriangleWave( half2(absObjectWorldPos.x + sinuswave, absObjectWorldPos.z + sinuswave * 0.7h) );
        // x used for main wind bending / y used for tumbling

    //  To make it better match we simplify the calculation
        float2 fOsc = (vOscillations.xy + vOscillations.yy) * 0.5;

        mainWindAnim += fOsc.x * _LuxURPWindStrengthTurbulencePulsemagnitudePulseFrequency.z;
        branchWindAnim += fOsc.y * _LuxURPWindStrengthTurbulencePulsemagnitudePulseFrequency.z;
    #else
        wind = UNITY_ACCESS_INSTANCED_PROP(Props, _Wind) * UNITY_ACCESS_INSTANCED_PROP(Props, _SquashAmount);
    #endif

// /////////////////////////

    float origLength = length(pos);

    float fDetailAmp = 0.1f;
    float fBranchAmp = 0.3f;
    
    // Phases (object, vertex, branch)
    float fObjPhase = dot(UNITY_MATRIX_M._m03_m13_m23, 1);
    float fBranchPhase = fObjPhase + animParams.x;
    float fVtxPhase = dot(pos.xyz, animParams.y + fBranchPhase);


//  MotionVectors
    float time = _Time.y;
#if defined MVPREVIOUS
    time -= unity_DeltaTime.z; // smooth deltatime
#endif

    // x is used for edges; y is used for branches
    // float2 vWavesIn = _Time.yy + float2(fVtxPhase, fBranchPhase );
    float2 vWavesIn = time.xx + float2(fVtxPhase, fBranchPhase );

    // 1.975, 0.793, 0.375, 0.193 are good frequencies
    half4 vWaves = (frac( vWavesIn.xxyy * float4(1.975, 0.793, 0.375, 0.193) ) * 2.0 - 1.0); // changed to float (android issues)
    vWaves = SmoothTriangleWave( vWaves );
    half2 vWavesSum = vWaves.xz + vWaves.yw;
    
    // Edge (xz) and branch bending (y)
    float3 bend = animParams.y * fDetailAmp *       abs(normal.xyz);
    bend.y = animParams.w * fBranchAmp;
    pos.xyz += ((vWavesSum.xyx * bend) + (wind.xyz * vWavesSum.y * animParams.w)) * wind.w          * branchWindAnim;
    
    // Primary bending
    pos.xyz += animParams.z * wind.xyz                                                              * mainWindAnim;

    pos = normalize(pos) * origLength;

    return pos;
}

// Detail bending
inline float3 AnimateVertexMV(float3 pos, float3 normal, float4 animParams, bool previousFrame)
{
    // animParams stored in color
    // animParams.x = branch phase
    // animParams.y = edge flutter factor
    // animParams.z = primary factor
    // animParams.w = secondary factor

    float mainWindAnim = 1;
    float branchWindAnim = 1;

//  Fade in Wind
    float4 wind;

    #if defined(_WINDFROMSCRIPT)
        wind.xyz = TransformWorldToObjectDir(_LuxURPWindDirSize.xyz);
    //  In case we have no Wind Prefab foliage will vanish otherwise.
        wind.xyz = clamp(wind.xyz, -1, 1);
        wind.xyz *= _LuxURPWindStrengthTurbulencePulsemagnitudePulseFrequency.x;
        wind.w = _LuxURPWindStrengthTurbulencePulsemagnitudePulseFrequency.y;

    //  Animate incoming wind
        float3 objectWorldPos = UNITY_PREV_MATRIX_M._m03_m13_m23;
        float3 absObjectWorldPos = abs(objectWorldPos.xyz * 0.125h);
        half sinuswave = _LuxURPWindStrengthTurbulencePulsemagnitudePulseFrequency.w;
        half2 vOscillations = SmoothTriangleWave( half2(absObjectWorldPos.x + sinuswave, absObjectWorldPos.z + sinuswave * 0.7h) );
        // x used for main wind bending / y used for tumbling

    //  To make it better match we simplify the calculation
        float2 fOsc = (vOscillations.xy + vOscillations.yy) * 0.5;

        mainWindAnim += fOsc.x * _LuxURPWindStrengthTurbulencePulsemagnitudePulseFrequency.z;
        branchWindAnim += fOsc.y * _LuxURPWindStrengthTurbulencePulsemagnitudePulseFrequency.z;
    #else
        wind = UNITY_ACCESS_INSTANCED_PROP(Props, _Wind) * UNITY_ACCESS_INSTANCED_PROP(Props, _SquashAmount);
    #endif

// /////////////////////////

    float origLength = length(pos);

    float fDetailAmp = 0.1f;
    float fBranchAmp = 0.3f;
    
    // Phases (object, vertex, branch)
    float fObjPhase = dot(UNITY_MATRIX_M._m03_m13_m23, 1);
    float fBranchPhase = fObjPhase + animParams.x;
    float fVtxPhase = dot(pos.xyz, animParams.y + fBranchPhase);


//  MotionVectors
    //float time = _Time.y;
    float time = previousFrame ? _LastTimeParameters.x : _Time.y;

    // x is used for edges; y is used for branches
    // float2 vWavesIn = _Time.yy + float2(fVtxPhase, fBranchPhase );
    float2 vWavesIn = time.xx + float2(fVtxPhase, fBranchPhase );

    // 1.975, 0.793, 0.375, 0.193 are good frequencies
    half4 vWaves = (frac( vWavesIn.xxyy * float4(1.975, 0.793, 0.375, 0.193) ) * 2.0 - 1.0); // changed to float (android issues)
    vWaves = SmoothTriangleWave( vWaves );
    half2 vWavesSum = vWaves.xz + vWaves.yw;
    
    // Edge (xz) and branch bending (y)
    float3 bend = animParams.y * fDetailAmp *       abs(normal.xyz);
    bend.y = animParams.w * fBranchAmp;
    pos.xyz += ((vWavesSum.xyx * bend) + (wind.xyz * vWavesSum.y * animParams.w)) * wind.w          * branchWindAnim;
    
    // Primary bending
    pos.xyz += animParams.z * wind.xyz                                                              * mainWindAnim;

    pos = normalize(pos) * origLength;

    return pos;
}

// Expand billboard and modify normal + tangent to fit
inline void ExpandBillboard (in float4x4 mat, inout float3 pos, inout float3 normal, inout float4 tangent)
{
    // tangent.w = 0 if this is a billboard
    float isBillboard = 1.0f - abs(tangent.w);

    // billboard normal
    float3 norb = normalize(mul(float4(normal, 0), mat)).xyz;

    // billboard tangent
    float3 tanb = normalize(mul(float4(tangent.xyz, 0.0f), mat)).xyz;

    pos += mul(float4(normal.xy, 0, 0), mat).xyz * isBillboard;
    normal = lerp(normal, norb, isBillboard);
    tangent = lerp(tangent, float4(tanb, -1.0f), isBillboard);
}


void TreeVertBark (inout VertexInput v)
{
    v.positionOS.xyz *= UNITY_ACCESS_INSTANCED_PROP(Props, _TreeInstanceScale.xyz);
    v.positionOS = AnimateVertex(v.positionOS, v.normalOS, float4(v.color.xy, v.texcoord1.xy));
    v.positionOS = Squash(v.positionOS);
    //v.color.rgb = UNITY_ACCESS_INSTANCED_PROP(Props, _TreeInstanceColor.rgb) * _Color.rgb;
    //v.normalOS = normalize(v.normalOS);
    //v.tangentOS.xyz = normalize(v.tangentOS.xyz);
}

void TreeVertLeaf (inout Attributes v)
{
    ExpandBillboard (UNITY_MATRIX_IT_MV, v.positionOS, v.normalOS, v.tangentOS);
    v.positionOS.xyz *= UNITY_ACCESS_INSTANCED_PROP(Props, _TreeInstanceScale.xyz);
    v.positionOS = AnimateVertex (v.positionOS, v.normalOS, float4(v.color.xy, v.texcoord1.xy));
    v.positionOS = Squash(v.positionOS);
    // v.color.rgb = UNITY_ACCESS_INSTANCED_PROP(Props, _TreeInstanceColor.rgb) * _Color.rgb;
    // v.normalOS = normalize(v.normalOS);
    // v.tangentOS.xyz = normalize(v.tangentOS.xyz);
}


float3 TreeVertLeafMV (Attributes v, float3 positionOS, bool previousFrame)
{
    ExpandBillboard (UNITY_MATRIX_IT_MV, positionOS, v.normalOS, v.tangentOS);
    positionOS.xyz *= UNITY_ACCESS_INSTANCED_PROP(Props, _TreeInstanceScale.xyz);
    positionOS = AnimateVertexMV (positionOS, v.normalOS, float4(v.color.xy, v.texcoord1.xy), previousFrame);
    positionOS = Squash(positionOS);

    return positionOS;

    // v.color.rgb = UNITY_ACCESS_INSTANCED_PROP(Props, _TreeInstanceColor.rgb) * _Color.rgb;
    // v.normalOS = normalize(v.normalOS);
    // v.tangentOS.xyz = normalize(v.tangentOS.xyz);
}

half ScreenDitherToAlpha(float x, float y, float c0)
{
#if (SHADER_TARGET > 30) || defined(SHADER_API_D3D11) || defined(SHADER_API_GLCORE) || defined(SHADER_API_GLES3)
    //dither matrix reference: https://en.wikipedia.org/wiki/Ordered_dithering
    const float dither[64] = {
        0, 32, 8, 40, 2, 34, 10, 42,
        48, 16, 56, 24, 50, 18, 58, 26 ,
        12, 44, 4, 36, 14, 46, 6, 38 ,
        60, 28, 52, 20, 62, 30, 54, 22,
        3, 35, 11, 43, 1, 33, 9, 41,
        51, 19, 59, 27, 49, 17, 57, 25,
        15, 47, 7, 39, 13, 45, 5, 37,
        63, 31, 55, 23, 61, 29, 53, 21 };

    int xMat = int(x) & 7;
    int yMat = int(y) & 7;

    half limit = (dither[yMat * 8 + xMat] + 11.0h) / 64.0h;
    //could also use saturate(step(0.995, c0) + limit*(c0));
    //original step(limit, c0 + 0.01);

    return lerp(limit*c0, 1.0h, c0);
#else
    return 1.0h;
#endif
}

half ComputeAlphaCoverage(float4 screenPos, float fadeAmount)
{
#if (SHADER_TARGET > 30) || defined(SHADER_API_D3D11) || defined(SHADER_API_GLCORE) || defined(SHADER_API_GLES3)
    float2 pixelPosition = screenPos.xy / (screenPos.w + 0.00001);
    pixelPosition *= _ScreenParams.xy;
    half coverage = ScreenDitherToAlpha(pixelPosition.x, pixelPosition.y, fadeAmount);
    return coverage;
#else
    return 1.0;
#endif
}

#endif