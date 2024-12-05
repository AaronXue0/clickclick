using UnityEngine;

namespace ClickClick.GestureTracking
{
    [System.Serializable]
    public class GestureMapping
    {
        public GameObject targetObject;
        public Sprite displayImage;
        public HandGesture gestureType;
    }
}