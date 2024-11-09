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

            // 定義手掌基準點 (手掌中心)
            var palmBase = landmarks.landmarks[0];  // WRIST
            var palmCenter = new Vector3(
                (landmarks.landmarks[5].x + landmarks.landmarks[17].x) / 2,
                (landmarks.landmarks[5].y + landmarks.landmarks[17].y) / 2,
                (landmarks.landmarks[5].z + landmarks.landmarks[17].z) / 2
            );

            // 拇指 (使用向量角度)
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

            // 食指
            if (IsFingerOpen(landmarks, 5, 6, 8, palmCenter))
            {
                state |= FingerState.IndexOpen;
            }

            // 中指
            if (IsFingerOpen(landmarks, 9, 10, 12, palmCenter))
            {
                state |= FingerState.MiddleOpen;
            }

            // 無名指
            if (IsFingerOpen(landmarks, 13, 14, 16, palmCenter))
            {
                state |= FingerState.RingOpen;
            }

            // 小指
            if (IsFingerOpen(landmarks, 17, 18, 20, palmCenter))
            {
                state |= FingerState.PinkyOpen;
            }

            return state;
        }

        private bool IsFingerOpen(Mediapipe.Tasks.Components.Containers.NormalizedLandmarks landmarks,
            int baseIndex, int midIndex, int tipIndex, Vector3 palmCenter)
        {
            var basePoint = landmarks.landmarks[baseIndex];
            var midPoint = landmarks.landmarks[midIndex];
            var tipPoint = landmarks.landmarks[tipIndex];

            // 計算手指彎曲角度
            float bendAngle = CalculateFingerAngle(
                new Vector3(basePoint.x, basePoint.y, basePoint.z),
                new Vector3(midPoint.x, midPoint.y, midPoint.z),
                new Vector3(tipPoint.x, tipPoint.y, tipPoint.z)
            );

            // 計算指尖到手掌中心的距離
            float tipToPalmDistance = Vector3.Distance(
                new Vector3(tipPoint.x, tipPoint.y, tipPoint.z),
                palmCenter
            );

            float baseToPalmDistance = Vector3.Distance(
                new Vector3(basePoint.x, basePoint.y, basePoint.z),
                palmCenter
            );

            // 如果手指彎曲角度較大且指尖距離手掌中心較遠，則認為手指是張開的
            return bendAngle > 160f && tipToPalmDistance > baseToPalmDistance;
        }

        private float CalculateFingerAngle(Vector3 point1, Vector3 point2, Vector3 point3)
        {
            Vector3 vector1 = point1 - point2;
            Vector3 vector2 = point3 - point2;

            float dotProduct = Vector3.Dot(vector1.normalized, vector2.normalized);
            // 確保 dotProduct 在有效範圍內
            dotProduct = Mathf.Clamp(dotProduct, -1f, 1f);

            return Mathf.Acos(dotProduct) * Mathf.Rad2Deg;
        }

        private HandGesture DetermineGesture(FingerState state)
        {
            // 布：大部分手指都打開
            // 允許拇指可能沒完全打開
            if ((state & (FingerState.IndexOpen | FingerState.MiddleOpen |
                         FingerState.RingOpen | FingerState.PinkyOpen)) ==
                (FingerState.IndexOpen | FingerState.MiddleOpen |
                 FingerState.RingOpen | FingerState.PinkyOpen))
            {
                return HandGesture.Paper;
            }

            // 剪刀：食指和中指打開，其他手指閉合
            // 允許有些微誤差
            if ((state & (FingerState.IndexOpen | FingerState.MiddleOpen)) ==
                (FingerState.IndexOpen | FingerState.MiddleOpen) &&
                (state & (FingerState.RingOpen | FingerState.PinkyOpen)) == 0)
            {
                return HandGesture.Scissors;
            }

            // 石頭：所有手指都閉合
            return HandGesture.Rock;
        }
    }
}