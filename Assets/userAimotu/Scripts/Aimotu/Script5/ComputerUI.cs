using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerUI : MonoBehaviour
{
    public static ComputerUI Instance;

    [Header("电脑+消息页面背景")]
    public GameObject ComputerPannel;
    public GameObject MessageBg;


    [Header("消息 — 按顺序拖入 Message1~7")]
    public GameObject[] Messages; // 0=日记1, 1=日记2 ... 6=日记7

    public int _currentMessage = 0;


    private IGameManager GameMgr => (IGameManager)FindAnyObjectByType<SceneManagerBase>();

    private void Awake()
    {
        Instance = this;
        ComputerPannel.SetActive(false);
    }
    /*
    private void OnEnable() => GlobalData.OnDiaryUnlocked += HandleDiaryUnlocked;
    private void OnDisable() => GlobalData.OnDiaryUnlocked -= HandleDiaryUnlocked;

    */


    // ── 电脑开关 ──
    public void Open()
    {
        ComputerPannel.SetActive(true);
        GameMgr?.PushUIBlock("ComputerUI");

    }

    public void Close()
    {
        ComputerPannel.SetActive(false);
        GameMgr?.PopUIBlock("ComputerUI");

    }

    // 消息弹出
    public void OpenMessageWindow()
    {
        MessageBg.SetActive(true);
        GameMgr?.PushUIBlock("ComputerMessages");
    }

    public void GetMessageContent()
    {
        Messages[_currentMessage].SetActive(true);
    }

    //Next Message

    public void NextMessage()
    {
        if (_currentMessage + 1 < Messages.Length)
        {
            Messages[_currentMessage].SetActive(false);
            Messages[_currentMessage + 1].SetActive(true);
            _currentMessage++;
        }
    }

    /*

    //———消息弹出————
    public void OpenPages()
    {
        pages.SetActive(true);
        GameMgr?.PushUIBlock("ComputerMessages");
        RefreshDiaryPages();
    }

    public void GetPageContent()
    {
        diaryPages[_currentpage].SetActive(true);
    }

    public void CloseMessage()
    {
        pages.SetActive(false);
        diaryPages[_currentpage].SetActive(false);
        GameMgr?.PopUIBlock("ComputerMessages");
    }

    //———日记页翻页————

    public void TurnPageLeft()
    {
        if (_currentpage - 1 >= 0)
        {
            diaryPages[_currentpage].SetActive(false);
            diaryPages[_currentpage - 1].SetActive(true);
            _currentpage--;
        }
    }

    public void TurnPageRight()
    {
        if (_currentpage + 1 < diaryPages.Length)
        {
            diaryPages[_currentpage].SetActive(false);
            diaryPages[_currentpage + 1].SetActive(true);
            _currentpage++;
        }
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

    */

}
