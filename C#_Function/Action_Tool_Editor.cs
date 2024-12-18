애니메이션과 다르게 애니메이션의 길이나 연결 다양하게 제작 가능


// ActionToolEditor: Unity 에디터에서 타임라인 기반의 애니메이션 노드를 생성하고 관리할 수 있는 사용자 정의 에디터 윈도우입니다.
// 각 애니메이션 노드에는 이름, 시작 시간, 지속 시간이 설정되며, 타임라인 상에 시각적으로 표시됩니다.
// 타겟 오브젝트에 Animator가 있을 경우, 노드의 애니메이션이 재생됩니다.
// ActionNodeBase: 모든 노드의 기본 클래스
public class ActionNodeBase
{
}

// AnimationNode: 애니메이션 노드를 정의하는 클래스
// - name: 애니메이션 이름
// - startTime: 타임라인 상의 시작 시간
// - duration: 애니메이션의 지속 시간
public class AnimationNode : ActionNodeBase
{
    public string name;       // 애니메이션 이름
    public float startTime;   // 시작 시간
    public float duration;    // 지속 시간
}

// ActionToolEditor: 사용자 정의 에디터 윈도우 클래스
// 애니메이션 노드를 타임라인에 추가하고 관리하는 UI를 제공
[CustomEditor(typeof(ActionToolEditor))]
public class ActionToolEditor : EditorWindow
{
    private List<ActionNodeBase> nodes = new List<ActionNodeBase>(); // 노드 목록
    private ActionNodeBase selectedNode;                             // 선택된 노드
    
    public GameObject targetObject;        // 타겟 오브젝트
    private float timeScale = 100f;        // 타임라인 시간 스케일 (1초당 픽셀 수)
    private float maxTime = 10.0f;         // 타임라인 최대 시간
    
    private float timelineWidth = 100f;    // 타임라인 너비
    private float timelineHeight = 200f;   // 타임라인 높이
    private float currentTime = 0f;        // 현재 시간 표시
    private Vector2 scrollPosition;        // 스크롤 뷰 위치

    // Unity 메뉴에 "Window -> Action -> ActionTool" 메뉴 항목 추가
    [MenuItem("Window/Action/ActionTool")]
    public static void ShowWindow()
    {
        GetWindow<ActionToolEditor>("ActionTool"); // 에디터 윈도우를 생성
    }

    // AddAnimationNode: 애니메이션 노드를 추가하는 함수
    // - name: 노드 이름
    // - duration: 노드 지속 시간
    private void AddAnimationNode(string name, float duration)
    {
        nodes.Add(new AnimationNode() { name = name, startTime = currentTime, duration =  duration });
    }
    
    // ShowAddNodeMenu: 노드를 추가하는 메뉴를 표시
    private void ShowAddNodeMenu()
    {
        AddAnimationNode("empty", 0); // 이름과 지속 시간을 초기화
    }

    // OnGUI: 에디터 윈도우의 GUI를 그리는 함수
    private void OnGUI()
    {
        EditorGUILayout.BeginVertical(); // 세로 레이아웃 시작
        
        // 타겟 오브젝트 필드 생성
        targetObject = EditorGUILayout.ObjectField("Target Object", targetObject, typeof(GameObject), true) as GameObject;
        
        // 타임 스케일과 최대 시간 입력 필드
        timeScale = EditorGUILayout.FloatField("Time Scale", timeScale);
        maxTime = EditorGUILayout.FloatField("Max Time", maxTime);
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Timeline", EditorStyles.boldLabel); // 타임라인 레이블

        // 스크롤 뷰 시작
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        
        // 타임라인 직사각형 영역 설정
        Rect timelineRect = GUILayoutUtility.GetRect(timeScale * maxTime, timelineHeight);
        
        DrawTimelineText(timelineRect); // 타임라인의 텍스트 표시

        timelineRect.y += 20; // 텍스트 영역 아래로 이동
        
        GUI.Box(timelineRect, ""); // 타임라인 배경 그리기

        DrawTimelineGrid(timelineRect);  // 타임라인 그리드 표시
        DrawCurrentTimeline(timelineRect); // 현재 시간 표시선

        DrawNodes(); // 노드 그리기

        HandleTimelineInput(timelineRect); // 타임라인 마우스 입력 처리
        
        EditorGUILayout.EndScrollView(); // 스크롤 뷰 종료
        
        EditorGUILayout.EndVertical(); // 세로 레이아웃 종료

        Repaint(); // 에디터 창을 새로고침
        
        // 노드 추가 버튼 표시
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Animation"))
        {
            ShowAddNodeMenu();
        }
        EditorGUILayout.EndHorizontal();

        // 선택된 노드의 정보를 편집할 수 있는 필드 표시
        if (selectedNode is AnimationNode animationNode)
        {
            animationNode.name = EditorGUILayout.TextField("Name", animationNode.name);
            animationNode.startTime = EditorGUILayout.FloatField("StartTime", animationNode.startTime);
            animationNode.duration = EditorGUILayout.FloatField("Duration", animationNode.duration);
        }
    }

