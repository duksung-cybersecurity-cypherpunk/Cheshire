using UnityEngine;

public class XrCameraManager : MonoBehaviour
{
    public GameObject xROrigin; // XR Origin을 참조

    void Start()
    {
        // arScene에서는 XR Origin만 활성화
        xROrigin.SetActive(true);
    }
}