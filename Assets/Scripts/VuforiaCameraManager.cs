using UnityEngine;

public class VuforiaCameraManager : MonoBehaviour
{
    public GameObject vuforiaCamera; // Vuforia AR Camera를 참조

    void Start()
    {
        // vuforiaScene에서는 Vuforia Camera만 활성화
        vuforiaCamera.SetActive(true);
    }
}
