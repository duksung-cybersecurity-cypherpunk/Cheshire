using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreSceneController : MonoBehaviour
{
    public void GoToLeaderboard()
    {
        SceneManager.LoadScene("LeaderboardScene");
    }

    public void GoToMain()
    {
        SceneManager.LoadScene("MainScene");
    }
       public void GoToMobileGameScene()
    {
        SceneManager.LoadScene("mobileGameScene");
    } 
}
