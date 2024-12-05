using UnityEngine;
using UnityEngine.UI;
using ClickClick.GestureTracking;

namespace ClickClick.Gameplay
{
    public class Tool : MonoBehaviour
    {
        [SerializeField] HandGesture _gesture;

        private Image _image;
        private RectTransform _rectTransform;

        void Awake()
        {
            _image = GetComponent<Image>();
            _rectTransform = GetComponent<RectTransform>();
        }

        void Update()
        {
            CheckOverlap();
        }

        private void CheckOverlap()
        {
            // Find all FixObjects in the scene
            var fixObjects = GameManager.Instance.GetFixObjects;

            foreach (var fixObject in fixObjects)
            {
                if (!fixObject.HasGesture) continue;

                // Get the RectTransform of the FixObject
                var fixObjectRect = fixObject.GetComponent<RectTransform>();

                // Check if rectangles overlap
                if (RectTransformUtility.RectangleContainsScreenPoint(fixObjectRect, _rectTransform.position))
                {
                    fixObject.TryFix(_gesture);
                    break;
                }
            }
        }

        public void SetGesture(HandGesture gesture)
        {
            _gesture = gesture;
        }
    }
}