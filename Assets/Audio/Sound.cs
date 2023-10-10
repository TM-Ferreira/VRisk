using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "ScriptableObjects/Sound")]
public class Sound : ScriptableObject
{
    public AudioClip clip;
    public AudioManager.SoundID id;
    
    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;
}
