using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using ClickClick.Manager;

namespace ClickClick.Tool
{
    public class SingleButtonProgress : CircularProgressOnHold
    {
        public bool progressApproval = true;
        [SerializeField] private GameObject targetButton;
        [SerializeField] private Image progressImage;

        private Vector3 originalTargetButtonScale;

        protected override float ProgressFillAmount
        {
            get => progressImage.fillAmount;
            set => progressImage.fillAmount = value;
        }

        protected override void InitializeTargetButton()
        {
            originalTargetButtonScale = targetButton.transform.localScale;
        }

        protected override void HandleProgressComplete()
        {
            targetButton.GetComponent<Button>().onClick.Invoke();
            SceneTransition.Instance.TransitionToScene(sceneToTransitionTo);
        }

        protected override bool IsOverlappingTargetButton(GameObject gestureObject)
        {
            if (!progressApproval) return false;

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

        protected override void UpdateTargetButtonScale(bool isOverlapping)
        {
            if (isOverlapping)
            {
                targetButton.transform.DOScale(originalTargetButtonScale * targetButtonScaleDownFactor, stateChangeDuration);
            }
            else
            {
                targetButton.transform.DOScale(originalTargetButtonScale, stateChangeDuration);
            }
        }
    }
}