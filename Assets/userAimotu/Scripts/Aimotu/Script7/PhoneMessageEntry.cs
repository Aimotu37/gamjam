using UnityEngine;
using UnityEngine.Video;

// 挂在 PhoneUI Messages 数组里的每一条消息 GameObject 上。
// 图片消息：留空 videoPlayer，组件什么都不做。
// 视频消息：拖入 VideoPlayer，启用时自动从头播放，禁用时停止。
public class PhoneMessageEntry : MonoBehaviour
{
    [Header("视频消息才需要填")]
    public VideoPlayer videoPlayer;

    [Header("可选：进入消息时单独播放的音频")]
    public AudioSource extraAudio;

    [Header("视频播完后是否自动循环")]
    public bool loopVideo = true;

    private void OnEnable()
    {
        if (videoPlayer != null)
        {
            videoPlayer.isLooping = loopVideo;
            videoPlayer.Stop();
            videoPlayer.Play();
        }
        if (extraAudio != null)
        {
            extraAudio.Stop();
            extraAudio.Play();
        }
    }

    private void OnDisable()
    {
        if (videoPlayer != null) videoPlayer.Stop();
        if (extraAudio != null) extraAudio.Stop();
    }
}
