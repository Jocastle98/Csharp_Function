# 전체 진행 순서
Character 초기화:

StateMachine 객체를 생성하고 IdleState로 초기화.
Unity의 Update 메서드에서 매 프레임 UpdateState 호출.
Idle 상태:
점프 키를 누르면 JumpState로 전환.
이동 입력이 있으면 WalkState로 전환.
  
Walk 상태:
점프 키를 누르면 JumpState로 전환.
이동 입력이 없으면 IdleState로 전환.
  
Jump 상태:착지하면 IdleState로 전환.

  
# 역할 요약
IState: 상태 인터페이스. 모든 상태 클래스는 이를 구현.
StateMachine: 상태 관리와 전환을 담당.
IdleState: 대기 상태. 입력에 따라 상태 전환.
WalkState: 이동 상태. 입력 변화에 따라 전환.
JumpState: 점프 상태. 착지 시 전환.
Character: FSM 초기화와 Unity 라이프사이클 연결.


----------
/// <summary>
/// 모든 상태가 구현해야 하는 공통 인터페이스.
/// 각 상태는 Enter, UpdateState, Exit 메서드를 통해 상태별 동작을 정의.
/// </summary>
public interface IState
{
    void Enter();               // 상태에 진입할 때 호출
    void UpdateState(float dt); // 상태를 업데이트할 때 호출
    void Exit();                // 상태에서 벗어날 때 호출
}

----------
/// <summary>
/// 상태 전환 및 현재 상태를 관리하는 상태 머신.
/// 상태를 변경하거나 매 프레임 상태를 업데이트하는 역할.
/// </summary>
public class StateMachine
{
    private IState currentState; // 현재 활성화된 상태

    /// <summary>
    /// 새로운 상태로 전환합니다.
    /// </summary>
    public void ChangeState(IState newState)
    {
        currentState?.Exit();  // 이전 상태의 Exit 호출
        currentState = newState; // 새로운 상태 설정
        currentState.Enter();  // 새로운 상태의 Enter 호출
    }

    /// <summary>
    /// 현재 상태를 업데이트합니다.
    /// </summary>
    public void UpdateState(float dt)
    {
        currentState?.UpdateState(dt); // 현재 상태의 Update 호출
    }
}

----------
  
/// <summary>
/// 캐릭터가 아무 행동도 하지 않을 때의 상태.
/// </summary>
public class IdleState : IState
{
    private StateMachine fsm;
    private Animator animator;

    public IdleState(StateMachine fsm, Animator animator)
    {
        this.fsm = fsm;
        this.animator = animator;
    }

    public void Enter()
    {
        animator.CrossFade("Idle", 0.1f); // Idle 애니메이션 실행
    }

    public void UpdateState(float dt)
    {
        // 점프 키 입력 시 Jump 상태로 전환
        if (Input.GetKeyDown(KeyCode.Space))
            fsm.ChangeState(new JumpState(fsm, animator));
        // 이동 입력이 있으면 Walk 상태로 전환
        else if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            fsm.ChangeState(new WalkState(fsm, animator));
    }

    public void Exit() { }
}


----------

/// <summary>
/// 캐릭터가 움직이는 상태.
/// </summary>
public class WalkState : IState
{
    private StateMachine fsm;
    private Animator animator;

    public WalkState(StateMachine fsm, Animator animator)
    {
        this.fsm = fsm;
        this.animator = animator;
    }

    public void Enter()
    {
        animator.CrossFade("Walk", 0.1f); // Walk 애니메이션 실행
    }

    public void UpdateState(float dt)
    {
        // 점프 키 입력 시 Jump 상태로 전환
        if (Input.GetKeyDown(KeyCode.Space))
            fsm.ChangeState(new JumpState(fsm, animator));
        // 이동 입력이 없으면 Idle 상태로 전환
        else if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
            fsm.ChangeState(new IdleState(fsm, animator));
    }

    public void Exit() { }
}

----------

/// <summary>
/// 캐릭터가 점프 중인 상태.
/// </summary>
public class JumpState : IState
{
    private StateMachine fsm;
    private Animator animator;

    public JumpState(StateMachine fsm, Animator animator)
    {
        this.fsm = fsm;
        this.animator = animator;
    }

    public void Enter()
    {
        animator.CrossFade("Jump", 0.1f); // Jump 애니메이션 실행
    }

    public void UpdateState(float dt)
    {
        // 착지하면 Idle 상태로 전환
        if (IsGrounded())
            fsm.ChangeState(new IdleState(fsm, animator));
    }

    private bool IsGrounded()
    {
        // 간단한 착지 로직 (임시)
        return Input.GetKeyDown(KeyCode.LeftShift); // 임시 조건
    }

    public void Exit() { }
}

----------


/// <summary>
/// 캐릭터의 FSM을 초기화하고 업데이트하는 MonoBehaviour 스크립트.
/// Unity의 라이프사이클 메서드를 통해 FSM 동작.
/// </summary>
public class Character : MonoBehaviour
{
    private StateMachine stateMachine; // 상태 머신
    private Animator animator;         // 애니메이터

    void Start()
    {
        animator = GetComponent<Animator>();
        stateMachine = new StateMachine();

        // 초기 상태를 Idle로 설정
        stateMachine.ChangeState(new IdleState(stateMachine, animator));
    }

    void Update()
    {
        // 상태 머신 업데이트 (현재 상태의 UpdateState 호출)
        stateMachine.UpdateState(Time.deltaTime);
    }
}
