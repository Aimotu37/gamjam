using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SequenceFrame
{
    public Sprite sprite;
    [Tooltip("切到这一帧时播放的音效（不填则用 defaultFrameSFX）")]
    public AudioClip sfx;
    [Range(0f, 1f)] public float volume = 1f;
}

public class SpriteSequencePlayer : MonoBehaviour
{
    [Header("显示组件（拖 Image 进来）")]
    public Image targetImage;

    [Header("帧序列（按顺序拖入）")]
    public List<SequenceFrame> frames = new List<SequenceFrame>();

    [Header("帧率")]
    public float fps = 8f;

    [Header("是否循环")]
    public bool loop = true;

    [Header("默认帧音效（帧未单独指定时使用）")]
    public AudioClip defaultFrameSFX;
    [Range(0f, 1f)] public float defaultVolume = 1f;

    [Header("音频播放源（留空则用 PlayClipAtPoint）")]
    public AudioSource sfxSource;

    private Coroutine _running;

    private void OnEnable()
    {
        if (targetImage == null || frames == null || frames.Count == 0) return;
        _running = StartCoroutine(Play());
    }

    private void OnDisable()
    {
        if (_running != null) StopCoroutine(_running);
        _running = null;
    }

    private IEnumerator Play()
    {
        int i = 0;
        float interval = 1f / Mathf.Max(0.01f, fps);
        while (true)
        {
            var frame = frames[i];
            if (frame.sprite != null) targetImage.sprite = frame.sprite;
            PlayFrameSFX(frame);
            yield return new WaitForSeconds(interval);
            i++;
            if (i >= frames.Count)
            {
                if (loop) i = 0;
                else { _running = null; yield break; }
            }
        }
    }

    private void PlayFrameSFX(SequenceFrame frame)
    {
        AudioClip clip = frame.sfx != null ? frame.sfx : defaultFrameSFX;
        if (clip == null) return;
        float vol = frame.sfx != null ? frame.volume : defaultVolume;
        if (sfxSource != null) sfxSource.PlayOneShot(clip, vol);
        else AudioSource.PlayClipAtPoint(clip, Vector3.zero, vol);
    }
}