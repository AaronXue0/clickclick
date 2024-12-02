using UnityEngine;
using UnityEngine.UI;
using Mediapipe.Tasks.Vision.HandLandmarker;
using ClickClick.GestureTracking;
using Mediapipe.Unity;
using DG.Tweening;

namespace ClickClick.Tool
{
    public class CircularProgressOnHold : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private GameObject targetButton;
        [SerializeField] private Image progressImage;
        [SerializeField] private float duration;
        [SerializeField] private float targetButtonScaleDownFactor = 0.9f;

        [Header("Hand Gesture")]
        [SerializeField] private GameObject rightHandGameObject;
        [SerializeField] private MultiHandLandmarkListAnnotation handLandmarkAnnotation;

        private HandGestureDetector gestureDetector;
        private HandLandmarkerResult currentResult;
        private bool needsUpdate = false;

        private float currentProgress = 0f;
        private bool isOverlapping = false;
        private bool isCompleted = false;

        private Vector3 originalTargetButtonScale;

        [SerializeField] private float stateChangeCooldown = 0.2f;
        private float lastStateChangeTime;

        [SerializeField] private float stateChangeDuration = 0.3f;

        private void Start()
        {
            gestureDetector = new HandGestureDetector();
            originalTargetButtonScale = targetButton.transform.localScale;
            lastStateChangeTime = -stateChangeCooldown;
        }

        private void Update()
        {
            if (needsUpdate)
            {
                UpdateGestureObjectsInternal(currentResult);
                needsUpdate = false;
            }

            // Check if rightHandGameObject is overlapping targetButton
            if (isOverlapping && !isCompleted)
            {
                // Increase progress over time
                currentProgress += Time.deltaTime / duration;
                currentProgress = Mathf.Clamp01(currentProgress);
                progressImage.fillAmount = currentProgress;

                // Trigger button click when progress reaches 1
                if (currentProgress >= 1f)
                {
                    targetButton.GetComponent<Button>().onClick.Invoke();

                    if (!isCompleted)
                    {
                        isCompleted = true;
                        SceneTransition.Instance.TransitionToScene("Game");
                    }
                }

                // Fade out rightHandGameObject's image alpha to 0
                rightHandGameObject.GetComponent<Image>().DOFade(0f, stateChangeDuration);

                // Scale down targetButton
                targetButton.transform.DOScale(originalTargetButtonScale * targetButtonScaleDownFactor, stateChangeDuration);
            }
            else if (!isCompleted && Time.time - lastStateChangeTime >= stateChangeCooldown)
            {
                currentProgress = 0f;
                progressImage.fillAmount = 0f;

                // Fade in rightHandGameObject's image alpha to 1
                rightHandGameObject.GetComponent<Image>().DOFade(1f, stateChangeDuration);

                // Reset targetButton's scale
                targetButton.transform.DOScale(originalTargetButtonScale, stateChangeDuration);

                lastStateChangeTime = Time.time;
            }
        }

        public void UpdateGestureObjects(HandLandmarkerResult result)
        {
            currentResult = result;
            needsUpdate = true;
        }

        private void UpdateGestureObjectsInternal(HandLandmarkerResult result)
        {
            DisableAllObjects(rightHandGameObject);

            if (ReferenceEquals(result, null) || result.handLandmarks == null || result.handLandmarks.Count == 0)
            {
                return;
            }

            for (int i = 0; i < result.handLandmarks.Count; i++)
            {
                var landmarks = result.handLandmarks[i];
                var handedness = result.handedness[i];
                bool isRightHand = handedness.categories[0].categoryName.ToLower().Contains("left");
                var gestureGroup = isRightHand ? rightHandGameObject : null;

                Debug.Log($"Gesture Group = {gestureGroup}");

                if (gestureGroup == null)
                    continue;

                var gesture = gestureDetector.DetectGesture(landmarks);
                UpdateHandGestureObject(gestureGroup, gesture, i);
                break;
            }
        }

        private void UpdateHandGestureObject(GameObject gestureGroup, HandGesture gesture, int handIndex)
        {
            gestureGroup.SetActive(true);
            UpdateObjectPosition(gestureGroup, handIndex);

            // Check if rightHandGameObject is overlapping targetButton
            bool newOverlappingState = IsOverlappingTargetButton(gestureGroup);
            if (newOverlappingState)
            {
                isOverlapping = newOverlappingState;
            }
            else if (Time.time - lastStateChangeTime >= stateChangeCooldown)
            {
                isOverlapping = newOverlappingState;
                lastStateChangeTime = Time.time;
            }
        }

        private void UpdateObjectPosition(GameObject targetObject, int handIndex)
        {
            if (handLandmarkAnnotation == null || handLandmarkAnnotation.transform.childCount <= handIndex)
            {
                return;
            }

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

        private void DisableAllObjects(GameObject gameObject)
        {
            gameObject.SetActive(false);
        }

        private bool IsOverlappingTargetButton(GameObject gestureObject)
        {
            if (gestureObject == null || gameObject.activeSelf == false)
            {
                return false;
            }

            RectTransform gestureRect = gestureObject.GetComponent<RectTransform>();
            RectTransform targetRect = targetButton.GetComponent<RectTransform>();

            if (gestureRect != null && targetRect != null)
            {
                Vector3[] gestureCorners = new Vector3[4];
                Vector3[] targetCorners = new Vector3[4];

                gestureRect.GetWorldCorners(gestureCorners);
                targetRect.GetWorldCorners(targetCorners);

                Rect gestureRectangle = new Rect(gestureCorners[0].x, gestureCorners[0].y,
                    gestureCorners[2].x - gestureCorners[0].x, gestureCorners[2].y - gestureCorners[0].y);

                Rect targetRectangle = new Rect(targetCorners[0].x, targetCorners[0].y,
                    targetCorners[2].x - targetCorners[0].x, targetCorners[2].y - targetCorners[0].y);

                return gestureRectangle.Overlaps(targetRectangle);
            }

            return false;
        }
    }
}