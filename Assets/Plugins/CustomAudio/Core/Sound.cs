using UnityEngine;

namespace AudioSystem
{
    public class Sound
    {
        private AudioSource source;

        public string SoundName { get; private set; }
        public bool Playing { get { return source != null && source.isPlaying; } }

        public AudioClip Clip { get { return source.clip; } set { source.clip = value; } }
        public bool Mute { get { return source.mute; } set { source.mute = value; } }
        public bool Loop { get { return source.loop; } set { source.loop = value; } }
        public float Volume { get { return source.volume; } set { source.volume = value; } }

        public Sound(string soundName, AudioClip clip, AudioSource audioSource, bool isMute = false, float volume = 1, bool loop = false)
        {
            source = audioSource;
            SoundName = soundName;
            Clip = clip;
            Mute = isMute;
            Loop = loop;
            Volume = volume;

            source.Play();
        }

        public void DestroySource()
        {
            Object.Destroy(source);
            source = null;
        }
    }
}