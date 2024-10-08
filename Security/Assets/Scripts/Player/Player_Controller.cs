using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Controller : MonoBehaviour
{
    PlayerInputAction inputAction;
    public PlayerInputAction InputAction => inputAction;
    CharacterController controller;
    public CharacterController Controller => controller;
    CinemachineVirtualCamera cinemachine;
    public CinemachineVirtualCamera Cinemachine => cinemachine;

    public Transform cameraRoot;    // 카메라 위치
    Vector3 moveDir = Vector3.zero; // 위치변수

    float currentSpeed = 0.0f;  // 현재 이동속도
    float walkSpeed = 3.0f;     // 걷기 속도
    float sprintSpeed = 4.7f;   // 달리기 속도
    bool sprintCheck = false;   // 달리기 체크

    float jumpHeight = 4.0f;        // 점프 높이
    bool jumpCheck = false;         // 점프 체크
    float jumpCheckHeight = 0.0f;   // 점프 높이 확인
    float gravity = 9.8f;           // 중력 크기
    int jumpCount = 0;              // 점프 횟수 - 더블점프 금지

    float crouchDecrease = 1.0f;    // 웅크리기 이동속도 감소량
    bool crouchCheck = false;       // 웅크리기 체크

    float rotateSensitiveX = 7.5f;  // 화면 x방향 전환 민감도
    float rotateSensitiveY = 10.0f; // 화면 y방향 전환 민감도
    float curRotateY = 0.0f;        // 현재 이동한 y방향

    Vector3 boxsize = new Vector3(0.25f, 0.125f, 0.25f);
    Vector3 groundCheckPos; // 바닥 위치

    public Action onSprinting;
    public Action offSprinting;

    bool isMove = false;
    public bool isStamina = false;

    private void Start()
    {
        currentSpeed = walkSpeed;
    }

    private void Awake()
    {
        inputAction = new PlayerInputAction();
        controller = GetComponent<CharacterController>();
        cinemachine = GetComponentInChildren<CinemachineVirtualCamera>();

        cameraRoot = transform.GetChild(0);
    }

    private void OnEnable()
    {
        inputAction.Player.Enable();
        inputAction.Player.Move.performed += OnMove;
        inputAction.Player.Move.canceled += OnMove;
        inputAction.Player.Sprint.performed += OnSprint;
        inputAction.Player.Sprint.canceled += OnSprint;
        inputAction.Player.Jump.performed += OnJump;
        inputAction.Player.Crouch.performed += OnCrouch;
        inputAction.Player.Crouch.canceled += OnCrouch;

        inputAction.Mouse.Enable();
        inputAction.Mouse.MouseVector2.performed += OnMouseDelta;
        inputAction.Mouse.MouseLeftClick.performed += OnMouseLeftClick;
        inputAction.Mouse.MouseRightClick.performed += OnMOuseRightClick;
    }

    private void OnDisable()
    {

        inputAction.Mouse.MouseRightClick.performed -= OnMOuseRightClick;
        inputAction.Mouse.MouseLeftClick.performed -= OnMouseLeftClick;
        inputAction.Mouse.MouseVector2.performed -= OnMouseDelta;
        inputAction.Mouse.Enable();

        inputAction.Player.Crouch.canceled -= OnCrouch;
        inputAction.Player.Crouch.performed -= OnCrouch;
        inputAction.Player.Jump.performed -= OnJump;
        inputAction.Player.Sprint.canceled -= OnSprint;
        inputAction.Player.Sprint.performed -= OnSprint;
        inputAction.Player.Move.canceled -= OnMove;
        inputAction.Player.Move.performed -= OnMove;
        inputAction.Player.Enable();
    }

    private void Update()
    {
        if(!IsGrounded())
            moveDir.y -= gravity * Time.deltaTime;

        if (isMove && sprintCheck)
        {
            isStamina = false;
            onSprinting?.Invoke();
        }

        // 플레이어 x, z좌표 이동
        controller.Move(Time.deltaTime * (currentSpeed * crouchDecrease) * transform.TransformDirection(new Vector3(moveDir.x, 0.0f, moveDir.z)));
        // 플레이어 y좌표 이동
        controller.Move(Time.deltaTime * new Vector3(0.0f, moveDir.y, 0.0f));
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 dir = context.ReadValue<Vector2>();
        moveDir.x = dir.x; moveDir.z = dir.y;

        if (context.performed)
        {
            isMove = true;

        }
        else
        {
            isMove = false;
            OffSprinting();
        }
    }

    private void OnSprint(InputAction.CallbackContext context)
    {
        if(!crouchCheck)    // 웅크리기 상태가 아닐 때
        {
            if (context.performed)
            {
                sprintCheck = true;
                currentSpeed = sprintSpeed;
            }
            else
            {
                if (sprintCheck)
                {
                    OffSprinting();
                }
            }
        }
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if(!crouchCheck  && IsGrounded())
        {
            if(jumpCount < 1)   // 더블점프 막기
            {
                moveDir.y = jumpHeight;
            
                if(jumpCount == 0)
                {
                    jumpCheckHeight = transform.position.y + controller.radius * 0.3f;
                }
                jumpCheck = true;
                jumpCount++;
            }
        }
    }

    private void OnCrouch(InputAction.CallbackContext context)
    {
        if (!sprintCheck && IsGrounded())
        {
            if (context.performed)
            {
                crouchDecrease = 0.5f;  // 감소량 0.5배
                crouchCheck = true;

                cameraRoot.transform.position += new Vector3(0.0f, -0.5f, 0.0f);
            }
            else
            {
                crouchDecrease = 1.0f;  // 감소량 없음
                crouchCheck = false;
                
                cameraRoot.transform.position += new Vector3(0.0f, 0.5f, 0.0f);
            }
        }
    }

    private void OnMouseDelta(InputAction.CallbackContext context)
    {
        Vector2 temp = context.ReadValue<Vector2>();

        // 입력받은 좌표를 x방향 전환 민감도만큼 이동
        float rotateX = temp.x * rotateSensitiveX * Time.fixedDeltaTime;
        transform.Rotate(Vector3.up, rotateX);
        float rotateY = temp.y * rotateSensitiveY * Time.fixedDeltaTime;
        curRotateY -= rotateY;

        // y방향 전환의 최소 및 최대량을 지정
        curRotateY = Mathf.Clamp(curRotateY, -60.0f, 60.0f);

        cameraRoot.rotation = Quaternion.Euler(curRotateY, cameraRoot.eulerAngles.y, cameraRoot.eulerAngles.z);
    }

    private void OnMouseLeftClick(InputAction.CallbackContext context)
    {

    }

    private void OnMOuseRightClick(InputAction.CallbackContext context)
    {

    }

    public void OffSprinting()
    {
        sprintCheck = false;
        currentSpeed = walkSpeed;

        offSprinting?.Invoke();
    }

    private bool IsGrounded()
    {
        // 점프 중일 때, 현재 y높이가 목표 y보다 높으면
        if(jumpCheck && transform.position.y > jumpCheckHeight)
        {
            jumpCheck = false;
        }

        // 캐릭터 밑으로 바닥을 체크하는 박스를 생성
        groundCheckPos = new Vector3(transform.position.x, transform.position.y + controller.radius * -3.0f, transform.position.z);

        // 박스가 Ground레이어에 닿을 경우
        if(Physics.CheckBox(groundCheckPos, boxsize, Quaternion.identity, LayerMask.GetMask("Ground")))
        {
            if(!jumpCheck)
            {
                if(moveDir.y < jumpHeight)
                {
                    moveDir.y = -0.01f;
                }
                jumpCheck = false;
                jumpCount = 0;
                return true;
            }
        }
        return false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected() // 선택했을 때
    {
    }

    private void OnDrawGizmos() // 상시
    {
        // 박스
        Gizmos.color = Color.cyan;
        Gizmos.DrawCube(groundCheckPos, boxsize);

        // 머리
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(cameraRoot.transform.position, 0.25f);

        // 사거리
        Gizmos.color = Color.red;
        Vector2 from = cameraRoot.position;
        Vector2 to = cameraRoot.position + cameraRoot.forward * 2.0f;
        Gizmos.DrawLine(from, to);
    }
#endif
}
