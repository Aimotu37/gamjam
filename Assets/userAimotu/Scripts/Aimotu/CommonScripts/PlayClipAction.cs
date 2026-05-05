using System.Collections;
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName ="PlayClip", menuName = "Actions/Play Clip")]
public class PlayClipAction : StateAction
{
    [Header("遮罩设置")]
    public bool useFadeIn = true;  // 是否需要进入时变黑
    public bool useFadeOut = true; // 是否需要结束后渐显
    public float fadeSpeed = 2.0f; // 渐变速度

    [Header("音效渐变开关")]
    public bool isAudioFade = true; // 是否开启音频渐隐渐显
    public float targetBGMVolume = 0.6f; // 恢复后的正常音量
    [Header("视频设置")]
    public VideoClip videoClip; // 添加视频剪辑引用
    [Header("音效设置")]
    public AudioClip transitionSFX; //音效

    [Header("播放设置")]
    public bool waitForClip = true;
    public bool loopVideo = false;
    [Header("调试")]
    public bool enableDebugLog = true;
    public bool destroyPlayerOnEnd = false; // 新增：是否在此转场销毁人物

    public override IEnumerator Execute()
    {
        var manager = GetManager();
        if (manager == null) yield break;

        manager.PushUIBlock("VideoClip");

        // 1. 等待 GameManager 初始化
        yield return new WaitUntil(() =>
            manager != null &&
            manager.uiRawImage != null &&
            manager.uiVideoPlayer != null &&
            manager.uiRenderTexture != null &&
            manager.transitionMaskGroup != null
        );

        var rawImage = manager.uiRawImage;
        var videoPlayer = manager.uiVideoPlayer;
        var renderTexture = manager.uiRenderTexture;
        var mask = manager.transitionMaskGroup; // 黑色遮罩
        float duration = 1.0f / fadeSpeed; // 计算实际需要的秒数


        // --- 【新增步骤 1：场景变昏暗】 ---
        if (useFadeIn)
        {

            if (enableDebugLog) Debug.Log("[PlayClip] 开始渐隐(变暗)...");

            if (transitionSFX != null) AudioManager.Instance.PlaySFX(transitionSFX);
            if (isAudioFade && AudioManager.Instance != null)
                AudioManager.Instance.FadeBGMVolume(0.0f, 1f / fadeSpeed);
            yield return FadeManager.Instance.FadeOut(1f / fadeSpeed); // 统一用 FadeManager
        }
        // 2. 检查视频剪辑
        if (videoClip == null)
        {
            Debug.LogError("[PlayAnimation] VideoClip 未赋值！请在 Inspector 中设置视频文件");
            yield break;
        }

        // 3. 配置 VideoPlayer
        videoPlayer.clip = videoClip;
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.targetTexture = renderTexture;
        videoPlayer.isLooping = loopVideo;
        videoPlayer.playOnAwake = false;
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource; // 可选：音频设置

        // 4. 配置 RawImage
        rawImage.texture = renderTexture;
        rawImage.gameObject.SetActive(true);

      //  if (enableDebugLog) Debug.Log("[PlayAnimation] UI 已显示，准备播放视频");

        // 5. 准备视频（重要！）
        videoPlayer.Prepare();

        // 等待视频准备完成
        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }


        // 6. 播放视频
        videoPlayer.Play();
        yield return new WaitForSeconds(0.2f);


        // 7. 等待播放完成
        if (waitForClip)
        {
            bool ended = false;
            void OnEnd(VideoPlayer vp) => ended = true;
            videoPlayer.loopPointReached += OnEnd;

            

            float timeout = 120f;
            float elapsed = 0f;
            while (!ended)
            {
                elapsed += Time.deltaTime;
                if (elapsed > timeout)
                {
                    Debug.LogWarning("[PlayClip] 超时强制退出");
                    break;
                }
                yield return null;
            }

            // 取消注册，防止下次复用时重复触发
            videoPlayer.loopPointReached -= OnEnd; 

            yield return new WaitForSeconds(0.1f);
            if (enableDebugLog) Debug.Log("[PlayClip] 视频播放完成");
        
        }
        if (destroyPlayerOnEnd)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                Destroy(player);
                if (enableDebugLog) Debug.Log("[PlayClip] 人物已销毁");
            }
        }
        videoPlayer.Stop();
        rawImage.gameObject.SetActive(false);
        if (useFadeOut)
        {
            if (isAudioFade) AudioManager.Instance.FadeBGMVolume(targetBGMVolume, 1f / fadeSpeed);
            yield return FadeManager.Instance.FadeIn(1f / fadeSpeed); // 统一用 FadeManager
        }
        // useFadeOut = false 时保持黑屏，什么都不做
        else
        {
            if (enableDebugLog) Debug.Log("[PlayClip] 保持黑屏，等待下一个 Action");

        }
        if (enableDebugLog) Debug.Log("[PlayClip] 全流程执行完成");
        manager.PopUIBlock("VideoClip");


        mask.alpha = 0f;
        mask.blocksRaycasts = false;

   
     
    }
}