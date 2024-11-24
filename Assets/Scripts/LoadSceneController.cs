using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneController : MonoBehaviour
{
    public void LoadMainScene()
    {
        // MainScene으로 전환
        SceneManager.LoadScene("MainScene");
    }
}
