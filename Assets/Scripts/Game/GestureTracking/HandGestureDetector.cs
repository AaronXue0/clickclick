using UnityEngine;
using Mediapipe.Tasks.Components.Containers;

namespace ClickClick.GestureTracking
{
    public class HandGestureDetector
    {
        [System.Flags]
        public enum FingerState
        {
            Closed = 0,
            ThumbOpen = 1,
            IndexOpen = 2,
            MiddleOpen = 4,
            RingOpen = 8,
            PinkyOpen = 16
        }

        private readonly FingerStateCalculator fingerStateCalculator;

        public HandGestureDetector()
        {
            fingerStateCalculator = new FingerStateCalculator();
        }

        public HandGesture DetectGesture(NormalizedLandmarks landmarks)
        {
            var fingerState = fingerStateCalculator.DetectFingerState(landmarks);
            return DetermineGesture(fingerState);
        }

        private HandGesture DetermineGesture(FingerState state)
        {
            if ((state & (FingerState.IndexOpen | FingerState.MiddleOpen |
                         FingerState.RingOpen | FingerState.PinkyOpen)) ==
                (FingerState.IndexOpen | FingerState.MiddleOpen |
                 FingerState.RingOpen | FingerState.PinkyOpen))
            {
                return HandGesture.Paper;
            }

            if ((state & (FingerState.IndexOpen | FingerState.MiddleOpen)) ==
                (FingerState.IndexOpen | FingerState.MiddleOpen) &&
                (state & (FingerState.RingOpen | FingerState.PinkyOpen)) == 0)
            {
                return HandGesture.Scissors;
            }

            return HandGesture.Rock;
        }
    }
}