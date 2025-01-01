using UnityEngine;

namespace AudioSystem
{
    public enum AudioSourceType
    {
        BGM,
        Sound
    }

    public enum BgmActionType
    {
        Play,
        Stop,
        Pause,
        Resume,
        FadeIn,
        FadeOut
    }

    public enum SoundActionType
    {
        Play,
        Stop,
        StopAll,
        AttchSoundOnBGM
    }

    public enum ClipReferenceType
    {
        File,
        Library
    }

    [System.Serializable]
    public class AudioAction
    {
        public AudioSourceType type;
        public BgmActionType bgmActionType;
        public SoundActionType soundActionType;

        public ClipReferenceType clipReferenceType;
        public string groupName;
        public string clipName;
        public AudioClip clip;
        public bool loop;

        public bool bgmResumeFadeIn = true;

        [Range(0, 10)]
        public float fadeDuration = 1;

        public PlaySoundMode soundMode;
        public bool rndPitch = false;
        public Vector2 rndRange = new Vector2(-0.7f, 0.7f);
    }
}
