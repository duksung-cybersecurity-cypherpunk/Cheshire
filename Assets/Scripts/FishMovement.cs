using UnityEngine;

public class FishMovement : MonoBehaviour
{
    public GameObject fishObject;  // 참조할 물고기 오브젝트
    public Transform cameraTransform;  // XR 카메라(AR 카메라)의 Transform
    public float moveSpeed = 1.0f;     // 물고기 이동 속도

    void Update()
    {
        if (fishObject == null) return;  // 물고기 오브젝트가 설정되지 않은 경우 실행 중지

        // 카메라 방향을 향한 벡터 계산
        Vector3 direction = (cameraTransform.position - fishObject.transform.position).normalized;

        // 물고기 이동
        fishObject.transform.position += direction * moveSpeed * Time.deltaTime;

        // 물고기가 카메라를 바라보도록 회전
        // fishObject.transform.LookAt(cameraTransform);
    }
}
