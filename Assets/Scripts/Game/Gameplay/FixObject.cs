using UnityEngine;
using UnityEngine.UI;
using ClickClick.GestureTracking;

namespace ClickClick.Gameplay
{
    public class FixObject : MonoBehaviour
    {
        private HandGesture _gesture;
        private Image image;

        private void Awake()
        {
            image = GetComponent<Image>();
        }

        public void AssignGesture(HandGesture gesture, Sprite sprite)
        {
            if (_gesture != HandGesture.None)
                return;

            _gesture = gesture;
            image.sprite = sprite;
        }

        public void TryFix(HandGesture gesture)
        {
            if (_gesture == HandGesture.None)
                return;

            if (_gesture == gesture)
                ObjectFixed();
        }

        public void ObjectFixed()
        {
            GameManager.Instance.FixedGesture(_gesture);

            image.sprite = null;
            _gesture = HandGesture.None;
        }

        public bool HasGesture => _gesture != HandGesture.None;
    }
}