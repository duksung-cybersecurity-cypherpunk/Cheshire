using System.Collections;
using TMPro;
using UnityEngine;

public class VuforiaSceneController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textFeelAR; // 깜빡거릴 텍스트

    private void Start()
    {
        // 코루틴을 사용해 텍스트 깜빡임 시작
        StartCoroutine(BlinkText());
    }

    // 텍스트가 깜빡이는 효과를 주는 코루틴
    private IEnumerator BlinkText()
    {
        while (true) // 무한 반복
        {
            // 텍스트 투명도를 1에서 0까지 줄임
            for (float alpha = 1; alpha >= 0.5; alpha -= Time.deltaTime)
            {
                SetTextAlpha(alpha);
                yield return null;
            }

            // 텍스트 투명도를 0에서 1까지 늘림
            for (float alpha = 0; alpha <= 1; alpha += Time.deltaTime)
            {
                SetTextAlpha(alpha);
                yield return null;
            }
        }
    }

    // 텍스트의 알파값을 설정하는 함수
    private void SetTextAlpha(float alpha)
    {
        Color color = textFeelAR.color;
        color.a = alpha;
        textFeelAR.color = color;
    }
}
