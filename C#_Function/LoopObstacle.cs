
해당 코드를 통해 장애물이 계속 랜덤반 패턴을 통해 나타나도록 구현했습니다.

  https://velog.io/@jocastle98/%EB%A9%8B%EC%9F%81%EC%9D%B4%EC%82%AC%EC%9E%90%EC%B2%98%EB%9F%BC
-%EB%B6%80%ED%8A%B8%EC%BA%A0%ED%94%84-TIL-%ED%9A%8C%EA%B3%A0-%EC%9C%A0%EB%8B%88%ED%8B%B0-%EA%B2%8
  C%EC%9E%84-%EA%B0%9C%EB%B0%9C-3%EA%B8%B0-9%EC%9D%BC%EC%B0%A8-Flappy-Bird-%EC%A0%9C%EC%9E%912UI-
  %EC%82%AC%EC%9A%B4%EB%93%9C-Loop%EB%A7%B5-%EB%B0%B0%EC%97%B4%EA%B3%BC-%EB%A6%AC%EC%8A%A4%ED%8A%B8
  해당 링크를 통해 참고하면 좋습니다.

-----------------
public class RandomObstacle : MonoBehaviour
{
    public GameObject[] obstacles; 
    public float obstacleSpeed = 2f; 
    
    public enum Obstacle_TYPE
    {
        LEFT,
        RIGHT,
        MIDDLE,
        SIDE,
    }
    public Obstacle_TYPE obstacleType;

    void Start()
    {
        randomObstacle();
    }

    void Update()
    {
        for (int i = 0; i < obstacles.Length; i++)
        {
            obstacles[i].transform.position -= new Vector3(0f, obstacleSpeed * Time.deltaTime, 0f);
            if (obstacles[i].transform.position.y <= -6)
            {
                obstacles[i].transform.position = new Vector3(obstacles[i].transform.position.x, 6f, obstacles[i].transform.position.z );
                randomObstacle();
            }
        }
    }

    public void randomObstacle()
    {
        foreach (var obstacle in obstacles)
        {
            obstacle.SetActive(false);
        }

        int ranInt = Random.Range(0, 4);
        obstacleType = (Obstacle_TYPE)ranInt;

        
        switch (obstacleType)
        {
            case Obstacle_TYPE.LEFT:
                obstacles[0].SetActive(true); 
                break;

            case Obstacle_TYPE.RIGHT:
                obstacles[2].SetActive(true); 
                break;

            case Obstacle_TYPE.MIDDLE:
                obstacles[1].SetActive(true);
                break;

            case Obstacle_TYPE.SIDE:
                obstacles[0].SetActive(true);
                obstacles[2].SetActive(true);
                break;
        }
    }
}
