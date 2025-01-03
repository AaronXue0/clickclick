using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using AudioSystem;
using ClickClick.Manager;
using ClickClick.Data;
using System.Linq;

namespace ClickClick.Rank
{
    public class RankManager : MonoBehaviour
    {
        [SerializeField] private Transform rankParent;
        [SerializeField] private List<RankData> rankDataList;
        [SerializeField] private GameObject btn;
        [SerializeField] private float revealDuration = 3f; // Duration of the reveal animation
        [SerializeField] private float numberChangeInterval = 0.05f; // How fast numbers change

        [Header("Audio")]
        [SerializeField] private AudioController drumRollAudio;
        [SerializeField] private AudioController showRankAudio;


        private List<GameObject> rankObjects = new List<GameObject>();
        private Transform playerRankObject;
        private int targetPlayerRank;
        private bool isRevealing = false;

        private void Awake()
        {
            // Get all rank objects under rankParent
            for (int i = 0; i < rankParent.childCount; i++)
            {
                rankObjects.Add(rankParent.GetChild(i).gameObject);
            }
            // Set player rank object (last one)
            playerRankObject = rankObjects[rankObjects.Count - 1].transform;
        }

        private void Start()
        {
            // Just load rankings directly
            LoadPlayerRankings();
        }

        private void LoadPlayerRankings()
        {
            // Get current player's data
            PlayerData currentPlayer = DataManager.Instance.GetCurrentPlayer();
            int currentRank = currentPlayer.rank;

            // Get all players sorted by rank
            List<PlayerData> allPlayers = DataManager.Instance.GetTopPlayers(999);

            // Find current player's index in the sorted list
            int currentPlayerIndex = allPlayers.FindIndex(p => p.playerId == currentPlayer.playerId);

            List<PlayerData> displayPlayers = new List<PlayerData>();

            if (currentRank == 1)
            {
                // If player is top ranked, get 4 players below
                displayPlayers = allPlayers
                    .Take(5)
                    .ToList();
            }
            else
            {
                // Get 2 players above and 2 below
                int startIndex = Mathf.Max(currentPlayerIndex - 2, 0);
                int count = 5;

                // Adjust if we're near the start of the list
                if (startIndex + count > allPlayers.Count)
                {
                    startIndex = Mathf.Max(allPlayers.Count - count, 0);
                }

                displayPlayers = allPlayers
                    .Skip(startIndex)
                    .Take(count)
                    .ToList();
            }

            // Update UI for each rank position
            for (int i = 0; i < displayPlayers.Count && i < rankDataList.Count; i++)
            {
                PlayerData player = displayPlayers[i];
                UpdateRankDisplay(rankDataList[i], player.rank, player.score);
                rankDataList[i].rank = player.rank;
                rankDataList[i].score = player.score;
            }

            // Start reveal animation
            AssignRank(currentRank);
        }

        public void AssignRank(int rank)
        {
            if (isRevealing) return;

            PlayerData currentPlayer = DataManager.Instance.GetCurrentPlayer();
            targetPlayerRank = currentPlayer.rank;

            FetchRank(targetPlayerRank);
            StartCoroutine(RevealRankSequence());
        }

        public void FetchRank(int rank)
        {
            int playerPosition = rank == 1 ? 0 : (rank == 2 ? 1 : 2);

            for (int i = 0; i < rankDataList.Count; i++)
            {
                if (i < playerPosition)
                {
                    rankDataList[i].rank = rank - (playerPosition - i);
                }
                else if (i > playerPosition)
                {
                    rankDataList[i].rank = rank + (i - playerPosition);
                }
                else
                {
                    rankDataList[i].rank = rank;
                }

                if (rankDataList[i].rank < 1) rankDataList[i].rank = 1;
            }
        }

