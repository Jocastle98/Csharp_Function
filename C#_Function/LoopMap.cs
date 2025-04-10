2D 게임의 무한맵을 구현했습니다.

 Vector2 newOffset = materialInstance.mainTextureOffset + new Vector2(0f, offsetVal);
해당 부분에서 x,y를 바꾸면 수평으로 이동하는 무한맵이 완성됩니다.

https://velog.io/@jocastle98/%EB%A9%8B%EC%9F%81%EC%9D%B4%EC%82%AC%EC%9E%90%EC%B2%98%EB%9F%BC
-%EB%B6%80%ED%8A%B8%EC%BA%A0%ED%94%84-TIL-%ED%9A%8C%EA%B3%A0-%EC%9C%A0%EB%8B%88%ED%8B%B
0-%EA%B2%8C%EC%9E%84-%EA%B0%9C%EB%B0%9C-3%EA%B8%B0-9%EC%9D%BC%EC%B0%A8-Flappy-Bird-%EC%A0%9C
%EC%9E%912UI-%EC%82%AC%EC%9A%B4%EB%93%9C-Loop%EB%A7%B5-%EB%B0%B0%EC%97%B4%EA%B3%BC-%EB%A6%AC%EC%8A%A4%ED%8A%B8
해당 링크를 통해 자세한 세팅에 대해 알 수 있습니다.

---------------------------
public class LoopMap : MonoBehaviour
{
    private Material materialInstance;
    public float offsetSpeed = 0.5f;

    void Start()
    {
        SpriteRenderer sp = GetComponent<SpriteRenderer>();
        materialInstance = Instantiate(sp.material);
        sp.material = materialInstance;
    }

    void Update()
    {
        float offsetVal = offsetSpeed * Time.deltaTime;
        Vector2 newOffset = materialInstance.mainTextureOffset + new Vector2(0f, offsetVal);
        materialInstance.SetTextureOffset("_MainTex", newOffset);
        
    }
}

