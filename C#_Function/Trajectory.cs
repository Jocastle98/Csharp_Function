public class Cannon : MonoBehaviour
{
    // 포탄 발사 시 적용할 힘 (N)
    public float Power = 500.0f;

    // 포탄의 질량 (kg)
    public float Mass = 10.0f;

    // 궤적 계산에서 최대 시뮬레이션 스텝 수
    public int maxStep = 20;

    // 각 시뮬레이션 스텝 간의 시간 간격 (s)
    public float timeStep = 0.1f;

    // 발사되는 포탄 Prefab
    public GameObject CannonBall;

    // 궤적을 표시할 작은 오브젝트 Prefab
    public GameObject Trajectory;

    // 궤적을 표시하는 오브젝트들의 리스트
    public List<GameObject> Objects = new List<GameObject>();

    // PredictTrajectory: 주어진 힘과 질량을 기반으로 궤적을 계산합니다.
    // - force: 포탄에 적용할 초기 힘
    // - mass: 포탄의 질량
    // - 반환값: 계산된 궤적의 위치 리스트
    List<Vector3> PredictTrajectory(Vector3 force, float mass)
    {
        List<Vector3> trajectory = new List<Vector3>(); // 궤적 좌표 리스트

        Vector3 position = transform.position; // 대포의 현재 위치
        Vector3 velocity = force / mass;       // 초기 속도 (F=ma, a=F/m)

        trajectory.Add(position); // 시작 위치를 궤적에 추가

        // 시뮬레이션 루프 (최대 maxStep 스텝까지 계산)
        for (int i = 1; i <= maxStep; i++)
        {
            float timeElapsed = timeStep * i; // 경과 시간 계산

            // 등가속도 운동 공식으로 다음 위치 계산
            trajectory.Add(position + 
                           velocity * timeElapsed + 
                           Physics.gravity * (0.5f * timeElapsed * timeElapsed));

            // 충돌 여부 확인
            if (CheckCollision(trajectory[i - 1], trajectory[i], out Vector3 hitPoint))
            {
                trajectory[i] = hitPoint; // 충돌 위치로 궤적 수정
                break; // 충돌 시 궤적 계산 중단
            }
        }

        return trajectory; // 계산된 궤적 반환
    }

    // CheckCollision: 두 위치 간의 충돌 여부를 확인합니다.
    // - start: 시작 위치
    // - end: 끝 위치
    // - hitPoint: 충돌 지점 (출력 값)
    // - 반환값: 충돌 여부 (true: 충돌 발생, false: 충돌 없음)
    private bool CheckCollision(Vector3 start, Vector3 end, out Vector3 hitPoint)
    {
        hitPoint = end; // 초기값 설정
        Vector3 direction = end - start; // 이동 방향 계산
        float distance = direction.magnitude; // 이동 거리 계산

        // Raycast로 충돌 감지
        if (Physics.Raycast(start, direction.normalized, out RaycastHit hit, distance, 1 << LayerMask.NameToLayer("Default")))
        {
            hitPoint = hit.point; // 충돌 지점 저장
            return true;
        }

        return false; // 충돌 없음
    }

    // Update: 매 프레임마다 대포 조작 및 궤적 시뮬레이션을 처리합니다.
    void Update()
    {
        // W 키를 눌러 대포를 위로 회전
        if (Input.GetKey(KeyCode.W))
        {
            transform.rotation *= Quaternion.Euler(-90 * Time.deltaTime, 0, 0);
        }
        // S 키를 눌러 대포를 아래로 회전
        if (Input.GetKey(KeyCode.S))
        {
            transform.rotation *= Quaternion.Euler(90 * Time.deltaTime, 0, 0);
        }

        // Z 키를 눌러 포탄 발사
        if (Input.GetKeyDown(KeyCode.Z))
        {
            GameObject go = Instantiate(CannonBall, transform.position, transform.rotation); // 포탄 생성
            go.GetComponent<Rigidbody>().mass = Mass; // 포탄의 질량 설정
            go.GetComponent<Rigidbody>().AddForce(transform.forward * Power, ForceMode.Impulse); // 힘 적용
            Destroy(go, 3.0f); // 3초 후 포탄 삭제
        }

        // Space 키를 눌러 궤적 시뮬레이션
        if (Input.GetKeyDown(KeyCode.Space))
        {
            List<Vector3> trajectorys = PredictTrajectory(transform.forward * Power, Mass); // 궤적 계산

            // 이전 궤적 오브젝트 삭제
            foreach (var o in Objects)
            {
                Destroy(o);
            }
            Objects.Clear();

            // 새로운 궤적 생성
            foreach (var trajectory in trajectorys)
            {
                var go = Instantiate(Trajectory, trajectory, Quaternion.identity); // 궤적 오브젝트 생성
                Objects.Add(go); // 리스트에 추가
            }
        }

        // 현재 궤적 오브젝트를 숨김
        foreach (var o in Objects)
        {
            o.SetActive(false);
        }

        // 새로 계산된 궤적을 업데이트하고 표시
        List<Vector3> trajectorys2 = PredictTrajectory(transform.forward * Power, Mass);

        if (Objects.Count == trajectorys2.Count)
        {
            for (var index = 0; index < trajectorys2.Count; index++)
            {
                var trajectory = trajectorys2[index];
                Objects[index].SetActive(true); // 오브젝트 활성화
                Objects[index].transform.position = trajectory; // 위치 업데이트
            }
        }
    }
}


