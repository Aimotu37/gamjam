using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;        
using UnityEngine.Video;
using S4;

namespace S61
{
    public class GameManager : MonoBehaviour,IGameManager 
    {
          public static S61.GameManager Instance;
          [Header("Scene Info")]
          public string Name;
          [Header("UI 引用绑定")]
          public S61.Task_S61 taskS61_Instance;
          public GameObject TaskModuleObject => taskS61_Instance != null ? taskS61_Instance.gameObject : null;
          // 实现接口属性
          public S61.Task_S61 TaskS6 => taskS61_Instance;
          public DialogueManager Dialog => dialogueManager;
          [Header("Video Setup")]
          // Inspector 里拖入对应的组件
          [SerializeField] private RawImage _uiRawImage;
          [SerializeField] private VideoPlayer _uiVideoPlayer;
          [SerializeField] private RenderTexture _uiRenderTexture;
          [SerializeField] private CanvasGroup _transitionMaskGroup;        // 在 Inspector 里拖入一个覆盖全屏的黑色 Image 的 CanvasGroup
          public RawImage uiRawImage => _uiRawImage;
          public VideoPlayer uiVideoPlayer => _uiVideoPlayer;
          public RenderTexture uiRenderTexture => _uiRenderTexture;
          public CanvasGroup transitionMaskGroup => _transitionMaskGroup;

          [Header("音效中心")]
          public AudioSource sfxSource;
          public PlayClipAction playClipAction;
          public DialogueManager dialogueManager;
        //  [HideInInspector] public Task_S6 task2;

          public RoomState CurrentState { get; private set; }
          public static event Action<RoomState> OnRoomStateChanged;


          //UI阻断其他交互
          [Header("UI Block Debug")]
          public int uiBlockCount = 0;
          private Stack<string> uiBlockStack = new Stack<string>();
          public bool IsUIBlocking => uiBlockCount > 0;

          // [关键修改] 这里的 List 承载的是 S4 的基类行为，但它认的是 S6 的事件
          public List<RoomStateEvent> roomStateEvents;

          public Sprite child_neutral,    // 幼年主控立绘 无表情
                        child_happy1,      // 幼年主控立绘 开心1
                        child_happy2,   // 幼年主控立绘 开心2
                        child_confused,        // 幼年主控立绘 疑惑
                        child_surprised,  // 幼年主控立绘 惊讶
                        child_pout;       // 幼年主控立绘 不满 / 生气 

          public Sprite GetCharacterPortrait(PortraitOption option)
          {
              switch (option)
              {
                  case PortraitOption.Child_Neutral: return child_neutral;
                  case PortraitOption.Child_Happy1: return child_happy1;
                  case PortraitOption.Child_Happy2: return child_happy2;
                  case PortraitOption.Child_Confused: return child_confused;
                  case PortraitOption.Child_Surprised: return child_surprised;
                  case PortraitOption.Child_Pout: return child_pout;
                  default: return null;
              }
          }


          private void Awake()
          {
              if (Instance == null)
              {
                  Instance = this;
              }
              else Destroy(gameObject);
          }
          private void Start()
          {
              uiBlockCount = 0;
              uiBlockStack.Clear();
              EnterState(RoomState.Dream2_Street);
              Debug.Log($"Screen Size: {Screen.width} x {Screen.height}, Aspect: {(float)Screen.width / Screen.height}");

          }

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
              //Debug.Log($"[UIBlock] POP by {source} (last: {last}) -> {uiBlockCount}");
          }
      


          #region ===== State Control =====
          private void TryPlayStateEvent(RoomState state)
          {
              if (DialogueManager.instance != null && DialogueManager.instance.IsDialogueActive) return; // 当前对话在进行中，延迟状态事件
              foreach (var ev in roomStateEvents)
              {
                  if (ev.triggerState == state && ev.actions != null)
                  {

                      StartCoroutine(ExecuteActionsSequentially(ev.actions));

                  }
              }
          }
          private IEnumerator ExecuteActionsSequentially(List<StateAction> actions)
          {
              foreach (var action in actions)
              {
                  if (action != null)
                      yield return action.Execute(); // 等待完成再执行下一个
              }
          }
          public void EnterState(RoomState newState)
          {
              CurrentState = newState;
              if (dialogueManager != null && !dialogueManager.dialoguePanel.activeInHierarchy)
              {
                  uiBlockCount = 0;
                  uiBlockStack.Clear();
              }
              Debug.Log($"[S6 State] -> {newState}");
              OnRoomStateChanged?.Invoke(newState);

              TryPlayStateEvent(newState);

          }

        #endregion
        // 在 S6.GameManager.cs 内部
        public void PlayGlobalSFX(AudioClip clip, float volume = 1.0f)
        {
            if (clip == null || sfxSource == null) return;

            // PlayOneShot 的第二个参数就是音量比例 (0.0 到 1.0)
            sfxSource.PlayOneShot(clip, volume);
        }
    }

}