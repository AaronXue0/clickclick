using UnityEngine;
using ClickClick.Data;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace ClickClick.Manager
{
    public class DataManager : MonoBehaviour
    {
        public static DataManager Instance { get; private set; }

        [SerializeField] private CharacterGroup characterGroup;
        private GoogleSheetsManager googleSheetsManager;

        private List<PlayerData> players = new List<PlayerData>();
        private int currentPlayerId = 0;  // To track the next player ID
        private PlayerData currentPlayer;  // To track the current active player

        private void Awake()
        {
            // Singleton pattern implementation
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            googleSheetsManager = gameObject.AddComponent<GoogleSheetsManager>();
        }

        private void Start()
        {
            Initialize();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                UploadPlayerData(CreateNewPlayer(1));
            }
        }

        public void Initialize()
        {
            characterGroup.Initialize();
            LoadPlayersData();
        }

        #region Player
        // Create a new player with default values
        public PlayerData CreateNewPlayer(int characterId = 0)
        {
            PlayerData newPlayer = new PlayerData
            {
                playerId = currentPlayerId++,
                characterId = characterId,
                score = 0,
                rank = 99999
            };

            players.Add(newPlayer);
            SavePlayersData();

            return newPlayer;
        }

        // Convert single player data to JSON
        public string ConvertPlayerToJson(PlayerData player)
        {
            return JsonUtility.ToJson(player);
        }

        // Convert all players data to JSON
        private void SavePlayersData()
        {
            foreach (PlayerData player in players)
            {
                string jsonData = ConvertPlayerToJson(player);
                PlayerPrefs.SetString($"Player_{player.playerId}", jsonData);
            }
            PlayerPrefs.SetInt("CurrentPlayerId", currentPlayerId);
            PlayerPrefs.Save();
        }

        // Load all saved players data
        private void LoadPlayersData()
        {
            currentPlayerId = PlayerPrefs.GetInt("CurrentPlayerId", 0);
            players.Clear();

            for (int i = 0; i < currentPlayerId; i++)
            {
                string jsonData = PlayerPrefs.GetString($"Player_{i}", "");
                if (!string.IsNullOrEmpty(jsonData))
                {
                    PlayerData player = JsonUtility.FromJson<PlayerData>(jsonData);
                    players.Add(player);
                }
            }
        }

        // Get player by ID
        public PlayerData GetPlayer(int playerId)
        {
            return players.Find(p => p.playerId == playerId);
        }
        #endregion

        #region Google Sheets
        public void UploadPlayerData(PlayerData playerData)
        {
            StartCoroutine(googleSheetsManager.UploadPlayerData(playerData));
        }

        public void LoadAllPlayersFromSheet()
        {
            StartCoroutine(googleSheetsManager.GetAllPlayersData());
        }
        #endregion

        #region Current Player Management
        public PlayerData GetCurrentPlayer()
        {
            // If no current player is set, create a new one
            if (currentPlayer == null)
            {
                currentPlayer = CreateNewPlayer();
            }
            return currentPlayer;
        }

        public void SetCurrentPlayer(PlayerData player)
        {
            currentPlayer = player;
        }

        public void SetCurrentPlayer(int playerId)
        {
            currentPlayer = GetPlayer(playerId);
            if (currentPlayer == null)
            {
                Debug.LogWarning($"Player with ID {playerId} not found. Creating new player.");
                currentPlayer = CreateNewPlayer();
            }
        }

        public void UpdatePlayerScore(int playerId, int newScore)
        {
            PlayerData player = GetPlayer(playerId);
            if (player != null)
            {
                player.score = newScore;
                RecalculateRanks();
                SavePlayersData();

                // Upload updated data to Google Sheets
                UploadPlayerData(player);
            }
            else
            {
                Debug.LogWarning($"Player with ID {playerId} not found. Cannot update score.");
            }
        }

        public void UpdateCurrentPlayerScore(int newScore)
        {
            PlayerData player = GetCurrentPlayer();
            player.score = newScore;
            RecalculateRanks();
            SavePlayersData();

            // Upload updated data to Google Sheets
            UploadPlayerData(player);
        }

        private void RecalculateRanks()
        {
            // Sort players by score in descending order
            var sortedPlayers = players.OrderByDescending(p => p.score).ToList();

            // Assign ranks (1-based index)
            for (int i = 0; i < sortedPlayers.Count; i++)
            {
                // Handle tied scores
                if (i > 0 && sortedPlayers[i].score == sortedPlayers[i - 1].score)
                {
                    sortedPlayers[i].rank = sortedPlayers[i - 1].rank;
                }
                else
                {
                    sortedPlayers[i].rank = i + 1;
                }
            }
        }

        public int GetPlayerRank(int playerId)
        {
            PlayerData player = GetPlayer(playerId);
            return player?.rank ?? -1;
        }

        public List<PlayerData> GetTopPlayers(int count)
        {
            return players
                .OrderBy(p => p.rank)
                .Take(count)
                .ToList();
        }
        #endregion
    }
}