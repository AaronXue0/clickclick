using UnityEngine;

namespace ClickClick.Data
{
    [System.Serializable]
    public class PlayerData
    {
        public int playerId;
        public int characterId;
        public int score;
        public int rank;

        public string imgPath;
        public string playerPhotoPath;
    }
}