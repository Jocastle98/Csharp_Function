
public class JocastleTimer : MonoBehaviour
{
    [Serializable]
    public class FillSettings
    {
        public Color color;
    }

    public FillSettings fillSettings;

    [Serializable]
    public class BackgroundSettings
    {
        public Color color;
    }
    public BackgroundSettings backgroundSettings;

    [SerializeField] private Image fillImage;
    [SerializeField] private float totalTime;
    [SerializeField] private TMP_Text timeText;
    
    public float CurrentImer{get; private set;}
    private bool _isPaused;

    private float elapsedTime;  // 1초 체크용
    void Update()
    {
        if (!_isPaused)
        {
            CurrentImer += Time.deltaTime;
            
            fillImage.fillAmount = 0;
            fillImage.fillAmount = CurrentImer / totalTime;
            
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= 1f)
            {
                elapsedTime = 0f;

                float remainTime = totalTime - CurrentImer;
                if (remainTime < 0f) remainTime = 0f;

                // 1초 단위 정수 감소
                timeText.text = Mathf.CeilToInt(remainTime).ToString();
            }
        }
    }
    
    public void StartTimer()
    {
        _isPaused = false;
    }

    public void PuaseTimer()
    {
        _isPaused = true;
    }

    public void ResetTimer()
    {
        CurrentImer = 0;
        fillImage.fillAmount = 1;
    }
}
