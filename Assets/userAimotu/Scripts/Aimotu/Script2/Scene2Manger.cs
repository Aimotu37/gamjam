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
        StopAllCoroutines();
        transitionMaskGroup.alpha = 0f;
        if (uiVideoPlayer != null)
        {
            
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
        
        PushUIBlock("Transition");

       
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

       
        yield return new WaitForSeconds(0.3f);

        
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
        
        if (uiVideoPlayer != null)
        {
            uiVideoPlayer.loopPointReached -= OnVideoFinished;
        }
    }
}