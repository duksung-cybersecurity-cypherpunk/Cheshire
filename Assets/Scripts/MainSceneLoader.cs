using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneLoader : MonoBehaviour
{
    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
        public void LoadPlayerSelectionScene()
    {
        SceneManager.LoadScene("playerSelectionScene");
    }
}
