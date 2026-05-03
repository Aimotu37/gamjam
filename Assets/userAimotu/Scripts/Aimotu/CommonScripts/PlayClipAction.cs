using System.Collections;
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName ="PlayClip", menuName = "Actions/Play Clip")]
public class PlayClipAction : StateAction
{
    [Header("淡入淡出")]
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

    [Header("遮罩处理")]
    // true  = 播完保留黑幕，由后续 SceneTransitionAction 处理（入睡过场用）
    // false = 播完按 useFadeOut 决定是否淡出（闪屏 / 普通过场用）
    public bool keepMaskAfterPlay = false;
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


        // --- 【新增步骤 1：淡入】 ---
        if (useFadeIn)
        {
            if (enableDebugLog) Debug.Log("[PlayClip] 开始渐隐(变暗)...");

            if (transitionSFX != null) AudioManager.Instance.PlaySFX(transitionSFX);
            if (isAudioFade && AudioManager.Instance != null) 
                AudioManager.Instance.FadeBGMVolume(0.0f, duration);
           // GameManager.Instance.StartCoroutine(AudioManager.Instance.FadeBGM(0, fadeSpeed));
            while (mask.alpha < 1.0f)
            {
                mask.alpha = Mathf.MoveTowards(mask.alpha, 1.0f, fadeSpeed * Time.deltaTime);
                yield return null;
            }
        }
        else
        {
            // 如果不需要渐隐过程，但视频需要黑底，直接把 alpha 设为 1
            mask.alpha = 1.0f;
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


        // 5.  准备视频（先 Prepare 再显示，避免旧帧闪烁）
        videoPlayer.Prepare();
        while (!videoPlayer.isPrepared)
            yield return null;

        // 清空 RenderTexture，防止上一个视频的残帧闪现
        RenderTexture.active = renderTexture;
        GL.Clear(true, true, Color.black);
        RenderTexture.active = null;

        rawImage.texture = renderTexture;
        rawImage.gameObject.SetActive(true);
        // 6. 播放视频
        bool videoFinished = false;
        VideoPlayer.EventHandler onLoopPoint = _ => videoFinished = true;
        videoPlayer.loopPointReached += onLoopPoint;
        videoPlayer.Play();
        yield return new WaitForSeconds(0.2f);


        // 7. 等待播放完成
        if (waitForClip)
        {
            yield return new WaitUntil(() => videoFinished);

            if (enableDebugLog) Debug.Log("[PlayClip] 视频播放完成");
        }
        else
        {
            if (enableDebugLog) Debug.Log("[PlayClip] 跳过等待，立即继续");
            yield return null;
        }
        videoPlayer.loopPointReached -= onLoopPoint;
        // 8. 销毁玩家（可选）
        if (destroyPlayerOnEnd)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                Destroy(player);
                if (enableDebugLog) Debug.Log("[PlayClip] 人物已销毁");
            }
        }
        // 9. 停止视频，隐藏 RawImage，清空 texture 防止残帧
        videoPlayer.Stop();
        rawImage.texture = null;
        rawImage.gameObject.SetActive(false);
        // 10. 遮罩处理
        if (keepMaskAfterPlay)
        {
            // 保留黑幕：后续 SceneTransitionAction 负责处理遮罩
            if (enableDebugLog) Debug.Log("[PlayClip] 保留遮罩，等待后续转场");
        }
        else if (useFadeOut)
        {

            if (enableDebugLog) Debug.Log("[PlayClip] 开始渐显(恢复)...");
           if (isAudioFade && AudioManager.Instance != null)
                AudioManager.Instance.FadeBGMVolume(targetBGMVolume, duration);
            while (mask.alpha > 0.0f)
            {
                mask.alpha = Mathf.MoveTowards(mask.alpha, 0.0f, fadeSpeed * Time.deltaTime);
                yield return null;
            }
        }
        else
        {
            // 如果不需要渐显，直接把黑屏关掉
            mask.alpha = 0.0f;
            mask.blocksRaycasts = false;
        }
        if (enableDebugLog) Debug.Log("[PlayClip] 全流程执行完成");
        manager.PopUIBlock("VideoClip");

    }
}