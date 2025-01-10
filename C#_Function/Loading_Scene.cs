슬라이더 / 텍스트 메쉬 프로를 생성 후 빈 게임 오브젝트에 있는 해당 스크립트에 할당 후 
실행하여 로딩창 구현


using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AsyncSceneLoader : MonoBehaviour
{
    public string sceneToLoad = "ActionCheck";  // 로딩할 씬 이름
    public Slider loadingSlider;
    public TextMeshProUGUI progressText;
    public float loadingSpeed = 0.5f;  // 로딩 속도 조절

    void Start()
    {
        LoadSceneAsync().Forget();
    }

    private async UniTask LoadSceneAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        operation.allowSceneActivation = false;  // 씬 자동 실행 방지

        float progress = 0f;

        while (!operation.isDone)
        {
            progress = Mathf.MoveTowards(progress, Mathf.Clamp01(operation.progress / 0.9f), Time.deltaTime * loadingSpeed);
            loadingSlider.value = progress;
            progressText.text = (progress * 100).ToString("F0") + "%";

            // 슬라이더가 가득 찼을 때 씬 실행
            if (progress >= 1f)
            {
                operation.allowSceneActivation = true;
            }

            await UniTask.Yield(PlayerLoopTiming.Update);
        }
    }
}