        private IEnumerator RevealRankSequence()
        {
            Debug.Log("Starting RevealRankSequence");
            isRevealing = true;
            int currentDisplayRank;
            float elapsedTime = 0f;

            // Start random number animation for all ranks except player
            Coroutine randomizeCoroutine = StartCoroutine(RandomizeNumbers());

            drumRollAudio.DoAction();

            // Decrease player's rank
            while (elapsedTime < revealDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / revealDuration;
                currentDisplayRank = Mathf.RoundToInt(Mathf.Lerp(9999, targetPlayerRank, t));
                UpdatePlayerRankDisplay(currentDisplayRank);
                yield return null;
            }

            // Stop only the randomize numbers coroutine instead of all coroutines
            if (randomizeCoroutine != null)
                StopCoroutine(randomizeCoroutine);

            // Set the final rank first
            UpdatePlayerRankDisplay(targetPlayerRank);

            for (int i = 0; i < rankObjects.Count - 1; i++) // Changed to exclude player's rank
            {
                UpdateRankDisplay(rankDataList[i], rankDataList[i].rank, rankDataList[i].score);
            }

            rankDataList[rankDataList.Count - 1].rankText.text = "?";

            // yield return new WaitForSeconds(1f);

            // Determine player position based on rank
            int targetPosition;
            if (targetPlayerRank == 1)
                targetPosition = 0;
            else if (targetPlayerRank == 2)
                targetPosition = 1;
            else
                targetPosition = 2;

            // yield return new WaitForSeconds(1f);
            yield return StartCoroutine(AnimatePlayerToPosition(targetPosition));
            yield return new WaitForSeconds(1f);

            yield return StartCoroutine(DisplayFinalRanks());
            isRevealing = false;

            btn.SetActive(true);
        }

        private IEnumerator AnimatePlayerToPosition(int targetIndex)
        {
            int currentIndex = rankObjects.Count - 1;
            while (currentIndex > targetIndex)
            {
                playerRankObject.SetSiblingIndex(currentIndex - 1);
                currentIndex--;
                yield return new WaitForSeconds(0.15f);
            }
            yield return null;
        }

        private IEnumerator RandomizeNumbers()
        {
            while (true)
            {
                int index = 0;
                foreach (RankData rankObj in rankDataList)
                {
                    if (index != rankDataList.Count - 1)
                    {
                        int randomRank = Random.Range(1, 10000);
                        int randomScore = Random.Range(0, 1000000);
                        UpdateRankDisplay(rankDataList[index], randomRank, randomScore);
                    }
                    index++;
                }
                yield return new WaitForSeconds(numberChangeInterval);
            }
        }

        private IEnumerator DisplayFinalRanks()
        {
            PlayerData currentPlayer = DataManager.Instance.GetCurrentPlayer();
            List<PlayerData> displayPlayers;

            if (currentPlayer.rank == 1)
            {
                // Get top 5 players
                displayPlayers = DataManager.Instance.GetTopPlayers(5);
            }
            else
            {
                // Get surrounding players
                List<PlayerData> allPlayers = DataManager.Instance.GetTopPlayers(999);
                int currentIndex = allPlayers.FindIndex(p => p.playerId == currentPlayer.playerId);
                int startIndex = Mathf.Max(currentIndex - 2, 0);
                displayPlayers = allPlayers
                    .Skip(startIndex)
                    .Take(5)
                    .ToList();
            }

            // Display ranks
            for (int i = 0; i < displayPlayers.Count && i < rankDataList.Count - 1; i++)
            {
                PlayerData player = displayPlayers[i];
                UpdateRankDisplay(rankDataList[i], player.rank, player.score);
                yield return new WaitForSeconds(0.15f);
            }

            showRankAudio.DoAction();
            yield return new WaitForSeconds(0.1f);

            // Show current player's rank
            UpdatePlayerRankDisplay(currentPlayer.rank);
        }

        private void UpdatePlayerRankDisplay(int rank)
        {
            PlayerData currentPlayer = DataManager.Instance.GetCurrentPlayer();
            UpdateRankDisplay(rankDataList[rankDataList.Count - 1], rank, currentPlayer.score);
        }

        private void UpdateRankDisplay(RankData rankData, int rank, int score)
        {
            if (rankData != null)
            {
                if (rankData.rankText != null)
                {
                    rankData.rankText.text = rank.ToString();
                }

                if (rankData.scoreText != null)
                {
                    rankData.scoreText.text = score.ToString();
                }
            }
        }
    }

    [System.Serializable]
    public class RankData
    {
        [HideInInspector]
        public int rank;
        [HideInInspector]
        public Sprite sprite;
        [HideInInspector]
        public Texture rawImageTexture;
        [HideInInspector]
        public int score;

        public TMP_Text rankText;
        public TMP_Text scoreText;
    }
}
