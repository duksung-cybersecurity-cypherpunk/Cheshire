using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float dragDistance = 50.0f; // �巡�� �Ÿ�
    private Vector3 touchStart;
    private Vector3 touchEnd;

    private Movement movement;

    private void Awake()
    {
        movement = GetComponent<Movement>();
    }
    // Update is called once per frame
    private void Update()
    {
        if (Application.isMobilePlatform)
        {
            OnMobilePlatform();
        }
        else
        {
            OnPCPlatform();
        }
    }

    private void OnMobilePlatform()
    {
        // ���� ȭ���� ��ġ�ϰ� ���� ������ �޼ҵ� ����
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);

        // ��ġ ����
        if (touch.phase == TouchPhase.Began)
        {
            touchStart = touch.position;
        }

        // ��ġ & �巡��

        else if (touch.phase == TouchPhase.Moved)
        {
            touchEnd = touch.position;

            OnDragXY();

        }
    }

    private void OnPCPlatform()
    {
        // ��ġ ����
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = Input.mousePosition;
        }
        else if ( Input.GetMouseButton(0) )
        {
            touchEnd = Input.mousePosition;

            OnDragXY();
        }
    }

    private void OnDragXY()
    {
        // ��ġ ���·� x�� �巡�� ������ dragDistance���� Ŭ ��
        if (Mathf.Abs(touchEnd.x - touchStart.x) >= dragDistance)
        {
            movement.MoveToX((int)Mathf.Sign(touchEnd.x - touchStart.x));
            return;
        }

        // ��ġ ���·� y�� ���� �������� �巡�� ������ dragDistance���� Ŭ ��
        if ( touchEnd.y -  touchStart.y >= dragDistance)
        {
            movement.MoveToY();
            return;
        }
    }
}