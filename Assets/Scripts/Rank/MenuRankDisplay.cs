using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ClickClick.Manager;

namespace ClickClick.Rank
{
    public class MenuRankDisplay : MonoBehaviour
    {
        [SerializeField] private RankContainer[] rankContainers;

        private const int TOP_PLAYERS_COUNT = 4;

        private void Start()
        {
            // UpdateRankDisplay();
        }

        private void UpdateRankDisplay()
        {
            var topPlayers = DataManager.Instance.GetTopPlayers(TOP_PLAYERS_COUNT);

            // Ensure we have enough containers
            if (rankContainers.Length < TOP_PLAYERS_COUNT)
            {
                Debug.LogWarning("Not enough rank containers assigned. Please assign " + TOP_PLAYERS_COUNT + " containers.");
                return;
            }

            // Update each rank container
            for (int i = 0; i < TOP_PLAYERS_COUNT; i++)
            {
                if (i < topPlayers.Count)
                {
                    // Show container and update score
                    rankContainers[i].rankContainer.SetActive(true);
                    rankContainers[i].scoreText.text = topPlayers[i].score.ToString();
                }
                else
                {
                    // Hide container if there's no player data
                    rankContainers[i].rankContainer.SetActive(false);
                }
            }
        }

        // Optional: Call this method when you want to refresh the display
        public void RefreshDisplay()
        {
            UpdateRankDisplay();
        }
    }

    [System.Serializable]
    public class RankContainer
    {
        public GameObject rankContainer;
        public Image rankImage;
        public TextMeshProUGUI scoreText;
    }
}
