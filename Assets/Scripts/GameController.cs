using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // UI 네임스페이스 추가

public class GameController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textTitle;
    [SerializeField]
    private TextMeshProUGUI textTapToPlay;

    [SerializeField]
    private TextMeshProUGUI textCoinCount;

    [SerializeField]
    private GameObject fadePanel; // 검은 패널

    private int coinCount = 0;
    private bool isGameOver = false; // 게임 종료 여부를 추적하는 변수
    public bool IsGameStart { private set; get; }

    private int selectedPlayer;


    private void Awake()
    {
        IsGameStart = false;
        isGameOver = false; // 게임 시작 시 초기화
        textTitle.enabled = true;
        textTapToPlay.enabled = true;
        textCoinCount.enabled = false;
        fadePanel.SetActive(false); // 페이드 패널 비활성화

                // 선택된 플레이어 불러오기
        selectedPlayer = PlayerPrefs.GetInt("SelectedPlayer", 5); // 기본값은 Player 5
        Debug.Log("Selected Player: " + selectedPlayer);
    }

    private IEnumerator Start()
    {
        // 마우스 왼쪽 버튼을 누르기 전까지 시작하지 않고 대기
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                IsGameStart = true;

                textTitle.enabled = false;
                textTapToPlay.enabled = false;
                textCoinCount.enabled = true;

                break;
            }
            yield return null;
        }
    }

    public void IncreaseCoinCount()
    {
        coinCount++;
        textCoinCount.text = coinCount.ToString();
    }

    public void GameOver()
    {
        if (isGameOver) // 이미 게임이 종료되었다면 중복 호출 방지
            return;

        isGameOver = true; // 게임 종료 상태로 설정

        // 플레이어별로 저장될 스코어 키 설정
        string finalScoreKey = "FinalScore_Player" + selectedPlayer;
        string highScoreKey = "HighScore_Player" + selectedPlayer;
        string previousHighScoreKey = "PreviousHighScore_Player" + selectedPlayer;

        // 현재 스코어 저장
        PlayerPrefs.SetInt(finalScoreKey, coinCount);
        PlayerPrefs.Save();

        // 기존의 최고점수 가져오기
        int previousHighScore = PlayerPrefs.GetInt(highScoreKey, 0); // 직전의 최고점수 저장

        // 새로운 최고점수를 갱신할지 확인
        if (coinCount > previousHighScore)
        {
            // 새로운 최고점수가 갱신되면, 기존 최고점수를 previousHighScore로 저장
            PlayerPrefs.SetInt(previousHighScoreKey, previousHighScore);
            
            // 현재 게임의 코인 카운트(새로운 최고점수)를 HighScore로 저장
            PlayerPrefs.SetInt(highScoreKey, coinCount);
        }
        else
        {
            // 새로운 최고점수가 갱신되지 않았을 경우, 최고점수는 그대로 유지
            PlayerPrefs.SetInt(previousHighScoreKey, previousHighScore);
        }

        PlayerPrefs.Save(); // 변경 사항 저장

        StartCoroutine(FadeOutAndLoadScoreScene());
    }

    private IEnumerator FadeOutAndLoadScoreScene()
    {
        fadePanel.SetActive(true);
        Image fadeImage = fadePanel.GetComponent<Image>();
        Color color = fadeImage.color;
        color.a = 0;
        fadeImage.color = color;

        float fadeDuration = 1.0f; // 페이드아웃 시간 (1초)
        float elapsed = 0;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsed / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        // 페이드아웃 완료 후 스코어 씬으로 이동
        SceneManager.LoadScene("scoreScene");
    }
}
