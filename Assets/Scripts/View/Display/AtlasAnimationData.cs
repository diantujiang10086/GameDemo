using UnityEngine;
using Unity.Mathematics;

[System.Serializable]
public class AtlasAnimation
{
    public bool isLoop;
    public int frameRate;
    public Sprite[] sprites;
    public float4[] tiles;
    public float2[] sizes;
}

[CreateAssetMenu(fileName = "AtlasAnimationData")]
public class AtlasAnimationData : ScriptableObject
{
    public AtlasAnimation[] atlasAnimations;
}
