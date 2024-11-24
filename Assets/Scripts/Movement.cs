using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private GameController gameController;  // IsGameStart 가 true일 때 움직이기 위해

    // X축 이동
    private float moveXWidth = 1.5f;    // 1회 이동 시 이동 거리 (x축)
    private float moveTimeX = 0.1f; // 1회 이동에 소요되는 시간 (x축)
    private bool isXMove = false;   // true: 이동중, false: 이동가능

    // Y축 이동
    private float originY = 0.55f;  // 점프 및 착지하는 y축 값
    private float gravity = -9.81f; // 중력
    private float moveTimeY = 0.4f; // 1회 이동에 소요되는 시간 (y축)
    private bool isJump = false;    // true: 점프중, false: 점프가능

    // Z축 이동
    [SerializeField]
    private float moveSpeed = 20.0f;    // 이동 속도 (z축)
    
    // 회전
    private float rotateSpeed = 300.0f; // 회전 속도

    private float limitY = -1.0f;   // 플레이어가 사망하는 y 위치

    private Rigidbody rigidboby;

    // 플레이어 이동 속도 증가
    private float speedIncreaseRate = 0.2f; // 매 초마다 증가할 속도
    private float maxMoveSpeed = 40.0f;     // 최대 이동 속도

    private void Awake()
    {
        rigidboby = GetComponent<Rigidbody>();
    }


    // Update is called once per frame
   private void Update()
    {
        // 현재 상태가 게임 시작이 아니면 Update()를 실행하지 않는다.
        if (gameController.IsGameStart == false) return;

        // +) 시간이 경과함에 따라 플레이어 이동 속도 증가
        if (moveSpeed < maxMoveSpeed)
        {
            moveSpeed += speedIncreaseRate * Time.deltaTime;
        }

        // z축 이동
        transform.position += Vector3.forward * moveSpeed * Time.deltaTime;

        // 오브젝트 회전 (x축)
        transform.Rotate(Vector3.right * rotateSpeed * Time.deltaTime);

        // 낭떠러지로 떨어지면 플레이어 사망
        if ( transform.position.y < limitY)
        {
            Debug.Log("Game Over");
        }

    }

    public void MoveToX(int x)
    {
        if (isXMove == true) return;

        if (x > 0 && transform.position.x < moveXWidth)
        {
            StartCoroutine(OnMoveToX(x));
        }
        else if (x < 0 && transform.position.x > -moveXWidth)
        {
            StartCoroutine(OnMoveToX(x));
        }

    }
    public void MoveToY()
    {
        // 현재 점프 중으로 점프 불가능
        if (isJump == true) return;

        StartCoroutine(OnMoveToY());
    }

    private IEnumerator OnMoveToX(int direction)
    {
        float current = 0;
        float percent = 0;
        float start = transform.position.x;
        float end = transform.position.x + direction * moveXWidth;

        isXMove = true;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / moveTimeX;

            float x = Mathf.Lerp(start, end, percent);
            transform.position = new Vector3(x, transform.position.y, transform.position.z);

            yield return null;
        }
        isXMove = false;
    }
    private IEnumerator OnMoveToY()
    {
        float current = 0;
        float percent = 0;
        float v0 = -gravity;    // y 방향의 초기 속도
        float fixedX = transform.position.x; // 현재 X 위치를 고정


        isJump = true;
        rigidboby.useGravity = false;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / moveTimeY;

            // 시간 경과에 따라 오브젝트의 y 위치를 바꿔준다
            // 포물선 운동 : 시작위치 + 초기속도 * 시간 + 중력 * 시간제곱
            float y = originY + (v0 * percent) + (gravity * percent * percent);

        //    transform.position = new Vector3(transform.position.x, y, transform.position.z);
          
            // +) 점프 시 이동 방지 - x 값을 고정하여 좌우 이동 방지
            transform.position = new Vector3(fixedX, y, transform.position.z);

            yield return null;
        }

        isJump = false;
        rigidboby.useGravity = true;
    }


}
