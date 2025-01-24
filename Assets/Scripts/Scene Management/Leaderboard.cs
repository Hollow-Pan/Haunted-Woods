using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour {

    [SerializeField] private Transform entryContainer;
    [SerializeField] private Transform entryTemplate;
    [SerializeField] private float templateHeight = 20f;

    [SerializeField] private Sprite goldMedal;
    [SerializeField] private Sprite silverMedal;
    [SerializeField] private Sprite bronzeMedal;

    private List<LeaderboardEntry> leaderboardEntryList;
    private List<Transform> leaderboardEntryTransformList;

    private void Awake() {
        string jsonString = PlayerPrefs.GetString("leaderboard");
        HighScores highScores = JsonUtility.FromJson<HighScores>(jsonString) ?? new HighScores();

        leaderboardEntryList = highScores.leaderboardEntryList ?? new List<LeaderboardEntry>();
        leaderboardEntryList.Sort((a, b) => b.score.CompareTo(a.score)); // Sort descending

        leaderboardEntryTransformList = new List<Transform>();
    }

    private void Start() {
        entryTemplate.gameObject.SetActive(false);

        UpdateLeaderboardUI();
    }

    public void AddScoreToLeaderboard(int score) {
        LeaderboardEntry newEntry = new LeaderboardEntry { score = score };

        leaderboardEntryList.Add(newEntry);
        leaderboardEntryList.Sort((a, b) => b.score.CompareTo(a.score)); // Keep it sorted

        // Save updated leaderboard
        HighScores highScores = new HighScores { leaderboardEntryList = leaderboardEntryList };
        string json = JsonUtility.ToJson(highScores);
        PlayerPrefs.SetString("leaderboard", json);
        PlayerPrefs.Save();

        // Update UI
        UpdateLeaderboardUI();
    }

    private void UpdateLeaderboardUI() {
        foreach (Transform entryTransform in leaderboardEntryTransformList) {
            Destroy(entryTransform.gameObject);
        }
        leaderboardEntryTransformList.Clear();

        int maxEntries = Mathf.Min(5, leaderboardEntryList.Count);
        for (int i = 0; i < maxEntries; i++) {
            CreateLeaderboardEntryTransform(leaderboardEntryList[i], entryContainer, leaderboardEntryTransformList);
        }
    }

    private void CreateLeaderboardEntryTransform(LeaderboardEntry leaderboardEntry, Transform container, List<Transform> transformList) {
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;

        Transform medal = entryTransform.Find("medalImageTemplate");
        Transform rankText = entryTransform.Find("posTextTemplate");

        if (rank <= 3) {
            medal.gameObject.SetActive(true);
            rankText.gameObject.SetActive(false);

            Image medalSprite = medal.GetComponent<Image>();
            if (rank == 1) medalSprite.sprite = goldMedal;
            else if (rank == 2) medalSprite.sprite = silverMedal;
            else if (rank == 3) medalSprite.sprite = bronzeMedal;
        } else {
            medal.gameObject.SetActive(false);
            rankText.gameObject.SetActive(true);
            rankText.GetComponent<TextMeshProUGUI>().text = $"{rank}TH";
        }

        entryTransform.Find("scoreTextTemplate").GetComponent<TextMeshProUGUI>().text = leaderboardEntry.score.ToString();

        transformList.Add(entryTransform);
    }

    [System.Serializable]
    private class HighScores {
        public List<LeaderboardEntry> leaderboardEntryList;
    }

    [System.Serializable]
    private class LeaderboardEntry {
        public int score;
    }
}
