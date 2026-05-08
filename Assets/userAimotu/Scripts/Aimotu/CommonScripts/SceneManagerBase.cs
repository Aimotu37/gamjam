// 这是所有 Scene GameManager 的公共基类
//其他场景的 GameManager 只需继承它，写自己独有的部分

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Video;

public abstract class SceneManagerBase : MonoBehaviour, IGameManager
{
    protected abstract RoomState InitialState { get; }

    [Header("Video Setup")]
    [SerializeField] private RawImage _uiRawImage;
    [SerializeField] private VideoPlayer _uiVideoPlayer;
    [SerializeField] private RenderTexture _uiRenderTexture;
    [SerializeField] private CanvasGroup _transitionMaskGroup;
    public RawImage uiRawImage => _uiRawImage;
    public VideoPlayer uiVideoPlayer => _uiVideoPlayer;
    public RenderTexture uiRenderTexture => _uiRenderTexture;
    public CanvasGroup transitionMaskGroup => _transitionMaskGroup;

    // 跨场景传递"继续游戏"状态
    public static int? PendingStateIndex = null;

    [Header("音效")]
    public AudioSource sfxSource;

    [Header("对话")]
    public DialogueManager dialogueManager;
    public DialogueManager Dialog => dialogueManager;

    [Header("立绘 Sprites")]
    public Sprite child_neutral;
    public Sprite child_happy1;
    public Sprite child_happy2;
    public Sprite child_confused;
    public Sprite child_surprised;
    public Sprite child_pout;
    public Sprite adult_tired;
    public Sprite adult_confused;
    public Sprite adult_confusedwithhand;
    public Sprite adult_angry;
    public Sprite adult_suprised;
    public Sprite adult_neutral;

    [Header("状态机事件")]
    public List<RoomStateEvent> roomStateEvents;

    public RoomState CurrentState { get; private set; }
    public static event Action<RoomState> OnRoomStateChanged;

    [Header("UI Block 调试")]
    public int uiBlockCount = 0;
    private Stack<string> uiBlockStack = new Stack<string>();
    public bool IsUIBlocking => uiBlockCount > 0;

    private RoomState _lastFiredState = (RoomState)(-1);
    private int _lastFiredFrame = -1;

    public abstract GameObject TaskModuleObject { get; }
    public TaskModule TaskModule => TaskModuleObject?.GetComponent<TaskModule>();

    // ──────────────────────────────────────────
    //  生命周期
    // ──────────────────────────────────────────
    protected virtual void Awake() { }

    protected virtual void Start()
    {
        uiBlockCount = 0;
        uiBlockStack.Clear();

        // 淡入转场遮罩
        if (_transitionMaskGroup != null)
        {
            _transitionMaskGroup.alpha = 1f;
            _transitionMaskGroup.blocksRaycasts = false;
            StartCoroutine(FadeInTransition());
        }

        // 只调用一次 EnterState
        if (PendingStateIndex.HasValue)
        {
            RoomState savedState = (RoomState)PendingStateIndex.Value;
            PendingStateIndex = null;
            EnterState(savedState);
        }
        else
        {
            EnterState(InitialState);
        }
    }

    protected IEnumerator FadeInTransition(float duration = 0.5f)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            if (_transitionMaskGroup != null)
                _transitionMaskGroup.alpha = 1f - Mathf.Clamp01(elapsed / duration);
            yield return null;
        }
        if (_transitionMaskGroup != null)
            _transitionMaskGroup.alpha = 0f;
    }

    // ──────────────────────────────────────────
    //  UI Block
    // ──────────────────────────────────────────
    public void PushUIBlock(string source = "Unknown")
    {
        uiBlockCount++;
        uiBlockStack.Push(source);
        Debug.Log($"[UIBlock] PUSH by {source} -> {uiBlockCount}");
    }

    public void PopUIBlock(string source = "Unknown")
    {
        if (uiBlockCount <= 0)
        {
            Debug.LogWarning($"[UIBlock] POP by {source} but count already 0");
            return;
        }
        uiBlockCount--;
        if (uiBlockStack.Count > 0) uiBlockStack.Pop();
        Debug.Log($"[UIBlock] POP by {source} -> {uiBlockCount}");
    }

    // ──────────────────────────────────────────
    //  状态机
    // ──────────────────────────────────────────
    public void EnterState(RoomState newState)
    {
        CurrentState = newState;

        // 对话未激活时重置 UIBlock
        if (dialogueManager != null && !dialogueManager.dialoguePanel.activeInHierarchy)
        {
            uiBlockCount = 0;
            uiBlockStack.Clear();
        }

        Debug.Log($"[State] -> {newState}");
        OnRoomStateChanged?.Invoke(newState);
        OnStateEntered(newState);

        if (newState != RoomState.None)
            SaveSystem.Save(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, newState);

        TryPlayStateEvent(newState);
    }

    protected virtual void OnStateEntered(RoomState newState) { }

    private void TryPlayStateEvent(RoomState state)
    {
        if (DialogueManager.instance != null && DialogueManager.instance.IsDialogueActive)
            return;

        // 同一帧内同一状态只触发一次，防止 Start() 双重调用重复执行
        if (state == _lastFiredState && Time.frameCount == _lastFiredFrame)
            return;

        _lastFiredState = state;
        _lastFiredFrame = Time.frameCount;

        foreach (var ev in roomStateEvents)
        {
            if (ev.triggerState == state && ev.actions != null)
                StartCoroutine(ExecuteActionsSequentially(ev.actions));
        }
    }

    private IEnumerator ExecuteActionsSequentially(List<StateAction> actions)
    {
        foreach (var action in actions)
        {
            if (action != null)
                yield return action.Execute();
        }
    }

    // ──────────────────────────────────────────
    //  工具方法
    // ──────────────────────────────────────────
    public void PlayGlobalSFX(AudioClip clip, float volume = 1.0f)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip, volume);
    }

    public Sprite GetCharacterPortrait(PortraitOption option)
    {
        return option switch
        {
            PortraitOption.Child_Neutral => child_neutral,
            PortraitOption.Child_Happy1 => child_happy1,
            PortraitOption.Child_Happy2 => child_happy2,
            PortraitOption.Child_Confused => child_confused,
            PortraitOption.Child_Surprised => child_surprised,
            PortraitOption.Child_Pout => child_pout,
            PortraitOption.Adult_Tired => adult_tired,
            PortraitOption.Adult_Confused => adult_confused,
            PortraitOption.Adult_Confusedwithhand => adult_confusedwithhand,
            PortraitOption.Adult_Angry => adult_angry,
            PortraitOption.Adult_Suprised => adult_suprised,
            PortraitOption.Adult_Neutral => adult_neutral,
            _ => null
        };
    }
}
