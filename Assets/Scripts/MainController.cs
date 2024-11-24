using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void StartSelectTeam()
    {
        SceneManager.LoadScene("playerSelectionScene");  //
    }
    public void StartPiStreamingScene()
    {
        SceneManager.LoadScene("piStreamingScene");  //
    }
    public void StartVuforiaScene()
    {
        SceneManager.LoadScene("vuforiaScene");  //
    }



}
