using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class NotebookUI : MonoBehaviour
{
    public static NotebookUI Instance;


    [Header("日记本根面板")]
    public GameObject rootPanel;
    public GameObject pages;
    public GameObject stickyHighlight;

    [Header("日记页面 — 按顺序拖入 Page_Diary1~7")]
    public GameObject[] diaryPages; // 0=日记1, 1=日记2 ... 6=日记7

    public int _currentpage = 0;

    [Header("线性日记场景（如 S7）：跳过 DiaryID 解锁过滤，所有页恒可见")]
    public bool bypassDiaryFilter = false;

    [Header("关闭日记本时触发（最后一页右翻自动关闭也会触发）")]
    public UnityEvent onClose;
    [Header("翻到指定页时触发一次（页码从 0 开始；-1 = 不启用）")]
    public int triggerPageIndex = -1;
    public UnityEvent onTriggerPageShown;

    private bool _triggerPageFired = false;


    private IGameManager GameMgr => (IGameManager)FindAnyObjectByType<SceneManagerBase>();

    private void Awake()
    {
        Instance = this;
        rootPanel.SetActive(false);
        if (stickyHighlight != null) stickyHighlight.SetActive(false);
    }

    private void OnEnable() => GlobalData.OnDiaryUnlocked += HandleDiaryUnlocked;
    private void OnDisable() => GlobalData.OnDiaryUnlocked -= HandleDiaryUnlocked;


    // ── 日记本开关 ──
    public void Open()
    {
        rootPanel.SetActive(true);
        GameMgr?.PushUIBlock("Notebook");
        GameMgr?.PushUIBlock("NotebookUI");
        if (stickyHighlight != null) stickyHighlight.SetActive(true);
        _triggerPageFired = false;
        RefreshDiaryPages();
    }

    public void Close()
    {
        rootPanel.SetActive(false);
        GameMgr?.PopUIBlock("NotebookUI");
        GameMgr?.PopUIBlock("Notebook");
        if (stickyHighlight != null) stickyHighlight.SetActive(false);
        onClose?.Invoke();
    }

    //———日记页打开————
    public void OpenPages()
    {
        pages.SetActive(true);
        GameMgr?.PushUIBlock("DiaryPages");
        RefreshDiaryPages();
    }

    public void GetPageContent()
    {
        diaryPages[_currentpage].SetActive(true);
        TryFireTriggerPage();
    }

    public void ClosePages()
    {
        pages.SetActive(false);
        diaryPages[_currentpage].SetActive(false);
        GameMgr?.PopUIBlock("DiaryPages");
    }

    //———日记页翻页————

    public void TurnPageLeft()
    {

        Debug.Log($"<color=yellow>[Notebook]</color> TurnPageLeft 被调用 当前页={_currentpage} 总页数={(diaryPages == null ? 0 : diaryPages.Length)}");

        if (_currentpage - 1 >= 0)
        {
            diaryPages[_currentpage].SetActive(false);
            diaryPages[_currentpage - 1].SetActive(true);
            _currentpage--;
            TryFireTriggerPage();
        }
    }

    public void TurnPageRight()
    {
        if (_currentpage + 1 < diaryPages.Length)
        {
            Debug.Log($"<color=yellow>[Notebook]</color> TurnPageRight 被调用 当前页={_currentpage} 总页数={(diaryPages == null ? 0 : diaryPages.Length)}");
            diaryPages[_currentpage].SetActive(false);
            diaryPages[_currentpage + 1].SetActive(true);
            _currentpage++;
            TryFireTriggerPage();
        }
        else
        {
            ClosePages();
            Close();
        }
          
        
    }
    // 当前页 == triggerPageIndex 时触发一次 onTriggerPageShown
    private void TryFireTriggerPage()
    {
        if (_triggerPageFired) return;
        if (triggerPageIndex < 0) return;
        if (_currentpage != triggerPageIndex) return;

        _triggerPageFired = true;
        onTriggerPageShown?.Invoke();
    }


    // ── 日记页刷新 ──
    private void RefreshDiaryPages()
    {
        if (diaryPages == null) return;
        if (bypassDiaryFilter) return; // S7 等线性场景：不按 DiaryID 解锁过滤
        DiaryID[] order = {
            DiaryID.Diary1_FishAndBeads,
            DiaryID.Diary2_SnackCart,
            DiaryID.Diary3_SummerTV,
            DiaryID.Diary4_SnackGachaToyPhone,
            DiaryID.Diary5_Stationery,
            DiaryID.Diary6_BooksAndMagazines,
            DiaryID.Diary7_Internet,
        };
        for (int i = 0; i < diaryPages.Length && i < order.Length; i++)
        {
            if (diaryPages[i] != null)
                diaryPages[i].SetActive(GlobalData.IsDiaryUnlocked(order[i]));
        }
    }

    private void HandleDiaryUnlocked(DiaryID id)
    {
        Debug.Log($"[NotebookUI] 日记解锁：{id}");
        if (rootPanel.activeSelf) RefreshDiaryPages();
    }

}