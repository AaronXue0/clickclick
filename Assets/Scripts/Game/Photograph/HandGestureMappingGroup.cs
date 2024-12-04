using UnityEngine;
using ClickClick.GestureTracking;

namespace ClickClick.Photograph
{
    [System.Serializable]
    public class HandGestureMapping
    {
        public HandGesture gestureType;
        public GameObject targetObject;
    }

    [System.Serializable]
    public class HandGestureMappingGroup
    {
        public HandGestureMapping[] gestureMappings;
    }
}