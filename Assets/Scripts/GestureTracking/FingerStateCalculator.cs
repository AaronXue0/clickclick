using UnityEngine;
using Mediapipe.Tasks.Components.Containers;
using static ClickClick.GestureTracking.HandGestureDetector;

namespace ClickClick.GestureTracking
{
    public class FingerStateCalculator
    {
        public FingerState DetectFingerState(NormalizedLandmarks landmarks)
        {
            FingerState state = FingerState.Closed;
            var palmBase = landmarks.landmarks[0];  // WRIST
            var palmCenter = CalculatePalmCenter(landmarks);

            if (IsThumbOpen(landmarks))
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

        private Vector3 CalculatePalmCenter(NormalizedLandmarks landmarks)
        {
            return new Vector3(
                (landmarks.landmarks[5].x + landmarks.landmarks[17].x) / 2,
                (landmarks.landmarks[5].y + landmarks.landmarks[17].y) / 2,
                (landmarks.landmarks[5].z + landmarks.landmarks[17].z) / 2
            );
        }

        private bool IsThumbOpen(NormalizedLandmarks landmarks)
        {
            var palmBase = landmarks.landmarks[0];
            var thumbBase = landmarks.landmarks[2];
            var thumbTip = landmarks.landmarks[4];

            float thumbAngle = CalculateFingerAngle(
                ToVector3(palmBase),
                ToVector3(thumbBase),
                ToVector3(thumbTip)
            );

            return thumbAngle > 25f;
        }

        private bool IsFingerOpen(NormalizedLandmarks landmarks,
            int baseIndex, int midIndex, int tipIndex, Vector3 palmCenter)
        {
            var basePoint = landmarks.landmarks[baseIndex];
            var midPoint = landmarks.landmarks[midIndex];
            var tipPoint = landmarks.landmarks[tipIndex];

            float bendAngle = CalculateFingerAngle(
                ToVector3(basePoint),
                ToVector3(midPoint),
                ToVector3(tipPoint)
            );

            float tipToPalmDistance = Vector3.Distance(ToVector3(tipPoint), palmCenter);
            float baseToPalmDistance = Vector3.Distance(ToVector3(basePoint), palmCenter);

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

        private Vector3 ToVector3(NormalizedLandmark landmark)
        {
            return new Vector3(landmark.x, landmark.y, landmark.z);
        }
    }
}