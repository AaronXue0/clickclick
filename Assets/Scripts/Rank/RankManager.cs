using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using AudioSystem;

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
            StartCoroutine(StartTestRevealSequence());
        }

        private IEnumerator StartTestRevealSequence()
        {
            yield return new WaitForSeconds(1f);
            AssignRank(3);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                AssignRank(1);
            }
        }

        public void AssignRank(int rank)
        {
            if (isRevealing) return;

            FetchRank(rank);

            targetPlayerRank = rank;
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
            int currentDisplayRank = 9999;
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
            // Display ranks for all positions
            for (int i = 0; i < rankObjects.Count - 1; i++)
            {
                int displayRank;
                int playerPosition = targetPlayerRank == 1 ? 0 : (targetPlayerRank == 2 ? 1 : 2);

                if (i < playerPosition) // Positions above player
                {
                    displayRank = targetPlayerRank - (playerPosition - i);
                    if (displayRank < 1) displayRank = 1;
                }
                else // Positions below player
                {
                    displayRank = targetPlayerRank + (i - playerPosition + 1);
                }

                UpdateRankDisplay(rankDataList[i], displayRank, GetScoreForRank(displayRank));
                rankDataList[i].rank = displayRank;
                yield return new WaitForSeconds(0.15f);
            }

            showRankAudio.DoAction();

            yield return new WaitForSeconds(0.1f);

            UpdatePlayerRankDisplay(targetPlayerRank);
        }

        private void UpdatePlayerRankDisplay(int rank)
        {
            UpdateRankDisplay(rankDataList[rankDataList.Count - 1], rank, GetScoreForRank(rank));
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

        private int GetScoreForRank(int rank)
        {
            // Simple scoring system: higher rank gets a lower score
            // You can adjust this logic based on your actual scoring system
            return (10000 - rank) * 100;
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
