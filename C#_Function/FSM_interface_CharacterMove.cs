인터 페이스를 통해 IState로 상태를 3가지로 나눕니다.
public interface IState
{
    void Enter();
    void Execute();
    void Exit();
}

--------------------------

IState를 상속받아 IdleState에서 적용을 합니다.
  먼저 Enter로 Idle 애니메이션을 실행합니다.
  Excute에서는 캐릭터의 상태를 변화하는 ChangeState를 통해 변화할 때의 조건을 추가합니다.
public class IdleState : IState
{
    private Character character;

    public IdleState(Character character)
    {
        this.character = character;
    }

    public void Enter()
    {
        Debug.Log("Entering Idle State");
        character.Animator.Play("Idle");
    }

    public void Execute()
    {
        Vector2 movementInput = character.GetMovementInput();
        if (movementInput.magnitude > 0.1f)
        {
            character.ChangeState(new WalkState(character));
        }
        if (character.IsJumpPressed())
        {
            character.ChangeState(new JumpState(character));
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Idle State");
    }
}


--------------------------

IdleState와 마찬가지로 진행합니다.

public class WalkState : IState
{
    private Character character;

    public WalkState(Character character)
    {
        this.character = character;
    }

    public void Enter()
    {
        Debug.Log("Entering Walk State");
        character.Animator.Play("Walk");
    }

    public void Execute()
    {
        Vector2 movementInput = character.GetMovementInput();
        character.Move(new Vector3(movementInput.x, 0, movementInput.y));

        if (movementInput.magnitude < 0.1f)
        {
            character.ChangeState(new IdleState(character));
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Walk State");
    }
}


--------------------------



public class JumpState : IState
{
    private Character character;

    public JumpState(Character character)
    {
        this.character = character;
    }

    public void Enter()
    {
        Debug.Log("Entering Jump State");
        character.Animator.Play("Jump");
        character.Jump();
    }

    public void Execute()
    {
        if (character.IsGrounded())
        {
            character.ChangeState(new IdleState(character));
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Jump State");
    }
}


--------------------------

캐릭터의 움직임에 관한 내용입니다.
  먼저 초기 상태를 IdleState로 맞추며 매 프레임을 호출하는 Update에서 현재 State의 Execute를 호출합니다.
  ChangeState에서 현재 상태를 다른 newState로 변형합니다.

public class Character : MonoBehaviour
{
    public Animator Animator;
    private IState currentState;
    private Rigidbody rb;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
    }

    private void Start()
    {
        ChangeState(new IdleState(this));
    }

    private void Update()
    {
        currentState.Execute();
    }

    public void ChangeState(IState newState)
    {
        if (currentState != null)
            currentState.Exit();

        currentState = newState;
        currentState.Enter();
    }

    public Vector2 GetMovementInput()
    {
        return moveAction.ReadValue<Vector2>();
    }

    public bool IsJumpPressed()
    {
        return jumpAction.triggered;
    }

    public void Move(Vector3 direction)
    {
        transform.Translate(direction * Time.deltaTime * 5f);
    }

    public void Jump()
    {
        rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }
}
