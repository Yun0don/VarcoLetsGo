using UnityEngine;

/// 입력 장치는 IPlayerInput 인터페이스를 통해 주입받는다.
public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;      // 이동 속도
    [SerializeField] float rotateLerp = 10f;    // 회전 보간 속도

    [SerializeField] Transform cam;             // 기준이 될 카메라 (없으면 자동으로 메인 카메라 사용)
    [SerializeField] MonoBehaviour inputSource; // IPlayerInput 구현체(MobileInputReader 등) 할당
    IPlayerInput input;                         // 실제로 사용할 입력 인터페이스

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // 카메라가 비어 있으면 메인 카메라를 자동으로 참조
        if (!cam && Camera.main != null)
            cam = Camera.main.transform;

        // 인스펙터에서 넣어준 MonoBehaviour를 IPlayerInput으로 캐스팅
        input = inputSource as IPlayerInput;

        if (input == null)
        {
            Debug.LogError($"{name}: inputSource에 IPlayerInput 구현체를 넣어줘야 합니다.");
        }
    }

    void FixedUpdate()
    {
        // 물리 이동은 FixedUpdate에서 처리
        UpdateMove();
    }

    /// 입력 값을 받아 이동 및 회전을 처리
    void UpdateMove()
    {
        if (input == null || cam == null)
            return;

        // 1) 입력(스틱/WASD) 값 가져오기
        Vector2 moveInput = input.Move;

        // 2) 카메라 방향 기준으로 월드 이동 방향 계산
        Vector3 camForward = cam.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 camRight = cam.right;
        camRight.y = 0f;
        camRight.Normalize();

        Vector3 moveDir = camForward * moveInput.y + camRight * moveInput.x;

        // 3) 실제 이동 처리
        if (moveDir.sqrMagnitude > 0.0001f)
        {
            moveDir.Normalize();

            // 위치 이동
            Vector3 targetPos = rb.position + moveDir * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(targetPos);

            // 바라보는 방향 회전
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            Quaternion newRot = Quaternion.Slerp(rb.rotation, targetRot, rotateLerp * Time.fixedDeltaTime);
            rb.MoveRotation(newRot);
        }

    }
}
