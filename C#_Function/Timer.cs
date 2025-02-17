

public class JocastleTimer : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private float totalTime;
    [SerializeField] private Image headCapImage;
    [SerializeField] private Image tailCapImage;
    [SerializeField] private TMP_Text timeText;
    
    
    public float CurrentImer{get; private set;}
    private bool _isPaused;

    void Awake()
    {
        //타이머 멈춰있음
        _isPaused = true;
    }
    
    void Update()
    {
        if (!_isPaused)
        {
            CurrentImer += Time.deltaTime;
            if (CurrentImer >= totalTime)
            {
                headCapImage.gameObject.SetActive(false);
                tailCapImage.gameObject.SetActive(false);
                _isPaused = true;
            }
            else
            {
                fillImage.fillAmount = (totalTime- CurrentImer) / totalTime;
                headCapImage.transform.localRotation 
                    = Quaternion.Euler(new Vector3(0,0,fillImage.fillAmount * 360));
                
                
                var timeTextTime = totalTime - CurrentImer;
                //F0: 소수 점 자리 표현 x
                timeText.text = timeTextTime.ToString("F0");
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

    public void InitTimer()
    {
        CurrentImer = 0;
        fillImage.fillAmount = 1;
        timeText.text = totalTime.ToString("F0");
        headCapImage.gameObject.SetActive(true);
        tailCapImage.gameObject.SetActive(true);
        _isPaused = false;
    }
}
