싱글톤을 통해 오디오의 전체적인 부분을 관리했습니다.

//오디오가 실행되어야 하는 효과음 부분에는 실행될 타이밍 코드에 해당 인스턴스 함수를 호출하여 실행합니다.
//AudioManager.Instance.PlaySound(5);  
  
// AudioManager: 게임 내 오디오를 중앙에서 관리하는 싱글톤 클래스
public class AudioManager : MonoBehaviour
{
    // 싱글톤 인스턴스. AudioManager는 전역적으로 하나만 존재하도록 보장.
    public static AudioManager Instance { get; private set; }
    
    // AudioSource 컴포넌트: 오디오 재생에 사용
    private AudioSource audioSource;
    
    // 오디오 클립 배열: 다양한 상황에서 사용할 사운드 파일들
    public AudioClip[] audioClip; 

    private void Awake()
    {
        // 싱글톤 패턴 구현
        if (Instance == null)
        {
            Instance = this; // 현재 오브젝트를 싱글톤 인스턴스로 설정
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 오브젝트 유지
        }
        else
        {
            Destroy(gameObject); // 중복된 AudioManager 오브젝트가 생성되면 삭제
        }
    }

    private void Start()
    {
        // AudioSource 컴포넌트 가져오기
        audioSource = GetComponent<AudioSource>();
        
        // 게임 시작 시 인트로 BGM 재생
        IntroBGM();
    }

    // 인트로 BGM 재생 메서드
    public void IntroBGM()
    {
        audioSource.clip = audioClip[0]; // 배열의 0번째 요소(인트로 BGM) 설정
        audioSource.loop = true; // BGM이 반복 재생되도록 설정
        audioSource.Play(); // BGM 재생
    }

    // 메인 BGM 재생 메서드
    public void MainBGM()
    {
        audioSource.clip = audioClip[1]; // 배열의 1번째 요소(메인 BGM) 설정
        audioSource.loop = true; // BGM이 반복 재생되도록 설정
        audioSource.Play(); // BGM 재생
    }

    // 효과음을 재생하는 메서드
    public void PlaySound(int index)
    {
        // 유효한 배열 인덱스인지 확인 후 효과음 재생
        if (index >= 2 && index < audioClip.Length && audioClip[index] != null)
        {
            audioSource.PlayOneShot(audioClip[index]); // 효과음을 한 번 재생
        }
    }
}
