
해당 코드는 코루틴을 이용하여 기존 위치를 가진 오브젝트를 UI상에 있는 다른 위치로 이동을 하며
Visualize하는 방식입니다.
코루틴을 통해 사용햇으며 해당 코루틴 방식을 통해 다른것에도 쉽게 적용을 할 수 있습니다.

또한, UI에 있는 특정위치로 이동 시키기 위헤
월드좌표 -> 스크린좌표 ->   로컬좌표 
3단계 변화를 줍니다.


public class ItemGetter : MonoBehaviour
{
    // 아이템이 이동할 박스의 RectTransform
    public RectTransform itemRectTransform;

    // 아이템과 이펙트가 표시될 UI 캔버스
    public Canvas canvas;

    // 아이템 프리팹 (UI Image 형태로 복사될 대상)
    public GameObject itemPrefab;

    // UI 좌표 변환에 사용할 카메라
    public Camera camera;
    
    // 아이템을 박스 위치로 이동시키는 코루틴
    IEnumerator GoingTBox(RectTransform itemTransform, RectTransform boxTransform)
    {
        // 이동에 걸리는 시간
        float duration = 1.0f;

        // 현재 경과 시간
        float t = 0.0f;

        // 아이템의 초기 위치 저장
        Vector3 itemBeginPOS = itemTransform.position;

        // 아이템을 박스까지 선형 보간으로 이동
        while (1.0f >= t / duration)
        {
            // 선형 보간을 통해 새 위치 계산
            Vector3 newPosition = Vector3.Lerp(itemBeginPOS, 
                boxTransform.position, t / duration);

            // 아이템 위치 갱신
            itemTransform.position = newPosition;

            // 경과 시간 증가
            t += Time.deltaTime;
            yield return null;
        }

        // 최종적으로 박스 위치로 설정
        itemTransform.position = boxTransform.position;

        // 아이템 오브젝트 삭제
        Destroy(itemTransform.gameObject);
    }

    // 트리거 충돌 시 호출되는 함수
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 충돌한 오브젝트 로그 출력
        Debug.Log(other);

        // 새로운 아이템 오브젝트 생성
        var newObject = Instantiate(itemPrefab, other.transform.position, Quaternion.identity, canvas.transform);

        // 충돌한 오브젝트의 스프라이트를 복사하여 새로운 아이템에 적용
        newObject.GetComponent<Image>().sprite = other.GetComponent<SpriteRenderer>().sprite;

        // 아이템 오브젝트의 초기 위치 설정
        newObject.transform.position = other.transform.position;

        // 월드 좌표를 스크린 좌표로 변환
        var newScreenPosition = Camera.main.WorldToScreenPoint(newObject.transform.position);

        // 스크린 좌표를 로컬 UI 좌표로 변환
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), newScreenPosition, camera, out localPoint);

        // 변환된 좌표를 새로운 아이템의 로컬 위치로 설정
        newObject.transform.localPosition = localPoint;

        // 아이템을 박스로 이동시키는 코루틴 실행
        StartCoroutine(GoingTBox(newObject.GetComponent<RectTransform>(), itemRectTransform));

        // 충돌한 오브젝트 삭제
        Destroy(other.gameObject);
    }
}
