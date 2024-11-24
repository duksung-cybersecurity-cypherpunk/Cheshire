using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelectionController : MonoBehaviour
{
    // 플레이어 선택 시 호출될 메서드
    public void SelectPlayer(int playerID)
    {
        // 선택된 플레이어 ID를 PlayerPrefs에 저장
        PlayerPrefs.SetInt("SelectedPlayer", playerID);
        PlayerPrefs.Save();

        // mobileGameScene으로 이동
        SceneManager.LoadScene("mobileGameScene");
    }
}
