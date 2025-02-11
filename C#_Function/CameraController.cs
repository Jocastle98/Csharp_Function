
public class CameraController : MonoBehaviour
{
    public Transform target; // 따라갈 대상, 보통 Player의 Transform
    public float smoothSpeed = 0.125f; // 카메라의 부드러운 이동 속도

    private void LateUpdate()
    {
        if (!target) return; // 대상이 없을 경우 아무것도 하지 않음

        // 목표 위치 계산
        Vector3 desiredPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

        // 부드러운 카메라 이동 (Lerp 사용)
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // 카메라 위치 업데이트
        transform.position = smoothedPosition;
    }
}
