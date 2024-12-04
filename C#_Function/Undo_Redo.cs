public class StackExaple : MonoBehaviour
{
   
    void Start()
    {
        position_stack.Push(transform.position);
    }
    
    private Stack<Vector3> position_stack = new Stack<Vector3>();
    private Stack<Vector3> reposition_stack = new Stack<Vector3>();
    
    [SerializeField]
    private float Speed = 3f;
    void Update()
    {
        Vector3 movePos = Vector3.zero;

        
        //이동 구현
        if (Input.GetKey(KeyCode.W))
        {
            movePos += transform.forward;                                                                                                                               
        }
        if (Input.GetKey(KeyCode.S))
        {
            movePos -= transform.forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            movePos -= transform.right;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movePos += transform.right;
        }
        
        //움직였던 정보를 기록하기 위해 키를 뗄때마다 위치를 기록
        if (Input.GetKeyDown(KeyCode.W) ||
            Input.GetKeyDown(KeyCode.S) ||
            Input.GetKeyDown(KeyCode.A) ||
            Input.GetKeyDown(KeyCode.D))
        {
            movePos = Vector3.zero;
            position_stack.Push(transform.position);
            
        }
        //왔던 포지션으로 되돌아가는 코드
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(position_stack.Count>0)
                reposition_stack.Push(transform.position);
                transform.position = position_stack.Pop();
                
        }
        //마지막 위치로 되돌아가는 코드
        if (Input.GetKeyDown(KeyCode.R))
        {
            if(reposition_stack.Count>0)
                position_stack.Push(transform.position);
                transform.position = reposition_stack.Pop();
        }
        transform.position += movePos.normalized *Speed* Time.deltaTime;     
    }
}

