using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Text;

public class CarController : MonoBehaviour
{
    private TcpClient controlClient;
    private NetworkStream controlStream;

    // IP 주소 및 포트 번호
    private string raspberryPiIP = "192.168.137.215";
    private int controlPort = 5050;

    private void Start()
    {
        try
        {
            // TCP 클라이언트 연결
            controlClient = new TcpClient(raspberryPiIP, controlPort);
            controlStream = controlClient.GetStream();
            Debug.Log("CarController connected to Raspberry Pi.");
        }
        catch (SocketException e)
        {
            Debug.LogError("Error connecting to Raspberry Pi: " + e.Message);
        }
    }

    // 차량 제어 함수들
    public void MoveForward()
    {
        SendControlCommand("forward", 30);  // 속도를 30으로 설정하여 전진
    }

    public void MoveBackward()
    {
        SendControlCommand("backward", 30);  // 후진
    }

    public void TurnLeft()
    {
        SendControlCommand("left", 30);  // 좌회전
    }

    public void TurnRight()
    {
        SendControlCommand("right", 30);  // 우회전
    }

    public void StopCar()
    {
        SendControlCommand("stop", 0);  // 정지
    }

    // "캡쳐하기" 버튼을 눌렀을 때 호출되는 함수
    public void CaptureFrame()
    {
        SendControlCommand("capture", 0);  // "capture" 명령을 Raspberry Pi로 전송
    }

    // 서버로 명령을 보내는 함수
    private void SendControlCommand(string direction, int speed)
    {
        if (controlStream != null && controlStream.CanWrite)
        {
            string message = $"{direction},{speed}\n";
            byte[] commandBytes = Encoding.ASCII.GetBytes(message);
            controlStream.Write(commandBytes, 0, commandBytes.Length);
            Debug.Log($"Sent command: {direction}, speed: {speed}");
        }
    }

    private void OnApplicationQuit()
    {
        if (controlStream != null) controlStream.Close();
        if (controlClient != null) controlClient.Close();
    }
}