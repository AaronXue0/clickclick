using UnityEngine;
using System;
using Mediapipe.Tasks.Vision.HandLandmarker;

namespace ClickClick.GestureTracking
{
    public class HandGestureGroups : MonoBehaviour
    {
        [SerializeField] private HandGestureMappingGroup leftHandGestures;
        [SerializeField] private HandGestureMappingGroup rightHandGestures;

        private HandLandmarkerResult currentResult;
        private bool needsUpdate = false;

        [Flags]
        private enum FingerState
        {
            Closed = 0,
            ThumbOpen = 1,
            IndexOpen = 2,
            MiddleOpen = 4,
            RingOpen = 8,
            PinkyOpen = 16
        }

        private void Update()
        {
            if (needsUpdate)
            {
                UpdateGestureObjectsInternal(currentResult);
                needsUpdate = false;
            }
        }

        public void UpdateGestureObjects(HandLandmarkerResult result)
        {
            currentResult = result;
            needsUpdate = true;
        }

        private void UpdateGestureObjectsInternal(HandLandmarkerResult result)
        {
            DisableAllObjects(leftHandGestures);
            DisableAllObjects(rightHandGestures);

            if (ReferenceEquals(result, null) || result.handLandmarks == null || result.handLandmarks.Count == 0)
            {
                return;
            }

            for (int i = 0; i < result.handLandmarks.Count; i++)
            {
                var landmarks = result.handLandmarks[i];
                var handedness = result.handedness[i];

                bool isRightHand = handedness.categories[0].categoryName.ToLower().Contains("left");
                var gestureGroup = isRightHand ? rightHandGestures : leftHandGestures;

                var fingerState = DetectFingerState(landmarks);
                var gesture = DetermineGesture(fingerState);

                var matchingMapping = Array.Find(gestureGroup.gestureMappings, m => m.gestureType == gesture);
                if (matchingMapping?.targetObject != null)
                {
                    matchingMapping.targetObject.SetActive(true);
                }
            }
        }

        private void DisableAllObjects(HandGestureMappingGroup group)
        {
            if (group?.gestureMappings == null) return;

            foreach (var mapping in group.gestureMappings)
            {
                if (mapping.targetObject != null)
                {
                    mapping.targetObject.SetActive(false);
                }
            }
        }

        private FingerState DetectFingerState(Mediapipe.Tasks.Components.Containers.NormalizedLandmarks landmarks)
        {
            FingerState state = FingerState.Closed;

            // Thumb
            float pseudoFixKeyPoint = landmarks.landmarks[2].x;
            if ((landmarks.landmarks[0].x > landmarks.landmarks[1].x && landmarks.landmarks[3].x < pseudoFixKeyPoint && landmarks.landmarks[4].x < pseudoFixKeyPoint) ||
                (landmarks.landmarks[0].x < landmarks.landmarks[1].x && landmarks.landmarks[3].x > pseudoFixKeyPoint && landmarks.landmarks[4].x > pseudoFixKeyPoint))
            {
                state |= FingerState.ThumbOpen;
            }

            // Index finger
            pseudoFixKeyPoint = landmarks.landmarks[6].y;
            if (landmarks.landmarks[7].y < pseudoFixKeyPoint && landmarks.landmarks[8].y < pseudoFixKeyPoint)
            {
                state |= FingerState.IndexOpen;
            }

            // Middle finger
            pseudoFixKeyPoint = landmarks.landmarks[10].y;
            if (landmarks.landmarks[11].y < pseudoFixKeyPoint && landmarks.landmarks[12].y < pseudoFixKeyPoint)
            {
                state |= FingerState.MiddleOpen;
            }

            // Ring finger
            pseudoFixKeyPoint = landmarks.landmarks[14].y;
            if (landmarks.landmarks[15].y < pseudoFixKeyPoint && landmarks.landmarks[16].y < pseudoFixKeyPoint)
            {
                state |= FingerState.RingOpen;
            }

            // Pinky
            pseudoFixKeyPoint = landmarks.landmarks[18].y;
            if (landmarks.landmarks[19].y < pseudoFixKeyPoint && landmarks.landmarks[20].y < pseudoFixKeyPoint)
            {
                state |= FingerState.PinkyOpen;
            }

            return state;
        }

        private HandGesture DetermineGesture(FingerState state)
        {
            // Rock: All fingers closed
            if (state == FingerState.Closed)
            {
                return HandGesture.Rock;
            }

            // Paper: All fingers open
            if (state == (FingerState.ThumbOpen | FingerState.IndexOpen | FingerState.MiddleOpen |
                         FingerState.RingOpen | FingerState.PinkyOpen))
            {
                return HandGesture.Paper;
            }

            // Scissors: Only index and middle fingers open
            if (state == (FingerState.IndexOpen | FingerState.MiddleOpen))
            {
                return HandGesture.Scissors;
            }

            return HandGesture.None;
        }
    }
}