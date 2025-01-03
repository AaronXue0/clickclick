using UnityEngine;

namespace ClickClick.Data
{
    [System.Serializable]
    public class PlayerData
    {
        public int playerId;
        public int characterId;
        public int score = 0;
        public int rank = 99999;

        public string imgPath;
        public string playerPhotoPath;
    }
}