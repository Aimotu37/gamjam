// 这是所有 Scene GameManager 的公共基类
// S4/S6/S61 的 GameManager 只需继承它，写自己独有的部分

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Video;
using S4; // IGameManager、PortraitOption 等公共类型在 S4 命名空间里

public abstract class SceneManagerBase : MonoBehaviour
{
    // ──────────────────────────────────────────
    //  子类必须实现：返回初始状态
    // ──────────────────────────────────────────
    protected abstract RoomState InitialState { get; }

    // ──────────────────────────────────────────
    //  Inspector 字段（每个 Scene 都一样的部分）
    // ──────────────────────────────────────────
    [Header("Video Setup")]
    [SerializeField] private RawImage _uiRawImage;
    [SerializeField] private VideoPlayer _uiVideoPlayer;
    [SerializeField] private RenderTexture _uiRenderTexture;
    [SerializeField] private CanvasGroup _transitionMaskGroup;
    public RawImage uiRawImage => _uiRawImage;
    public VideoPlayer uiVideoPlayer => _uiVideoPlayer;
    public RenderTexture uiRenderTexture => _uiRenderTexture;
    public CanvasGroup transitionMaskGroup => _transitionMaskGroup;

    [Header("音效")]
    public AudioSource sfxSource;
   // public PlayClipAction playClipAction;

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

    [Header("状态机事件")]
    public List<RoomStateEvent> roomStateEvents;

    // ──────────────────────────────────────────
    //  运行时状态
    // ──────────────────────────────────────────
    public RoomState CurrentState { get; private set; }
    public static event Action<RoomState> OnRoomStateChanged;

    [Header("UI Block 调试")]
    public int uiBlockCount = 0;
    private Stack<string> uiBlockStack = new Stack<string>();
    public bool IsUIBlocking => uiBlockCount > 0;

    // ──────────────────────────────────────────
    //  接口实现
    // ──────────────────────────────────────────
    public abstract GameObject TaskModuleObject { get; }

    // ──────────────────────────────────────────
    //  生命周期
    // ──────────────────────────────────────────
    protected virtual void Awake()
    {
        // 子类可以 override Awake() 并调用 base.Awake()
        // 在这里注册单例由子类自己做（因为类型不同）
    }

    protected virtual void Start()
    {
        uiBlockCount = 0;
        uiBlockStack.Clear();
        // 确保转场遮罩初始不拦截点击
        if (_transitionMaskGroup != null)
        {
            _transitionMaskGroup.alpha = 0;
            _transitionMaskGroup.blocksRaycasts = false;
        }
        EnterState(InitialState);
        //Debug.Log($"Screen Size: {Screen.width} x {Screen.height}");
    }

    // ──────────────────────────────────────────
    //  UI Block（完全共享，不需要再写了）
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
    //  状态机（完全共享）
    // ──────────────────────────────────────────
    public void EnterState(RoomState newState)
    {
        CurrentState = newState;

        // 重置 UIBlock（对话面板不在的时候）
        if (dialogueManager != null && !dialogueManager.dialoguePanel.activeInHierarchy)
        {
            uiBlockCount = 0;
            uiBlockStack.Clear();
        }

        Debug.Log($"[State] -> {newState}");
        OnRoomStateChanged?.Invoke(newState);

        // S4 特有：刷新所有可交互物件
        OnStateEntered(newState);

        TryPlayStateEvent(newState);
    }

    // 子类可以 override 这里来加场景特有逻辑（比如 S4 的 RefreshInteractable）
    protected virtual void OnStateEntered(RoomState newState) { }

    private void TryPlayStateEvent(RoomState state)
    {
        if (DialogueManager.instance != null && DialogueManager.instance.IsDialogueActive)
            return;

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
    //  工具方法（完全共享）
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
            PortraitOption.Child_Neutral   => child_neutral,
            PortraitOption.Child_Happy1    => child_happy1,
            PortraitOption.Child_Happy2    => child_happy2,
            PortraitOption.Child_Confused  => child_confused,
            PortraitOption.Child_Surprised => child_surprised,
            PortraitOption.Child_Pout      => child_pout,
            _                              => null
        };
    }
}
