using System.Collections.Generic;
using UnityEngine;

namespace AudioSystem
{
    [CreateAssetMenu(menuName = "Game/Audio/Clip Asset Group")]
    public class ClipAssetGroup : ScriptableObject
    {
        [SerializeField] private string groupName;
        [SerializeField] private List<ClipAsset> assets;

        public string GroupName { get { return groupName; } }
        public int Count { get { return assets.Count; } }
        public ClipAsset this[int index] { get { return assets[index]; } }
        public ClipAsset this[string clipName] { get { return assets.Find(asset => asset.clipName == clipName); } }
        public int FindIndex(string clipName) { return assets.FindIndex(clipAsset => clipAsset.clipName == clipName); }
    }
}