# Unity3D 환경에서 RC카를 활용한 2D,3D, AR 통합 게임형 시스템  

![image 65](https://github.com/user-attachments/assets/6a13a8a1-7bbf-4a7f-afc0-7a4787f262ff)

## 프로젝트 소개  
**Unity3D 환경에서 RC카를 활용한 2D, 3D, AR 통합 게임형 시스템**은 실제 RC카와 Unity3D를 결합하여 2D, 3D 및 AR 게임 요소를 포함한 인터랙티브 게임 플랫폼입니다.  

이 시스템은 Unity3D와 Raspberry Pi 기반의 RC카를 통해 현실과 가상 세계를 연결하는 사용자 경험을 제공합니다.

<br>

---

<br>


## 시스템 구성  

![image 74](https://github.com/user-attachments/assets/36e7cc3a-5a10-488c-91d0-b8b356cd4cab)


### 1. Unity3D  
- RC카 조작 및 실시간 영상 스트리밍 처리  
- 3D 게임 환경 렌더링 및 AR 효과 적용  

### 2. Raspberry Pi  
- RC카의 모터와 카메라 제어를 담당  
- Unity3D와 TCP 소켓 통신을 통해 데이터를 송수신  

### 3. 하드웨어 구성  
- **Raspberry Pi 4**  
- **라즈베리파이 쉴드**  
- **GPIO 핀**  
- **RC카 모터**  
- **카메라 모듈 v1**

<br>

## 주요 기능  
- **RC카 제어**  
  Unity3D로 빌드된 Android 앱을 통해 사용자가 RC카의 움직임을 제어할 수 있습니다.  
- **실시간 영상 스트리밍**  
  Raspberry Pi 카메라 모듈을 활용해 TCP 소켓 통신으로 RC카 시점의 실시간 영상을 Unity3D에 스트리밍합니다.  
- **3D 게임 요소 통합**  
  Unity3D에서 구현된 3D 게임 플레이를 제공합니다.  
- **AR 기능 확장**  
  Vuforia 기술을 사용하여 AR 환경에서 가상 오브젝트와의 상호작용이 가능하도록 구현되었습니다.

<br>


## 개발자
- **최연 (Unity, IoT)**
- **홍가영 (Design, IoT)**
