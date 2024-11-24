using UnityEngine;
using Vuforia;
using TMPro;
using System.Collections;

public class ARCollisionDetector : MonoBehaviour
{
    private ImageTargetBehaviour imageTargetBehaviour;
    public Transform arCameraTransform;  // AR 카메라를 Inspector에서 직접 할당
    public TextMeshProUGUI[] textObjects;  // 여러 TextMeshProUGUI 텍스트 오브젝트를 할당
    public float distanceThreshold = 12f; // 카메라와 마커 간 거리가 12 이하일 때 텍스트 표시
    private bool isTextDisplayed = false; // 텍스트가 이미 표시 중인지 여부

    private void Start()
    {
        // AR 카메라 Transform이 할당되지 않은 경우, NullReferenceException 방지
        if (arCameraTransform == null)
        {
            Debug.LogError("AR Camera Transform has not been assigned.");
            return;
        }

        // 텍스트 오브젝트가 할당되지 않은 경우 자동으로 찾기
        if (textObjects == null || textObjects.Length == 0)
        {
            textObjects = FindObjectsOfType<TextMeshProUGUI>();
        }

        if (textObjects != null && textObjects.Length > 0)
        {
            Debug.Log("TextObjects are initialized.");
            DisableAllText();  // 모든 텍스트를 비활성화
        }
        else
        {
            Debug.LogError("No TextMeshProUGUI objects found.");
        }

        // Image Target의 ImageTargetBehaviour 가져오기
        imageTargetBehaviour = GetComponent<ImageTargetBehaviour>();
        if (imageTargetBehaviour)
        {
            Debug.Log("ImageTargetBehaviour is assigned.");
            // 상태 변화 이벤트 등록
            imageTargetBehaviour.OnTargetStatusChanged += OnObserverStatusChanged;
        }
        else
        {
            Debug.LogError("ImageTargetBehaviour not found on this GameObject.");
        }
    }

    private void OnDestroy()
    {
        // 이벤트 등록 해제
        if (imageTargetBehaviour)
        {
            imageTargetBehaviour.OnTargetStatusChanged -= OnObserverStatusChanged;
        }
    }

    // 마커가 인식되거나 상태가 변할 때 호출되는 메서드
    private void OnObserverStatusChanged(ObserverBehaviour behaviour, TargetStatus targetStatus)
    {
        Debug.Log("Observer Status Changed: " + targetStatus.Status);

        // 이미지 타겟이 사라졌을 때 텍스트 비활성화
        if (targetStatus.Status == Status.NO_POSE || targetStatus.Status == Status.LIMITED)
        {
            DisableAllText();
            isTextDisplayed = false;  // 다시 텍스트가 표시될 수 있도록 플래그 초기화
        }
    }

    private void Update()
    {
        // 거리가 이미 체크되었는지 확인하고, 이미 텍스트가 표시되었으면 더 이상 체크하지 않음
        if (!isTextDisplayed)
        {
            CheckDistance();
        }
    }

    // 카메라와 이미지 마커의 거리를 체크하는 함수
    private void CheckDistance()
    {
        // 카메라와 마커의 거리를 계산
        float distance = Vector3.Distance(arCameraTransform.position, transform.position);
        Debug.Log("Distance to Image Target: " + distance);

        // 특정 거리 이하로 가까워졌을 때 텍스트 표시 (한 번만 표시)
        if (distance <= distanceThreshold && !isTextDisplayed)
        {
            Debug.Log("AR Camera is close to the Image Target! Starting Coroutine...");
            StartCoroutine(DisplayTextForSeconds(5));  // 텍스트를 5초 동안 표시
            isTextDisplayed = true;
        }
    }

    // 모든 텍스트 오브젝트를 비활성화하는 함수
    private void DisableAllText()
    {
        foreach (TextMeshProUGUI text in textObjects)
        {
            if (text != null)
            {
                text.gameObject.SetActive(false);  // 모든 텍스트 오브젝트 비활성화
            }
        }
    }

    // 텍스트를 일정 시간 동안 표시하는 코루틴
    private IEnumerator DisplayTextForSeconds(float seconds)
    {
        // 새로운 텍스트가 활성화되기 전에 모든 텍스트를 비활성화
        DisableAllText();

        if (textObjects != null && textObjects.Length > 0)
        {
            Debug.Log("Activating TextObject...");
            textObjects[0].gameObject.SetActive(true);  // 첫 번째 텍스트를 활성화
            yield return new WaitForSeconds(seconds);  // 5초 동안 대기
            Debug.Log("Deactivating TextObject after " + seconds + " seconds.");
            textObjects[0].gameObject.SetActive(false);  // 텍스트를 비활성화
            isTextDisplayed = false;  // 텍스트가 다시 표시될 수 있도록 플래그 재설정
        }
        else
        {
            Debug.LogError("TextObjects are null.");
        }
    }
}
