using System;
using DefaultNamespace;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    private void Awake()
    {
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();

            sound.source.mute = sound.mute;
            sound.source.bypassEffects = sound.bypassEffects;
            sound.source.bypassListenerEffects = sound.bypassListenerEffects;
            sound.source.playOnAwake = sound.playOnAwake;
            sound.source.loop = sound.loop;
            sound.source.priority = sound.priority;
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.panStereo = sound.stereoPan;
            sound.source.spatialBlend = sound.spatialBlend;
            sound.source.reverbZoneMix = sound.reverbZoneMix;
        }
    }

    public void PlaySound(string soundName)
    {
        var foundSound = Array.Find(sounds, sound => sound.name == soundName);

        if (foundSound == null)
        {
            Debug.Log($"Sound: {soundName} not found!");
            return;
        }

        foundSound.source.Play();
    }
}