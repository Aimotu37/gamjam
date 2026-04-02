using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "AudioAction", menuName = "Game/State Actions/Audio Control")]
public class AudioAction : StateAction
{
    public enum AudioCommand { PlaySFX, PlayBGM, StopBGM, FadeBGM }

    [Header("命令设置")]
    public AudioCommand command;
    public AudioClip clip;

    [Header("参数设置")]
    [Range(0f, 1f)] public float volume = 1.0f;
    public float fadeDuration = 1.0f;
    public bool loop = true;

    public override IEnumerator Execute()
    {
        if (AudioManager.Instance == null)
        {
            Debug.LogError("场景中找不到 AudioManager 实例！");
            yield break;
        }

        switch (command)
        {
            case AudioCommand.PlaySFX:
                AudioManager.Instance.PlaySFX(clip, volume);
                break;
            case AudioCommand.PlayBGM:
                AudioManager.Instance.PlayBGM(clip, loop);
                // 如果需要确保音量正确，可以立刻设置音量
                AudioManager.Instance.bgmSource.volume = volume;
                break;
            case AudioCommand.StopBGM:
                // 使用你脚本里的 Fade 功能平滑停止，或者直接停止
                AudioManager.Instance.FadeBGMVolume(0, fadeDuration);
                break;
            case AudioCommand.FadeBGM:
                AudioManager.Instance.FadeBGMVolume(volume, fadeDuration);
                break;
        }

        yield return null;
    }
}