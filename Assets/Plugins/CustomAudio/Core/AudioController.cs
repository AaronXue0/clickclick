using UnityEngine;
using AudioSystem;

[AddComponentMenu("0 : SOB/APIs/Audio API")]
public class AudioController : MonoBehaviour
{
    public bool actionOnStart;
    public AudioAction audioAction;

    private void Start()
    {
        if (actionOnStart)
            DoAction();
    }

    public void DoAction()
    {
        switch (audioAction.type)
        {
            case AudioSourceType.BGM:
                switch (audioAction.bgmActionType)
                {
                    case BgmActionType.Play:
                        if (audioAction.clipReferenceType == ClipReferenceType.File)
                            AudioManager.PlayBGM(audioAction.clip, audioAction.loop);
                        else
                            AudioManager.PlayBGM(
                                audioAction.groupName,
                                audioAction.clipName,
                                audioAction.loop
                            );
                        break;
                    case BgmActionType.Pause:
                        AudioManager.PauseBGM();
                        break;
                    case BgmActionType.Resume:
                        AudioManager.ResumeBGM(
                            audioAction.bgmResumeFadeIn,
                            audioAction.fadeDuration
                        );
                        break;
                    case BgmActionType.Stop:
                        AudioManager.StopBGM();
                        break;
                    case BgmActionType.FadeIn:
                        AudioManager.FadeInBgm(audioAction.fadeDuration);
                        break;
                    case BgmActionType.FadeOut:
                        AudioManager.FadeOutBgm(audioAction.fadeDuration);
                        break;
                }
                break;
            case AudioSourceType.Sound:
                switch (audioAction.soundActionType)
                {
                    case SoundActionType.Play:

                        if (audioAction.clipReferenceType == ClipReferenceType.File)
                            AudioManager.PlaySound(
                                audioAction.clip,
                                audioAction.loop,
                                audioAction.soundMode,
                                0,
                                audioAction.rndPitch
                            );
                        else
                            AudioManager.PlaySound(
                                audioAction.groupName,
                                audioAction.clipName,
                                audioAction.loop,
                                audioAction.soundMode,
                                0,
                                audioAction.rndPitch,
                                audioAction.rndRange
                            );
                        break;
                    case SoundActionType.Stop:
                        AudioManager.StopSound(audioAction.clipName);
                        break;
                    case SoundActionType.StopAll:
                        AudioManager.StopAllSound();
                        break;
                    case SoundActionType.AttchSoundOnBGM:
                        if (audioAction.clipReferenceType == ClipReferenceType.File)
                        {
                            AudioManager.PlaySound(
                                audioAction.clip,
                                audioAction.loop,
                                audioAction.soundMode,
                                AudioManager.GetBGMTime
                            );
                        }
                        else
                        {
                            AudioManager.PlaySound(
                                audioAction.groupName,
                                audioAction.clipName,
                                audioAction.loop,
                                audioAction.soundMode,
                                AudioManager.GetBGMTime
                            );
                        }
                        break;
                }
                break;
        }
    }
}
