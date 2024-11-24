using TMPro;
using UnityEngine;

public class LeaderboardController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI[] playerRankTexts; // 1등부터 5등까지 표시할 TextMeshProUGUI 배열

    private void Start()
    {
        // 팀 이름 배열
        string[] teamNames = { "Gryffindor", "Ravenclaw", "Hufflepuff", "Slytherin", "Muggle" };

        // 각 플레이어의 이름과 최고 점수 불러오기
        PlayerData[] players = new PlayerData[5];
        for (int i = 0; i < 5; i++)
        {
            int highScore = PlayerPrefs.GetInt("HighScore_Player" + (i + 1), 0); // 각 플레이어의 최고 점수 가져오기
            players[i] = new PlayerData(teamNames[i], highScore); // teamNames 배열에서 이름 매핑
        }

        // 점수에 따라 내림차순 정렬
        System.Array.Sort(players, (x, y) => y.highScore.CompareTo(x.highScore));

        // 순위별로 팀명과 점수 표시
        for (int i = 0; i < playerRankTexts.Length; i++)
        {
            playerRankTexts[i].text = $"{i + 1}. {players[i].playerName} : {players[i].highScore}";
        }
    }
}

[System.Serializable]
public class PlayerData
{
    public string playerName;
    public int highScore;

    public PlayerData(string playerName, int highScore)
    {
        this.playerName = playerName;
        this.highScore = highScore;
    }
}
