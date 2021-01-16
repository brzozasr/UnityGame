using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    [System.Serializable]
    public class Sound
    {
        public string name;
        
        public AudioClip clip;

        public bool mute;
        public bool bypassEffects;
        public bool bypassListenerEffects;
        public bool playOnAwake;
        public bool loop;
        [Range(0, 256)]
        public int priority = 128;
        [Range(0f, 1f)]
        public float volume = 1;
        [Range(.1f, 3f)]
        public float pitch = 1;
        [Range(-1f, 1f)]
        public float stereoPan = 0;
        [Range(0f, 1f)] 
        public float spatialBlend = 0;
        [Range(0f, 1.1f)] 
        public float reverbZoneMix = 1;
        
        [HideInInspector]
        public AudioSource source;

    }
}