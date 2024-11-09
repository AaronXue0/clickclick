using UnityEngine;
using Mediapipe.Tasks.Vision.HandLandmarker;
using Mediapipe.Unity;
using ClickClick.GestureTracking;

namespace ClickClick.GestureTracking
{
    public class HandGestureObjectManager : MonoBehaviour
    {
        [SerializeField] private HandGestureMappingGroup leftHandGestures;
        [SerializeField] private HandGestureMappingGroup rightHandGestures;
        [SerializeField] private MultiHandLandmarkListAnnotation handLandmarkAnnotation;

        private HandGestureDetector gestureDetector;
        private HandLandmarkerResult currentResult;
        private bool needsUpdate = false;

        private void Start()
        {
            ValidateComponents();
            gestureDetector = new HandGestureDetector();
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
                if (!ValidateHandIndex(i, result)) continue;

                var landmarks = result.handLandmarks[i];
                var handedness = result.handedness[i];
                bool isRightHand = handedness.categories[0].categoryName.ToLower().Contains("left");
                var gestureGroup = isRightHand ? rightHandGestures : leftHandGestures;

                var gesture = gestureDetector.DetectGesture(landmarks);
                UpdateHandGestureObject(gestureGroup, gesture, i);
            }
        }

        private void UpdateHandGestureObject(HandGestureMappingGroup gestureGroup, HandGesture gesture, int handIndex)
        {
            var matchingMapping = System.Array.Find(gestureGroup.gestureMappings, m => m.gestureType == gesture);
            if (matchingMapping?.targetObject == null) return;

            matchingMapping.targetObject.SetActive(true);
            UpdateObjectPosition(matchingMapping.targetObject, handIndex);
        }

        private void UpdateObjectPosition(GameObject targetObject, int handIndex)
        {
            if (handLandmarkAnnotation.transform.childCount <= handIndex) return;

            var handAnnotation = handLandmarkAnnotation.transform.GetChild(handIndex);
            var pointListAnnotation = handAnnotation?.GetChild(0);

            if (pointListAnnotation != null && pointListAnnotation.childCount > 9)
            {
                var middleFingerBase = pointListAnnotation.GetChild(9);
                if (middleFingerBase != null)
                {
                    targetObject.transform.SetParent(middleFingerBase, false);
                    targetObject.transform.localPosition = Vector3.zero;
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

        private bool ValidateHandIndex(int index, HandLandmarkerResult result)
        {
            return index < result.handedness.Count && index < result.handLandmarks.Count;
        }

        private void ValidateComponents()
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