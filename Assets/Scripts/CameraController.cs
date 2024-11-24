using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform target;   // 카메라 추적 대상
    private float zDistance;

    private void Awake()
    {
        if ( target != null)
        {
            zDistance = target.position.z - transform.position.z;
        }
    }
    private void LateUpdate()
    {
        // 타겟 없으면 실행하지 않는다
        if (target == null) return;

        // 카메라의 위치(Position) 정보 갱신
        Vector3 position = transform.position;
        position.z = target.position.z - zDistance;
        transform.position = position;


    }
}
