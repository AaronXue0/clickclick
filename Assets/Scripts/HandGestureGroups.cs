using UnityEngine;
using System;
using Mediapipe.Tasks.Vision.HandLandmarker;
using Mediapipe.Tasks.Components.Containers;
using Mediapipe.Unity;

namespace ClickClick.GestureTracking
{
    public class HandGestureGroups : MonoBehaviour
    {
        [SerializeField] private HandGestureMappingGroup leftHandGestures;
        [SerializeField] private HandGestureMappingGroup rightHandGestures;
        [SerializeField] private MultiHandLandmarkListAnnotation handLandmarkAnnotation;

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

        [SerializeField] private float followSpeed = 10f;
        [SerializeField] private float depth = 10f;
        [SerializeField] private Vector3 positionOffset = Vector3.zero;

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

            if (handLandmarkAnnotation == null)
            {
                Debug.LogError("HandLandmarkAnnotation is not assigned!");
                return;
            }

            for (int i = 0; i < result.handLandmarks.Count; i++)
            {
                if (i >= result.handedness.Count || i >= result.handLandmarks.Count)
                {
                    Debug.LogError($"Index {i} is out of bounds for handedness or landmarks!");
                    continue;
                }

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

                    if (handLandmarkAnnotation.transform.childCount > i)
                    {
                        var handAnnotation = handLandmarkAnnotation.transform.GetChild(i);
                        if (handAnnotation != null)
                        {
                            var pointListAnnotation = handAnnotation.GetChild(0);
                            if (pointListAnnotation != null && pointListAnnotation.childCount > 9)
                            {
                                var middleFingerBase = pointListAnnotation.GetChild(9);
                                if (middleFingerBase != null)
                                {
                                    matchingMapping.targetObject.transform.SetParent(middleFingerBase, false);
                                    matchingMapping.targetObject.transform.localPosition = Vector3.zero;
                                }
                                else
                                {
                                    Debug.LogWarning("Middle finger base landmark not found!");
                                }
                            }
                            else
                            {
                                Debug.LogWarning("Point List Annotation doesn't have enough landmarks!");
                            }
                        }
                        else
                        {
                            Debug.LogWarning("Hand annotation not found for index: " + i);
                        }
                    }
                }
            }
        }

        private Vector3 CalculateHandCenter(NormalizedLandmarks landmarks)
        {
            Vector3 sum = Vector3.zero;
            int[] palmPoints = new int[] { 0, 5, 9, 13, 17 };

            foreach (int index in palmPoints)
            {
                var landmark = landmarks.landmarks[index];
                sum += new Vector3(landmark.x, landmark.y, landmark.z);
            }

            return sum / palmPoints.Length;
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

        private FingerState DetectFingerState(NormalizedLandmarks landmarks)
        {
            FingerState state = FingerState.Closed;

            var palmBase = landmarks.landmarks[0];  // WRIST
            var palmCenter = new Vector3(
                (landmarks.landmarks[5].x + landmarks.landmarks[17].x) / 2,
                (landmarks.landmarks[5].y + landmarks.landmarks[17].y) / 2,
                (landmarks.landmarks[5].z + landmarks.landmarks[17].z) / 2
            );

            var thumbBase = landmarks.landmarks[2];  // THUMB_CMC
            var thumbTip = landmarks.landmarks[4];   // THUMB_TIP
            var thumbAngle = CalculateFingerAngle(
                new Vector3(palmBase.x, palmBase.y, palmBase.z),
                new Vector3(thumbBase.x, thumbBase.y, thumbBase.z),
                new Vector3(thumbTip.x, thumbTip.y, thumbTip.z)
            );
            if (thumbAngle > 25f)
            {
                state |= FingerState.ThumbOpen;
            }

            if (IsFingerOpen(landmarks, 5, 6, 8, palmCenter))
            {
                state |= FingerState.IndexOpen;
            }

            if (IsFingerOpen(landmarks, 9, 10, 12, palmCenter))
            {
                state |= FingerState.MiddleOpen;
            }

            if (IsFingerOpen(landmarks, 13, 14, 16, palmCenter))
            {
                state |= FingerState.RingOpen;
            }

            if (IsFingerOpen(landmarks, 17, 18, 20, palmCenter))
            {
                state |= FingerState.PinkyOpen;
            }

            return state;
        }

        private bool IsFingerOpen(NormalizedLandmarks landmarks,
            int baseIndex, int midIndex, int tipIndex, Vector3 palmCenter)
        {
            var basePoint = landmarks.landmarks[baseIndex];
            var midPoint = landmarks.landmarks[midIndex];
            var tipPoint = landmarks.landmarks[tipIndex];

            float bendAngle = CalculateFingerAngle(
                new Vector3(basePoint.x, basePoint.y, basePoint.z),
                new Vector3(midPoint.x, midPoint.y, midPoint.z),
                new Vector3(tipPoint.x, tipPoint.y, tipPoint.z)
            );

            float tipToPalmDistance = Vector3.Distance(
                new Vector3(tipPoint.x, tipPoint.y, tipPoint.z),
                palmCenter
            );

            float baseToPalmDistance = Vector3.Distance(
                new Vector3(basePoint.x, basePoint.y, basePoint.z),
                palmCenter
            );

            return bendAngle > 160f && tipToPalmDistance > baseToPalmDistance;
        }

        private float CalculateFingerAngle(Vector3 point1, Vector3 point2, Vector3 point3)
        {
            Vector3 vector1 = point1 - point2;
            Vector3 vector2 = point3 - point2;

            float dotProduct = Vector3.Dot(vector1.normalized, vector2.normalized);
            dotProduct = Mathf.Clamp(dotProduct, -1f, 1f);

            return Mathf.Acos(dotProduct) * Mathf.Rad2Deg;
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

        private void Start()
        {
            if (handLandmarkAnnotation == null)
            {
                Debug.LogError("HandLandmarkAnnotation is not assigned! Please assign it in the inspector.");
            }

            if (leftHandGestures == null || rightHandGestures == null)
            {
                Debug.LogError("Hand gesture groups are not assigned! Please assign them in the inspector.");
            }
        }
    }
}