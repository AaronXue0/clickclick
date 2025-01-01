using System.Collections;
using System.Collections.Generic;
// using SceneSystem;
using UnityEngine;

namespace Obscura.Refactor.Audio
{
    using AudioSystem;

    [AddComponentMenu("0 : SOB/APIs/Repeat Audio API")]
    public class RepeatAudioPlayer : MonoBehaviour
    {
        [SerializeField]
        private Vector2 rndRepeatTime;

        [SerializeField]
        private AudioController audioController;

        [SerializeField]
        private RandomPlay randomPlay;

        private bool isPlaying = false;

        public void DoAction()
        {
            if (isPlaying)
                return;

            isPlaying = true;
            StartCoroutine(PlayAudioAtRandomIntervals());
        }

        public void StopAction()
        {
            isPlaying = false;
        }

        private IEnumerator PlayAudioAtRandomIntervals()
        {
            while (isPlaying)
            {
                if (audioController != null)
                {
                    audioController.DoAction();
                }
                else if (randomPlay != null)
                {
                    randomPlay.RndAudio();
                }
                yield return new WaitForSeconds(Random.Range(rndRepeatTime.x, rndRepeatTime.y));
            }
        }
    }
}
