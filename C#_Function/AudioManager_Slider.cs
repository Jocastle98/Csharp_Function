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

    [Header("UI Sliders (Optional)")]
    // 슬라이더가 연결되어 있다면 Inspector에서 할당합니다.
    public Slider bgmSlider;
    public Slider sfxSlider;

    // PlayerPrefs에 저장할 키들
    private const string BGMVolumeKey = "BGMVolume";
    private const string SFXVolumeKey = "SFXVolume";

    void Awake()
    {
        float savedBGMVolume = PlayerPrefs.GetFloat(BGMVolumeKey, 1f);
        float savedSFXVolume = PlayerPrefs.GetFloat(SFXVolumeKey, 1f);

        if (bgmSource != null)
            bgmSource.volume = savedBGMVolume;
        if (sfxSource != null)
            sfxSource.volume = savedSFXVolume;

        if (bgmSlider != null)
        {
            bgmSlider.value = savedBGMVolume;
            bgmSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
        }
        if (sfxSlider != null)
        {
            sfxSlider.value = savedSFXVolume;
            sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        }
    }
    
    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
    }

    /// <summary>
    /// 인트로 BGM 재생
    /// </summary>
    public void PlayIntroBGM()
    {
        if (bgmSource != null && audioClips.Length > 0 && audioClips[0] != null)
        {
            bgmSource.clip = audioClips[0];
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    /// <summary>
    /// 메인 BGM 재생 
    /// </summary>
    public void PlayMainBGM()
    {
        if (bgmSource != null && audioClips.Length > 1 && audioClips[1] != null)
        {
            bgmSource.clip = audioClips[1];
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    /// <summary>
    /// 효과음 재생 
    /// </summary>
    public void PlaySound(int index)
    {
        if (sfxSource != null && index >= 2 && index < audioClips.Length && audioClips[index] != null)
        {
            sfxSource.PlayOneShot(audioClips[index]);
        }
    }

    /// <summary>
    /// 슬라이더로 BGM 볼륨 조절 
    /// </summary>
    /// <param name="value">슬라이더 값</param>
    public void OnBGMVolumeChanged(float value)
    {
        if (bgmSource != null)
        {
            bgmSource.volume = value;
            PlayerPrefs.SetFloat(BGMVolumeKey, value);
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// 슬라이더로 SFX 볼륨 조절 
    /// </summary>
    /// <param name="value">슬라이더 값</param>
    public void OnSFXVolumeChanged(float value)
    {
        if (sfxSource != null)
        {
            sfxSource.volume = value;
            PlayerPrefs.SetFloat(SFXVolumeKey, value);
            PlayerPrefs.Save();
        }
    }
}
