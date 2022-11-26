using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Audio;

//https://www.youtube.com/watch?v=6OT43pvUyfY
//followed Brackeys tutorial on Audio in Unity


public class AudioManager : MonoBehaviour
{
    //list of sound scriptable objects
    public List<SoundObj> sounds;

    private void Awake()
    {
        foreach (SoundObj sound in sounds)
        {
            //links the sounds in the list with the necessary audiosources

            if (!sound.ExistingSource)
            {
                //means it doesnt have a source given to it and will use the Game manager as the source
                sound.source = gameObject.GetComponent<AudioSource>();
            }

            SourceSettings(sound);

        }
    }

    public bool getSoundStatus(string soundName)
    {
        SoundObj sound = sounds.Find(sound => sound.SoundName == soundName);

        if (sound.source.isPlaying)
        {
            //if the sound is playing return true
            return true;
        } else
        {
            //else false
            return false;
        }
    }
    public void AddSoundToList(SoundObj newSound)
    {
        if (!newSound.ExistingSource)
        {
            //means it doesnt have a source given to it and will use the Game manager as the source
            newSound.source = gameObject.GetComponent<AudioSource>();
        }

        SourceSettings(newSound);
        sounds.Add(newSound);
    }

    public void Play(string soundName)
    {
        //starts a sound
        SoundObj sound = sounds.Find(sound => sound.SoundName == soundName);
        if (sound == null)
        {
            Debug.LogWarning("Sound: " + soundName + " not found!");
            return;
        }
        SourceSettings(sound);
        sound.source.Play();
        print("playing: " + soundName);
    }

    private void SourceSettings(SoundObj newSound)
    {
        newSound.source.clip = newSound.clip;
        newSound.source.volume = newSound.volume;
        newSound.source.loop = newSound.soundLoop;
        newSound.source.spatialBlend = newSound.spatialBlend;
    }
    public void StopPlaying(string _soundName)
    {
        //stops a sound
        SoundObj sound = sounds.Find(sound => sound.SoundName == _soundName);

        if (sound == null)
        {
            Debug.LogWarning("Sound: " + _soundName + " not found!");
            return;
        }

        sound.source.Stop();
    }
}
