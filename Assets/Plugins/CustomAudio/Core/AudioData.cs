public class AudioData
{
    public bool isMuteBgm;
    public bool isMuteSound;
    public float musicVolume;
    public float soundVolume;

    public AudioData(
        bool isMuteBgm = false,
        bool isMuteSound = false,
        float musicVolume = 0.5f,
        float soundVolume = 0.5f
    )
    {
        this.isMuteBgm = isMuteBgm;
        this.isMuteSound = isMuteSound;
        this.musicVolume = musicVolume;
        this.soundVolume = soundVolume;
    }
}
