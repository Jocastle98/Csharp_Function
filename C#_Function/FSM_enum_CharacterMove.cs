
해당 코드에서 rb.velocity.y ==0f 로 땅을 체크하는 부분은 간단하기 위해 한부분일뿐이고
실제로는 다른 방식으로 땅을 체크하여 제작합니다.

/// <summary>
/// CharacterFSM은 캐릭터 상태 (Idle, Walk, Jump)를 관리하는 유한 상태 머신(FSM)입니다.
/// 이 코드는 플레이어 입력과 캐릭터의 조건(움직임, 착지 상태 등)을 기반으로 상태 전환을 처리합니다.
/// 상태별 로직을 명확히 분리하여 유지보수 및 확장이 용이하도록 설계되었습니다.
/// </summary>
public enum CharacterFSMState
{
    Idle, // 대기 상태
    Walk, // 걷는 상태
    Jump  // 점프 상태
}

public class CharacterFSM : MonoBehaviour
{
    // Animator 파라미터를 해시로 저장 (성능 최적화)
    private static readonly int Speed_Hash = Animator.StringToHash("Speed");

    // 현재 상태와 이전 상태를 추적
    private CharacterFSMState currentState = CharacterFSMState.Idle;
    private CharacterFSMState prevState = CharacterFSMState.Idle;

    // 물리 기반 움직임 처리를 위한 Rigidbody
    private Rigidbody rb;

    // 캐릭터 움직임 및 점프 관련 설정
    [SerializeField] private float moveSpeed = 3.0f;
    [SerializeField] private float jumpForce = 10.0f;

    // 캐릭터가 땅에 있는지 여부를 확인하는 변수
    private bool isGrounded;

    // 플레이어 입력을 처리하기 위한 변수
    private Vector2 moveInput;
    private InputAction moveAction;
    private InputAction jumpAction;

    // 캐릭터 애니메이션을 관리하는 Animator
    private Animator animator;

    void Start()
    {
        // 필요한 컴포넌트 초기화
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        currentState = CharacterFSMState.Idle;

        // PlayerInput으로부터 입력 액션 불러오기
        moveAction = GetComponent<PlayerInput>().actions["Move"];
        jumpAction = GetComponent<PlayerInput>().actions["Jump"];
    }

    void Update()
    {
        // 지속적으로 움직임 및 점프 입력을 읽음
        moveInput = moveAction.ReadValue<Vector2>();
        bool bPressedJump = jumpAction.triggered;

        // 캐릭터가 땅에 있는지 확인
        GroundCheck();

        // 입력 및 상태 조건에 따라 상태 전환 처리
        StateChange(bPressedJump);

        // 상태 진입, 업데이트, 종료 로직 실행
        EnterState();
        UpdateState();
        ExitState();
    }

    /// <summary>
    /// 캐릭터가 땅에 있는지 확인하기 위해 수직 속도를 체크합니다.
    /// </summary>
    private void GroundCheck()
    {
        isGrounded = rb.velocity.y == 0.0f;
    }

    /// <summary>
    /// 현재 상태와 입력 및 조건을 기반으로 상태를 전환합니다.
    /// </summary>
    /// <param name="bPressedJump">점프 버튼이 눌렸는지 여부</param>
    private void StateChange(bool bPressedJump)
    {
        prevState = currentState;

        switch (currentState)
        {
            case CharacterFSMState.Idle:
                // 움직임 입력이 있으면 걷는 상태로 전환
                if (moveInput.sqrMagnitude > 0.0f)
                {
                    currentState = CharacterFSMState.Walk;
                }
                // 점프 버튼이 눌렸고 땅에 있으면 점프 상태로 전환
                if (bPressedJump && isGrounded)
                {
                    currentState = CharacterFSMState.Jump;
                }
                break;
            case CharacterFSMState.Walk:
                // 움직임 입력이 없으면 대기 상태로 전환
                if (moveInput.sqrMagnitude <= 0.0f)
                {
                    currentState = CharacterFSMState.Idle;
                }
                // 점프 버튼이 눌렸고 땅에 있으면 점프 상태로 전환
                if (bPressedJump && isGrounded)
                {
                    currentState = CharacterFSMState.Jump;
                }
                break;
            case CharacterFSMState.Jump:
                // 땅에 착지하면 대기 상태로 전환
                if (isGrounded)
                {
                    currentState = CharacterFSMState.Idle;
                }
                break;
        }
    }

    /// <summary>
    /// 새로운 상태로 진입할 때 수행되는 로직입니다.
    /// 예: 애니메이션 시작, 변수 초기화 등
    /// </summary>
    private void EnterState()
    {
        if (prevState != currentState)
        {
            switch (currentState)
            {
                case CharacterFSMState.Idle:
                    // 대기 상태 애니메이션
                    animator.SetFloat(Speed_Hash, 0.0f);
                    break;
                case CharacterFSMState.Walk:
                    // 걷기 상태 애니메이션
                    animator.SetFloat(Speed_Hash, 1.0f);
                    break;
                case CharacterFSMState.Jump:
                    // 점프 애니메이션 실행 및 점프 힘 적용
                    animator.CrossFade("Jump", 0.1f);
                    rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
                    break;
            }
        }
    }

    /// <summary>
    /// 이전 상태를 종료할 때 수행되는 로직입니다.
    /// 예: 애니메이션 종료 등
    /// </summary>
    private void ExitState()
    {
        if (prevState != currentState)
        {
            switch (prevState)
            {
                case CharacterFSMState.Jump:
                    // 점프 상태 종료 후 대기 상태 애니메이션으로 전환
                    animator.CrossFade("Idles", 0.1f);
                    break;
            }
        }
    }

    /// <summary>
    /// 현재 상태를 업데이트합니다.
    /// 예: 지속적인 움직임 로직 처리
    /// </summary>
    private void UpdateState()
    {
        switch (currentState)
        {
            case CharacterFSMState.Walk:
                // 움직임 입력에 따라 Rigidbody의 속도 설정
                rb.velocity = new Vector3(moveInput.x * moveSpeed, rb.velocity.y, moveInput.y * moveSpeed);
                break;
            case CharacterFSMState.Jump:
                // 점프 중에는 특별한 업데이트 로직 없음
                break;
        }
    }
}
