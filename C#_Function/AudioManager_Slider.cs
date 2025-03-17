Setting Panel에서 슬라이더를 적용하여 해당 Audio를 재생하도록 구현

using UnityEngine;
using UnityEngine.UI;

public class SettingPanelController : PopupPanelController
{

    [SerializeField] private Slider _bgmSlider;
    [SerializeField] private Slider _sfxSlider;

    private void OnEnable()
    {
        AudioManager.Instance.InitSliders(_bgmSlider, _sfxSlider);
    }
    public void OnClickClosedButton()
    {
        Hide();
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Audio Sources")]
    // BGM과 SFX용 AudioSource. Inspector에서 할당하거나, 같은 GameObject에 두 개의 AudioSource 컴포넌트를 추가합니다.
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    // AudioClip 배열: 
    // - 인덱스 0 : 인트로 BGM
    // - 인덱스 1 : 메인 BGM
    // - 인덱스 2 이상 : 효과음(SFX)
    public AudioClip[] audioClips;

    // PlayerPrefs에 저장할 키들
    private const string BGMVolumeKey = "BGMVolume";
    private const string SFXVolumeKey = "SFXVolume";

        private void Awake()
    {
        float bgmVolume = PlayerPrefs.GetFloat(Constants.BGMVolumeKey, 1f);
        float sfxVolume = PlayerPrefs.GetFloat(Constants.SFXVolumeKey, 1f);

        if (_bgmSource != null)
        {
            _bgmSource.volume = bgmVolume;
        }
        if (_sfxSource != null)
        {
            _sfxSource.volume = sfxVolume;
        }


    }

    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
    }

    /// <summary>
    /// SettingPanel 에서 슬라이더 값 변경시 호출
    /// </summary>
    public void InitSliders(Slider bgmSlider, Slider sfxSlider)
    {
        float bgmVolume = PlayerPrefs.GetFloat(Constants.BGMVolumeKey, 1f);
        float sfxVolume = PlayerPrefs.GetFloat(Constants.SFXVolumeKey, 1f);

        if (bgmSlider != null)
        {
            bgmSlider.value = bgmVolume;
            bgmSlider.onValueChanged.AddListener(OnBgmVolumeChanged);
        }
        if (sfxSlider != null)
        {
            sfxSlider.value = sfxVolume;
            sfxSlider.onValueChanged.AddListener(OnSfxVolumeChanged);
        }
    }

    public void PlayIntroBgm()
    {
        if (_bgmSource != null && _audioClips.Length > 0 && _audioClips[0] != null)
        {
            _bgmSource.clip = _audioClips[0];
            _bgmSource.loop = true;
            _bgmSource.Play();
        }
    }

    public void PlayGameBgm()
    {
        if (_bgmSource != null && _audioClips.Length > 1 && _audioClips[1] != null)
        {
            _bgmSource.clip = _audioClips[1];
            _bgmSource.loop = true;
            _bgmSource.Play();
        }
    }

    public void PlaySfxSound(int index)
    {
        if (_sfxSource != null && index >= 2 && index < _audioClips.Length && _audioClips[index] != null)
        {
            _sfxSource.PlayOneShot(_audioClips[index]);
        }
    }

    private void OnBgmVolumeChanged(float volume)
    {
        if (_bgmSource != null)
        {
            _bgmSource.volume = volume;
            PlayerPrefs.SetFloat(Constants.BGMVolumeKey, volume);
            PlayerPrefs.Save();
        }
    }

    private void OnSfxVolumeChanged(float volume)
    {
        if (_sfxSource != null)
        {
            _sfxSource.volume = volume;
            PlayerPrefs.SetFloat(Constants.SFXVolumeKey, volume);
            PlayerPrefs.Save();
        }
    }
}
