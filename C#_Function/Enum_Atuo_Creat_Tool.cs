Enum을 자동으로 생성해주는 툴 코드입니다.

interface로 나눠진 캐릭터의 상태 Idle, Walk, Jump와 같은 동작들을 
버튼 클릭으로 스크립트를 생성하여 enum을 생성합니다.
  
적용 하기 위해 각각의 상태 스크립트 클래스 위에 [State("IdleState")] 이러한 코드를 작성을 해줍니다.


public class StateManagerGeneratorWindow : EditorWindow
{
    // Unity Editor 메뉴에 'Tools/State Manager Generator' 메뉴 아이템을 추가하고, 클릭 시 해당 창을 띄움
    [MenuItem("Tools/State Manager Generator")]
    public static void ShowWindow()
    {
        // Unity의 EditorWindow를 생성하고 타이틀을 설정
        GetWindow<StateManagerGeneratorWindow>("State Manager Generator");
    }

    // Unity Editor 창의 GUI를 그리는 함수
    private void OnGUI()
    {
        // 'Generate' 버튼을 클릭 시 아래의 코드 실행
        if (GUILayout.Button("Generate"))
        {
            // Enum을 수집할 Dictionary 생성
            Dictionary<string, List<Type>> enums = new();
        
            // 현재 도메인의 모든 어셈블리를 가져와 검사
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                // 특정 Attribute(StateAttribute)를 가진 타입만 필터링
                var types = assembly.GetTypes().Where(t => t.GetCustomAttributes(typeof(StateAttribute), true).Length > 0);

                // 찾은 타입에 대해 반복문 수행
                foreach (var type in types)
                {
                    // StateAttribute를 가져와 StateName이 존재하는 경우
                    var attr = type.GetCustomAttribute<StateAttribute>();
                    if (attr != null && !string.IsNullOrEmpty(attr.StateName))
                    {
                        // Dictionary에 StateName이 존재하지 않으면 추가하고, 해당 타입을 저장
                        if (!enums.ContainsKey(attr.StateName))
                        {
                            enums.Add(attr.StateName, new List<Type>());
                        }
                        enums[attr.StateName].Add(type);
                    }
                }
            }

            // StateName을 알파벳 순으로 정렬
            var List = enums.Keys.OrderBy(s => s).ToList();
            var savePath = "Assets/02. Script/Generated";
            StringBuilder sb = new StringBuilder();
            
            // 해당 경로에 폴더가 없으면 생성
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            // 스크립트의 헤더를 추가
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            
            sb.AppendLine("public class StateTypesClasses");
            sb.AppendLine("{");
            // Enum을 정의하는 메서드를 호출
            GenerateFillEnumText(sb, List);

            // Dictionary에 타입과 Enum 매핑
            sb.AppendLine("\tprivate static readonly Dictionary<Type, StateTypes> TypeToState = new()");
            sb.AppendLine("\t{");
            
            // StateName과 해당하는 타입들을 순회하며 Dictionary에 추가
            foreach (var se in List)
            {
                var orderedList = enums[se].OrderBy(t => t.Name).ToList();
                foreach (var type in orderedList)
                {
                    sb.AppendLine($"\t\t[typeof({type})] = StateTypes.{se},");
                }
            }
            sb.AppendLine("\t};");
            
            // GetState 메서드 작성
            sb.AppendLine("\tpublic static StateTypes GetState<T>() => GetState(typeof(T));");
            sb.AppendLine("\tpublic static StateTypes GetState(Type type )");
            sb.AppendLine("\t{");
            sb.AppendLine("\t\treturn TypeToState.GetValueOrDefault(type, StateTypes.None);");
            sb.AppendLine("\t}");
            
            sb.AppendLine("}");
            
            // 파일을 생성하고 해당 경로에 저장
            string filePath = Path.Combine(savePath, "StateTypesClasses.cs");
            File.WriteAllText(filePath, sb.ToString());
        }
    }

    // Enum을 정의하는 메서드
    private static void GenerateFillEnumText(StringBuilder sb, List<string> List)
    {
        sb.AppendLine("\tpublic enum StateTypes");
        sb.AppendLine("\t{");
        sb.AppendLine("\t\tNone,");
        foreach (var se in List)
        {
            sb.AppendLine($"\t\t{se},");
        }
        sb.AppendLine("\t\tMax");
        sb.AppendLine("\t}");
    }
}
