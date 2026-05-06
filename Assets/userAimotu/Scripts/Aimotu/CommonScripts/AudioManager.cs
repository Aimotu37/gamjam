using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;
    private Coroutine fadeCoroutine; // ���ڼ�¼��ǰ���ڽ��еĽ��䣬��ֹ��ͻ
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // �����������ŵ���ĳ�� Canvas ��������£�ǿ���Ƴ�����Ŀ¼
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

    // ����˲ʱ��Ч (�����Ը�)
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip != null) sfxSource.PlayOneShot(clip, volume);
    }

    // ���ű�������
    public void PlayBGM(AudioClip clip, bool loop = true)
    {
        if (bgmSource.clip == clip) return;
        bgmSource.clip = clip;
        bgmSource.loop = loop;
        bgmSource.Play();
    }

    //�������Ʒ���
    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }

    public float GetBGMVolume()
    {
        return bgmSource.volume;
    }

    public float GetSFXVolume()
    {
        return sfxSource.volume;
    }

}

