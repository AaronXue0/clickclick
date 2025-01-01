using System.Collections;
using System.Collections.Generic;
using AudioSystem;
using UnityEngine;

namespace Obscura.Refactor.Audio
{
    [AddComponentMenu("0 : SOB/APIs/Random Audio API")]
    public class RandomPlay : MonoBehaviour
    {
        [SerializeField]
        private List<AudioController> audios;

        public void RndAudio()
        {
            AudioController controller = audios[Random.Range(0, audios.Count)];
            controller.DoAction();
        }

        public void StopAllSound()
        {
            foreach (var a in audios)
            {
                a.audioAction.soundActionType = SoundActionType.Stop;
                a.DoAction();
                a.audioAction.soundActionType = SoundActionType.Play;
            }
        }
    }
}
