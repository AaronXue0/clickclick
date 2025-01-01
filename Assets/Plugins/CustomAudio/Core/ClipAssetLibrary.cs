using System.Collections.Generic;
using UnityEngine;

namespace AudioSystem
{
    [CreateAssetMenu(menuName = "Game/Audio/Clip Asset Library")]
    public class ClipAssetLibrary : ScriptableObject
    {
        [SerializeField] private List<ClipAssetGroup> groups;

        public int GroupCount { get { return groups.Count; } }
        public ClipAssetGroup this[int index] { get { return groups[index]; } }
        public ClipAssetGroup this[string groupName] { get { return groups.Find(asset => asset.GroupName == groupName); } }
        public int FindIndex(string groupName) { return groups.FindIndex(group => group.GroupName == groupName); }
    }
}