    // DrawNodes: 타임라인에 노드를 그리는 함수
    private void DrawNodes()
    {
        int space = 10; // 노드 간격 설정

        foreach (var actionNodeBase in nodes)
        {
            AnimationNode animNode = actionNodeBase as AnimationNode; // AnimationNode로 형변환
            if (animNode != null)
            {
                float x = (animNode.startTime * timeScale); // 노드 시작 위치 계산
                
                // 노드 버튼을 그리기
                if (GUI.Button(new Rect(x, space, animNode.duration * timeScale + 30, 30), animNode.name))
                {
                    selectedNode = animNode; // 노드 선택
                }

                // 현재 시간이 노드의 시간 범위 안에 있을 경우 애니메이션 재생
                if (currentTime >= animNode.startTime && currentTime <= animNode.startTime + animNode.duration)
                {
                    if (targetObject)
                    {
                        if (targetObject.TryGetComponent<Animator>(out var animator))
                        {
                            Debug.Log($"{animNode.name} {currentTime - animNode.startTime} {animNode.duration}");
                            
                            animator.Play(animNode.name, 0, (currentTime - animNode.startTime) / animNode.duration);
                            animator.Update(0.0f); // 애니메이션 업데이트
                        }
                    }   
                }
            }

            space += 30; // 다음 노드를 위해 간격 추가
        }
    }

    // DrawTimelineText: 타임라인의 시간을 나타내는 텍스트를 그리는 함수
    void DrawTimelineText(Rect timelineRect)
    {
        float secondWidth = timeScale;
        float totalSeconds = maxTime;

        for (int i = 0; i < totalSeconds; i++)
        {
            float x = timelineRect.x + (secondWidth * i); // 각 초의 위치 계산
            GUI.Label(
                new Rect(x - 15, timelineRect.y, 30, 15),
                i.ToString("F1"),
                new GUIStyle(EditorStyles.miniLabel) { alignment = TextAnchor.MiddleCenter }
            );
        }
    }

    // DrawTimelineGrid: 타임라인의 격자를 그리는 함수
    void DrawTimelineGrid(Rect timelineRect)
    {
        float secondWidth = timeScale;
        float totalSeconds = maxTime;

        for (int i = 0; i < totalSeconds; i++)
        {
            float x = timelineRect.x + (secondWidth * i); // 그리드 위치 계산
            Handles.DrawLine(new Vector3(x, timelineRect.y, 0),
                new Vector3(x, timelineRect.y + timelineRect.height, 0));
        }
    }

    // DrawCurrentTimeline: 현재 시간을 나타내는 빨간색 라인을 그리는 함수
    void DrawCurrentTimeline(Rect timelineRect)
    {
        float x = timelineRect.x + (currentTime * timeScale); // 현재 시간 위치 계산
        Handles.color = Color.red;
        Handles.DrawLine(new Vector3(x, timelineRect.y), new Vector3(x, timelineRect.y + timelineRect.height));
        Handles.color = Color.white; // 기본 색상 복원
    }

    // HandleTimelineInput: 타임라인 상에서 마우스 입력을 처리하는 함수
    void HandleTimelineInput(Rect timelineRect)
    {
        Event e = Event.current; // 현재 이벤트 가져오기
        if (timelineRect.Contains(e.mousePosition))
        {
            if (e.type == EventType.MouseDown || e.type == EventType.MouseDrag && e.button == 0)
            {
                float clickPosition = e.mousePosition.x - timelineRect.x; // 클릭한 위치 계산
                currentTime = Mathf.Clamp(clickPosition / timeScale, 0f, timelineRect.width); // 시간 범위 제한
                
                e.Use(); // 이벤트 사용 처리
                Repaint(); // 에디터 창 새로고침
            }
        }
    }
}
