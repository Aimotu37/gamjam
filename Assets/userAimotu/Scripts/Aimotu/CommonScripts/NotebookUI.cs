using UnityEngine;
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
        if (stickyHighlight != null) stickyHighlight.SetActive(true);
        RefreshDiaryPages();
    }

    public void Close()
    {
        rootPanel.SetActive(false);
        GameMgr?.PopUIBlock("Notebook");
        if (stickyHighlight != null) stickyHighlight.SetActive(false);
    }

    //———日记页打开————
    public void OpenPages()
    {

        pages.SetActive(true);
        GameMgr?.PushUIBlock("DiaryPages");
        RefreshDiaryPages();
    }

    public void ClosePages()
    {
        pages.SetActive(false);
        GameMgr?.PopUIBlock("DiaryPages");
    }

    // ── 日记页刷新 ──
    private void RefreshDiaryPages()
    {
        if (diaryPages == null) return;
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