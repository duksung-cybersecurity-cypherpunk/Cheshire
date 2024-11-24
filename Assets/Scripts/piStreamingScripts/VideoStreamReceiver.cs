using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class VideoStreamReceiver : MonoBehaviour
{
    private TcpClient videoClient;
    private NetworkStream videoStream;
    private Thread receiveThread;
    private Texture2D videoTexture;
    private Queue<byte[]> frameQueue = new Queue<byte[]>();

    // PiStreamMaterial을 Quad에 적용하기 위한 필드
    public Material piStreamMaterial;  // Inspector에서 연결할 Material
    public GameObject videoQuad;       // Inspector에서 연결할 Quad 오브젝트

    void Start()
    {
        // Raspberry Pi 스트림 설정
        videoClient = new TcpClient("192.168.137.215", 5000);  // Raspberry Pi의 IP 주소와 포트
        videoStream = videoClient.GetStream();

        // 비디오 출력용 텍스처 설정
        videoTexture = new Texture2D(640, 480, TextureFormat.RGB24, false);

        // 데이터 수신 스레드 시작
        receiveThread = new Thread(ReceiveData);
        receiveThread.Start();
    }

    void Update()
    {
        // 수신한 영상을 큐에서 꺼내 PiStreamMaterial에 적용
        if (frameQueue.Count > 0)
        {
            byte[] imageData = frameQueue.Dequeue();
            videoTexture.LoadImage(imageData);
            videoTexture.Apply();

            // PiStreamMaterial에 Raspberry Pi 스트림을 텍스처로 적용
            if (piStreamMaterial != null && videoQuad != null)
            {
                piStreamMaterial.mainTexture = videoTexture;
                // Quad의 Material에 PiStreamMaterial을 적용
                videoQuad.GetComponent<Renderer>().material = piStreamMaterial;
            }
        }
    }

    // Raspberry Pi에서 비디오 데이터를 수신하는 스레드
    void ReceiveData()
    {
        while (true)
        {
            try
            {
                // 프레임 크기를 먼저 수신
                byte[] sizeBytes = new byte[sizeof(long)];
                videoStream.Read(sizeBytes, 0, sizeBytes.Length);
                long dataSize = BitConverter.ToInt64(sizeBytes, 0);

                // 해당 크기만큼 데이터 수신
                byte[] data = new byte[dataSize];
                int totalBytesRead = 0;
                while (totalBytesRead < dataSize)
                {
                    int bytesRead = videoStream.Read(data, totalBytesRead, (int)(dataSize - totalBytesRead));
                    if (bytesRead == 0)
                    {
                        break;
                    }
                    totalBytesRead += bytesRead;
                }

                // 수신된 프레임을 큐에 추가하여 메인 스레드에서 처리
                lock (frameQueue)
                {
                    frameQueue.Enqueue(data);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error receiving video stream: " + e.Message);
                break;
            }
        }
    }

    void OnApplicationQuit()
    {
        // 종료 시 리소스 해제
        if (receiveThread != null)
        {
            receiveThread.Abort();
        }
        videoStream.Close();
        videoClient.Close();
    }
}