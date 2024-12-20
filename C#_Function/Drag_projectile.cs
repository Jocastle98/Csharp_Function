앵그리버드 처럼 새총을 당겨 발사체를 발사하는 로직

// 이 스크립트는 Unity 씬에서 객체를 드래그하는 메커니즘을 처리합니다.
// 포인터 동작(클릭, 드래그, 해제)에 응답하기 위해 Unity의 이벤트 인터페이스를 구현합니다.
// LineRenderer를 사용하여 당기는 방향을 시각화하고 드래그 거리를 제한합니다.

public class DraggableObject : MonoBehaviour, 
    IPointerDownHandler,  // 포인터 클릭 이벤트를 처리합니다.
    IPointerUpHandler,    // 포인터 해제 이벤트를 처리합니다.
    IDragHandler          // 드래그 이벤트를 처리합니다.
{ 
    // 초기 위치와 현재 당기는 위치를 저장하는 변수입니다.
    private Vector3 startPosition;
    private Vector3 pullPosition;
    
    // 화면 좌표를 월드 좌표로 변환하기 위해 메인 카메라를 참조합니다.
    private Camera MainCamera;
 
    [SerializeField] private LineRenderer lineRenderer; // 당기는 방향을 시각화하는 LineRenderer입니다.
    [SerializeField] private float maxPullDistance;     // 허용되는 최대 당김 거리입니다.

    public GameObject MetalPrefab; // 발사체로 사용할 오브젝트 프리팹입니다.
    public float speed = 100f;     // 발사체의 발사 속도를 조절하는 변수입니다.

    public Transform firePos; // 발사체가 발사될 위치를 나타내는 트랜스폼입니다.

    // 스크립트 인스턴스가 로드될 때 호출됩니다.
    private void Awake()
    {
        MainCamera = Camera.main; // 메인 카메라를 참조합니다.
    }

    // 객체가 클릭되었을 때 호출됩니다.
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
        // 포인터 위치를 기준으로 월드 좌표에서 시작 위치를 기록합니다.
        pullPosition = startPosition = MainCamera.ScreenToWorldPoint(
            new Vector3(eventData.position.x, 
                eventData.position.y, 
                MainCamera.WorldToScreenPoint(transform.position).z));
    }

    // 포인터가 해제되었을 때 호출됩니다.
    public void OnPointerUp(PointerEventData eventData)
    {
        // 포인터의 월드 좌표를 계산합니다.
        Vector3 mouseWorldPos = MainCamera.ScreenToWorldPoint(
            new Vector3(eventData.position.x, 
                eventData.position.y, 
                MainCamera.WorldToScreenPoint(transform.position).z));

        // 현재 위치를 기준으로 방향을 계산합니다.
        Vector3 startPosition1 = new Vector3(0, 0, 0); // 초기 지점을 원점으로 설정합니다.
        Vector3 endPosition = mouseWorldPos; // 드래그가 끝난 위치입니다.
        Vector3 powerDirection = startPosition1 - endPosition; // 발사체가 날아갈 방향을 계산합니다.
        
        // 발사체를 생성합니다.
        GameObject projectile = Instantiate(MetalPrefab, powerDirection, Quaternion.identity);
        
        // 발사체에 Rigidbody가 있는지 확인합니다.
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // 힘의 크기와 방향을 설정합니다.
            Vector3 force = powerDirection.normalized * speed * powerDirection.magnitude;

            // AddForce를 사용해 발사체에 힘을 가합니다.
            rb.AddForce(force, ForceMode.Impulse); // Impulse 모드를 사용하여 순간적인 힘을 가합니다.
        }

        // 드래그 후 객체와 LineRenderer를 초기화합니다.
        transform.position = startPosition; // 객체를 초기 위치로 이동시킵니다.
        lineRenderer.SetPosition(1, startPosition); // LineRenderer의 끝점을 초기 위치로 설정합니다.
    }


    // 객체를 드래그하는 동안 지속적으로 호출됩니다.
    public void OnDrag(PointerEventData eventData)
    {
        if (Camera.main != null)
        {
            // 포인터 위치를 월드 좌표로 변환합니다.
            Vector3 mouseWorldPos = MainCamera.ScreenToWorldPoint(
                new Vector3(eventData.position.x, 
                eventData.position.y, 
                MainCamera.WorldToScreenPoint(transform.position).z));
            
            // 시작 위치와 현재 포인터 위치를 기준으로 당기는 방향을 계산합니다.
            Vector3 pullDirection = startPosition - mouseWorldPos;

            // LineRenderer의 끝점 위치입니다.
            Vector3 LinePosition1 = Vector3.zero;
            
            // 당김 거리가 허용된 최대치를 초과하면 당김 방향을 조정합니다.
            if (pullDirection.magnitude > maxPullDistance)
            {
                pullDirection = pullDirection.normalized * maxPullDistance;
                LinePosition1 = startPosition - pullDirection;
            }
            else
            {
                LinePosition1 = mouseWorldPos;
            }
            
            // 객체의 위치를 포인터를 따라가도록 업데이트합니다.
            transform.position = mouseWorldPos;
            
            // LineRenderer의 끝점 위치를 업데이트합니다.
            lineRenderer.SetPosition(1, LinePosition1);
        }
    }
}
