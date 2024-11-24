using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // UI 네임스페이스 추가

public class PortalEffectController : MonoBehaviour
{
    [SerializeField]
    private GameObject portalRedPrefab; // "Potal Red" 프리팹을 할당할 변수
    [SerializeField]
    private Transform quadTransform; // Pi 스트리밍 화면을 표시하는 Quad의 Transform
    [SerializeField]
    private GameObject fadePanel; // 페이드 아웃용 패널 (흰색)

    // 버튼을 누르면 호출되는 함수
    public void ShowPortalRedEffect()
    {
        // Quad 위에 이펙트 생성
        Vector3 spawnPosition = quadTransform.position + new Vector3(0, 0, -0.1f); // Quad 위에 생성
        if (portalRedPrefab != null)
        {
            Instantiate(portalRedPrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Portal Red Prefab is not assigned.");
        }

        // 3초 후 페이드 아웃을 시작하고 씬을 전환
        StartCoroutine(FadeOutAndChangeScene());
    }

    // 3초 대기 후 페이드 아웃하면서 씬 전환
    private IEnumerator FadeOutAndChangeScene()
    {
        // 3초 대기
        yield return new WaitForSeconds(3.0f);

        // 페이드 패널을 활성화하고 흰색으로 페이드 아웃
        fadePanel.SetActive(true);
        Image fadeImage = fadePanel.GetComponent<Image>();
        Color color = fadeImage.color;
        color.a = 0; // 처음에는 투명
        fadeImage.color = color;

        float fadeDuration = 1.5f; // 페이드 아웃 시간 (1초)
        float elapsed = 0;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsed / fadeDuration); // 서서히 흰색으로
            fadeImage.color = color;
            yield return null;
        }

        // 페이드 아웃 후 씬 전환
        SceneManager.LoadScene("vuforiaScene");
    }
}