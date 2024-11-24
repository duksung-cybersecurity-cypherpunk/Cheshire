### 라즈베리파이4에서 모터(RC카 바퀴) 제어, 카메라 영상 송신 하는 코드
### 라즈베리파이 쉴드에 고장난 핀이 있어 이에 맞게 커스텀한 상태

import cv2
import socket
import struct
import numpy as np
import threading
import RPi.GPIO as GPIO

# 바퀴에 맞는 핀 번호 설정 (앞바퀴 제외)
wheel_pins = {
    "right_rear": [22, 13],  # 오른쪽 뒷바퀴
    "left_rear": [18, 21],   # 왼쪽 뒷바퀴
}

# PWM 객체를 저장할 딕셔너리
pwm_objects = {}

# GPIO 및 PWM 초기화
def init_motor_pins():
    GPIO.setmode(GPIO.BCM)  # BCM 모드 사용
    GPIO.setwarnings(False)  # 경고 메시지 끄기
    for pins in wheel_pins.values():
        GPIO.setup(pins[0], GPIO.OUT)  # 방향 제어 핀
        GPIO.setup(pins[1], GPIO.OUT)  # PWM 제어 핀
        GPIO.output(pins[0], False)     # 초기화: 모든 바퀴를 멈춤

        # 각 바퀴별 PWM 객체 생성 및 저장
        pwm = GPIO.PWM(pins[1], 1000)   # 주파수 1000Hz
        pwm.start(0)                    # PWM 시작, 속도 0으로 설정
        pwm_objects[pins[1]] = pwm      # PWM 객체 저장

# 각 바퀴를 제어하는 함수
def control_wheel(wheel_name, direction, speed):
    pins = wheel_pins[wheel_name]
    speed = max(0, min(100, speed))  # 속도 값은 0~100 사이로 제한

    # 방향 설정 (모든 바퀴에 동일하게 적용)
    if direction == "forward":
        GPIO.output(pins[0], True)   # 전진
    elif direction == "backward":
        GPIO.output(pins[0], False)  # 후진

    # PWM 설정
    pwm = pwm_objects[pins[1]]
    pwm.ChangeDutyCycle(speed)

# 모든 바퀴 정지
def stop_wheels():
    print("Stopping all wheels.")
    for pins in wheel_pins.values():
        GPIO.output(pins[0], False)
        pwm = pwm_objects[pins[1]]
        pwm.ChangeDutyCycle(0)

# 차량을 제어하는 함수
def move_car(direction, speed=30):
    print(f"Moving car: {direction} with speed: {speed}")

    if direction == "forward":
        # 왼쪽 뒷바퀴: 전진, 오른쪽 뒷바퀴: 후진 (반대로 설정)
        control_wheel("left_rear", "forward", speed)
        control_wheel("right_rear", "backward", speed)
    elif direction == "backward":
        # 왼쪽 뒷바퀴: 후진, 오른쪽 뒷바퀴: 전진 (반대로 설정)
        control_wheel("left_rear", "backward", speed)
        control_wheel("right_rear", "forward", speed)
    elif direction == "left":
        # 왼쪽 뒷바퀴: 후진, 오른쪽 뒷바퀴: 후진
        control_wheel("left_rear", "backward", speed)
        control_wheel("right_rear", "backward", speed)
    elif direction == "right":
        # 왼쪽 뒷바퀴: 전진, 오른쪽 뒷바퀴: 전진
        control_wheel("left_rear", "forward", speed)
        control_wheel("right_rear", "forward", speed)
    elif direction == "stop":
        stop_wheels()

# 버튼 입력 처리 서버
def button_input_server():
    control_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    control_socket.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
    control_socket.bind(('0.0.0.0', 5050))
    control_socket.listen(1)

    print("Waiting for control input connection...")
    client_socket, client_address = control_socket.accept()
    print(f"Control input connected from {client_address}")

    try:
        while True:
            data = client_socket.recv(1024).decode().strip()
            if data:
                try:
                    direction, speed = data.split(',')
                    move_car(direction, int(speed))
                except ValueError:
                    print(f"Invalid data received: {data}")
                    continue
    except Exception as e:
        print(f"Control input server error: {e}")
    finally:
        stop_wheels()
        client_socket.close()
        control_socket.close()

# 카메라 스트리밍 서버
def camera_streaming():
    server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server_socket.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
    server_socket.bind(('0.0.0.0', 5000))
    server_socket.listen(1)

    print("Waiting for a camera connection...")
    client_socket, client_address = server_socket.accept()
    print(f"Camera connection from {client_address} has been established!")

    # GStreamer 파이프라인 설정
    cap = cv2.VideoCapture("v4l2src device=/dev/video0 ! video/x-raw, width=640, height=480, framerate=30/1 ! videoconvert ! appsink", cv2.CAP_GSTREAMER)

    if not cap.isOpened():
        print("Cannot open camera")
        return

    try:
        while True:
            ret, frame = cap.read()
            if not ret:
                print("Failed to grab frame")
                break

            # 프레임을 JPEG로 인코딩
            encode_param = [int(cv2.IMWRITE_JPEG_QUALITY), 90]
            result, buffer = cv2.imencode('.jpg', frame, encode_param)
            if not result:
                print("Failed to encode frame")
                continue

            # 바이트 형식으로 변환하여 전송
            data = np.array(buffer).tobytes()

            # 프레임 크기를 먼저 전송
            message_size = struct.pack("L", len(data))
            client_socket.sendall(message_size + data)
    except Exception as e:
        print(f"Camera server error: {e}")
    finally:
        cap.release()
        client_socket.close()
        server_socket.close()

if __name__ == "__main__":
    # GPIO 초기화
    init_motor_pins()

    # 버튼 입력 서버와 카메라 스트리밍 서버를 각각 스레드로 실행
    button_input_thread = threading.Thread(target=button_input_server)
    camera_thread = threading.Thread(target=camera_streaming)

    button_input_thread.start()
    camera_thread.start()

    button_input_thread.join()
    camera_thread.join()

    # 모든 PWM 종료 및 GPIO 정리
    stop_wheels()
    GPIO.cleanup()
    print("Servers closed and GPIO cleaned up.")
