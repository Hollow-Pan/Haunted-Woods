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

        string jsonString = PlayerPrefs.GetString("leaderboard", string.Empty);
        HighScores highScores = string.IsNullOrEmpty(jsonString) 
            ? new HighScores { leaderboardEntryList = new List<LeaderboardEntry>() }
            : JsonUtility.FromJson<HighScores>(jsonString);

        leaderboardEntryList = highScores.leaderboardEntryList;
        leaderboardEntryTransformList = new List<Transform>();

        leaderboardEntryList.Sort((a, b) => b.score.CompareTo(a.score));
    }

    private void Start() {
        entryTemplate.gameObject.SetActive(false);

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

        TextMeshProUGUI rankText = entryTransform.Find("posTextTemplate").GetComponent<TextMeshProUGUI>();
        Image medalImage = entryTransform.Find("medalImageTemplate").GetComponent<Image>();

        if (rank == 1) {
            medalImage.sprite = goldMedal;
            medalImage.gameObject.SetActive(true);
            rankText.gameObject.SetActive(false);
        } else if (rank == 2) {
            medalImage.sprite = silverMedal;
            medalImage.gameObject.SetActive(true);
            rankText.gameObject.SetActive(false);
        } else if (rank == 3) {
            medalImage.sprite = bronzeMedal;
            medalImage.gameObject.SetActive(true);
            rankText.gameObject.SetActive(false);
        } else {
            medalImage.gameObject.SetActive(false);
            rankText.gameObject.SetActive(true);
            rankText.text = rank + GetRankSuffix(rank);
        }

        // Set the score text
        entryTransform.Find("scoreTextTemplate").GetComponent<TextMeshProUGUI>().text = leaderboardEntry.score.ToString();

        transformList.Add(entryTransform);
    }

    private string GetRankSuffix(int rank) {
        switch (rank % 10) {
            case 1 when rank != 11: return "ST";
            case 2 when rank != 12: return "ND";
            case 3 when rank != 13: return "RD";
            default: return "TH";
        }
    }

    public void AddLeaderboardEntry(int score){
        LeaderboardEntry newEntry = new LeaderboardEntry {score = score};
        leaderboardEntryList.Add(newEntry);

        leaderboardEntryList.Sort((a, b) => b.score.CompareTo(a.score));

        HighScores highScores = new HighScores { leaderboardEntryList = leaderboardEntryList };
        string json = JsonUtility.ToJson(highScores);
        PlayerPrefs.SetString("leaderboard", json);
        PlayerPrefs.Save();
    }

    private class HighScores{
        public List<LeaderboardEntry> leaderboardEntryList;
    }

    [System.Serializable]
    private class LeaderboardEntry {
        public int score;
    }
}
