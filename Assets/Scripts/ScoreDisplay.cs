using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI currentScoreText;
    [SerializeField]
    private TextMeshProUGUI highScoreText;

    private int selectedPlayer;

    private void Start()
    {
        // 선택된 플레이어 ID 가져오기
        selectedPlayer = PlayerPrefs.GetInt("SelectedPlayer", 1);

        // 각 플레이어별로 스코어 불러오기
        string finalScoreKey = "FinalScore_Player" + selectedPlayer;
        string previousHighScoreKey = "PreviousHighScore_Player" + selectedPlayer;

        int finalScore = PlayerPrefs.GetInt(finalScoreKey, 0);
        int previousHighScore = PlayerPrefs.GetInt(previousHighScoreKey, 0);

        // 스코어 텍스트 설정
        currentScoreText.text = "Current Score: " + finalScore.ToString();
        highScoreText.text = "Previous High Score: " + previousHighScore.ToString();
    }
}
