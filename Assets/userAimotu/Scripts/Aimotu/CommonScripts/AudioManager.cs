using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;
    public AudioMixer mainMixer;
    private Coroutine fadeCoroutine; // ???????????????????????????
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // ?????????????????? Canvas ??????????????????????
            if (transform.parent != null) transform.SetParent(null);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void FadeBGMVolume(float targetVolume, float duration)
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(DoFade(targetVolume, duration));
    }
    private IEnumerator DoFade(float targetVolume, float duration)
    {
        float startVolume = bgmSource.volume;
        float timer = 0;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(startVolume, targetVolume, timer / duration);
            yield return null;
        }
        bgmSource.volume = targetVolume;
    }

    // ?????????? (???????)
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip != null) sfxSource.PlayOneShot(clip, volume);
    }

    // ???????????
    public void PlayBGM(AudioClip clip, bool loop = true)
    {
        if (bgmSource.clip == clip) return;
        bgmSource.clip = clip;
        bgmSource.loop = loop;
        bgmSource.Play();
    }

    //???????????
    public void SetBGMVolume(float volume)
    {
       
        bgmSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        float db = Mathf.Log10(Mathf.Clamp(volume,0.0001f, 1f)) * 20f;
        
        mainMixer.SetFloat("SFXVolume", db);
        PlayerPrefs.SetFloat("SFXVolume", db);
        sfxSource.volume = volume;
    }

    public float GetBGMVolume()
    {
        return bgmSource.volume;
    }

    public float GetSFXVolume()
    {
        return PlayerPrefs.GetFloat("SFXVolume");
    }

}

