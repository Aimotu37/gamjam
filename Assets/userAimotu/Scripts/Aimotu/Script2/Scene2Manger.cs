using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Scene2Manger : SceneManagerBase
{
    public static Scene2Manger Instance { get; private set; }

    [Header("Scene 2 配置")] 
    public string nextSceneName = "Script3";
    public float fadeDuration = 1f;
    
    private bool _isExiting = false;

    // 实现基类要求的抽象属性
    protected override RoomState InitialState => RoomState.None;
    public override GameObject TaskModuleObject => null;

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }

    protected override void Start()
    {
        base.Start();

        if (uiVideoPlayer != null)
        {
            // 确保视频从头播放
            uiVideoPlayer.Stop(); 
            uiVideoPlayer.Play();
            uiVideoPlayer.loopPointReached += OnVideoFinished;
        }
        else
        {
            Debug.LogError("[Scene2] uiVideoPlayer is missing in Inspector!");
        }
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        if (_isExiting) return;
        Debug.Log("[Scene2] Video playback completed. Starting transition...");
        StartCoroutine(FadeAndExit());
    }

    private IEnumerator FadeAndExit()
    {
        _isExiting = true;

        // 1. 锁定 UI
        PushUIBlock("Transition");

        // 2. 渐黑效果
        if (transitionMaskGroup != null)
        {
            transitionMaskGroup.blocksRaycasts = true;
            float elapsed = 0;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                transitionMaskGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
                yield return null;
            }
        }

        // 3. 稍微停顿，给玩家一点反应时间
        yield return new WaitForSeconds(0.3f);

        // 4. 加载场景
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("[Scene2] nextSceneName is empty!");
        }
    }

    private void OnDestroy()
    {
        // 记得取消注册，防止内存泄漏
        if (uiVideoPlayer != null)
        {
            uiVideoPlayer.loopPointReached -= OnVideoFinished;
        }
    }
}