using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "SoundClip", menuName = "CustomItems/SoundClip")]
public class SoundObj : ScriptableObject
{
    public string SoundName;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    public AudioSource source;
    public bool soundLoop;

    [Range(0f, 1f)]
    public float spatialBlend;
    public bool ExistingSource;
}
