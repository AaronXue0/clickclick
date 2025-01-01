using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    public Slider musicVolumeSlider;
    public Slider soundVolumeSlider;

    private void Start()
    {
        float defaultMusic = 0.5f;// DataManager.MusicVolume;
        float defaultSound = 0.5f;// DataManager.SoundVolume;

        musicVolumeSlider.value = defaultMusic;
        soundVolumeSlider.value = defaultSound;

        musicVolumeSlider.onValueChanged.AddListener(UpdateMusicVolume);
        soundVolumeSlider.onValueChanged.AddListener(UpdateSoundVolume);
    }

    public void UpdateMusicVolume(float volume)
    {
        AudioManager.UpdateMusicVolume(volume);
    }

    public void UpdateSoundVolume(float volume)
    {
        AudioManager.UpdateSoundVolume(volume);
    }
}
